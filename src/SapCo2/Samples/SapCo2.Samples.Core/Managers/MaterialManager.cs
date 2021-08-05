using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Abstraction.Enumerations;
using SapCo2.Query;
using SapCo2.Samples.Core.Abstracts;
using SapCo2.Samples.Core.Extensions;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Managers
{
    public class MaterialManager : IMaterialManager
    {
        #region variables

        private readonly IServiceProvider _serviceProvider;
        private const int PartitionCount = 500;
        private const int MaxConcurrentThreadCount = 5;

        #endregion

        #region Methods

        #region Constructor

        public MaterialManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Interface Implementation


        public async Task<Material> GetMaterialAsync(string materialCode)
        {

            List<string> whereClause = new AbapQuery().Set(QueryOperator.Equal("MATNR", materialCode))
                .And(QueryOperator.NotEqual("LVORM", true, RfcDataTypes.BOOLEAN_X)).GetQuery();

            using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
            return (await sapClient.GetTableDataAsync<Material>(whereClause)).FirstOrDefault();
        }
        public async Task<Material> GetMaterialWithSubTableAsync(string materialCode)
        {
            Material material = await GetMaterialAsync(materialCode);
            return (await SetOptionsAsync(new List<Material> { material }, new MaterialQueryOptions { IncludeAll = true })).FirstOrDefault();
        }
        public async Task<List<Material>> GetMaterialsByPrefixAsync(string materialCodePrefix, int recordCount = 5)
        {
            List<string> whereClause = new AbapQuery().Set(QueryOperator.StartsWith("MATNR", materialCodePrefix))
                .And(QueryOperator.NotEqual("LVORM", true, RfcDataTypes.BOOLEAN_X)).GetQuery();

            using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
            return await sapClient.GetTableDataAsync<Material>(whereClause, rowCount: recordCount,includeUnsafeFields:true);
        }
        public async Task<List<Material>> GetMaterialsByPrefixWithSubTablesAsync(string materialCodePrefix, int recordCount = 5)
        {
            List<Material> materials = await GetMaterialsByPrefixAsync(materialCodePrefix, recordCount);
            return await SetOptionsAsync(materials, new MaterialQueryOptions { IncludeAll = true });
        }

        #region IPrintable Interface Implemantation

        public void Print(List<Material> model)
        {
            if (!model?.Any() ?? true)
            {
                Console.WriteLine("\n Material Not Found!");
                return;
            }


            Console.WriteLine($"\n======= Material List ================");
            Console.WriteLine("".PadLeft(20, '='));

            foreach (Material material in model)
                Console.WriteLine($"{material.Code}\t{material.Definition?.Definition}\t{material.MaterialCategoryCode}-{material.MaterialCategory?.Definition}");
        }
        public void Print(Material model)
        {
            if (model == null)
            {
                Console.WriteLine("\nMaterial Not Found!");
                return;
            }
            Print(new List<Material> { model });
        }

        #endregion

        #endregion

        #region Private Methods

        private async Task<List<Material>> SetOptionsAsync(List<Material> materialList,
           MaterialQueryOptions queryOptions, bool getUnsafeFields = true)
        {
            if (!materialList.Any())
                return materialList;

            var taskList = new List<Task>();
            var definitionList = new ConcurrentQueue<MaterialDefinition>();
            var materialCategoryDefinitionList = new ConcurrentQueue<MaterialCategoryDefinition>();
            using (var semaphoreSlim = new SemaphoreSlim(MaxConcurrentThreadCount))
            {
                List<Task> materialDefinitionTaskList = SetMaterialDefinitionOptionAsync(queryOptions, materialList, definitionList, semaphoreSlim);
                if (materialDefinitionTaskList.Any())
                    taskList.AddRange(materialDefinitionTaskList);

                List<Task> materialCategoryTaskList =
                    SetMaterialCategoryOptionAsync(queryOptions, materialList, materialCategoryDefinitionList, semaphoreSlim);
                if (materialCategoryTaskList.Any())
                    taskList.AddRange(materialCategoryTaskList);

                if (!taskList.Any())
                    return materialList;

                await Task.WhenAll(taskList).ConfigureAwait(false);
            }

            ILookup<string, MaterialDefinition> materialDefinitionLookup = definitionList.ToLookup(x => x.Code);
            ILookup<string, MaterialCategoryDefinition> materialCategoryLookup =
                materialCategoryDefinitionList.ToLookup(x => x.Code);

            foreach (Material material in materialList)
            {
                material.Definition = materialDefinitionLookup.Contains(material.Code)
                    ? materialDefinitionLookup[material.Code].FirstOrDefault()
                    : null;
                material.MaterialCategory = materialCategoryLookup.Contains(material.MaterialCategoryCode)
                    ? materialCategoryLookup[material.MaterialCategoryCode].FirstOrDefault()
                    : null;
            }

            return materialList;
        }

        private List<Task> SetMaterialDefinitionOptionAsync(MaterialQueryOptions queryOptions, List<Material> materialList,
            ConcurrentQueue<MaterialDefinition> definitionList, SemaphoreSlim semaphoreSlim = null)
        {
            var taskList = new List<Task>();
            if (!queryOptions.IncludeDefinition)
                return taskList;

            var list = materialList.Where(x => x.Code != null).Select(x => (object)x.Code).ToList();
            List<object>[] parts = list.Partition(PartitionCount);

            foreach (List<object> part in parts)
            {
                List<string> query = new AbapQuery()
                    .Set(QueryOperator.In("MATNR", part))
                    .GetQuery();

                taskList.Add(Task.Run(async () =>
                {
                    try
                    {
                        if (semaphoreSlim != null)
                            await semaphoreSlim.WaitAsync();
                        using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
                        List<MaterialDefinition> definitions = await client.GetTableDataAsync<MaterialDefinition>(query, includeUnsafeFields: true);
                        foreach (MaterialDefinition definition in definitions)
                            definitionList.Enqueue(definition);
                    }
                    finally
                    {
                        if (semaphoreSlim != null)
                            semaphoreSlim.Release();
                    }
                }));
            }

            return taskList;
        }

        private List<Task> SetMaterialCategoryOptionAsync(MaterialQueryOptions queryOptions, List<Material> materialList,
            ConcurrentQueue<MaterialCategoryDefinition> materialCategoryDefinitionList, SemaphoreSlim semaphoreSlim = null)
        {
            var taskList = new List<Task>();
            if (!queryOptions.IncludeMaterialCategory)
                return taskList;

            var materialCategoryList =
                materialList.Where(x => x.MaterialCategoryCode != null)
                    .Select(x => (object)x.MaterialCategoryCode)
                    .Distinct()
                    .ToList();

            List<object>[] materialCategoryParts = materialCategoryList.Partition(PartitionCount);

            foreach (List<object> part in materialCategoryParts)
            {
                List<string> queryList = new AbapQuery()
                    .Set(QueryOperator.In("ZZEXTWG", part))
                    .GetQuery();

                taskList.Add(Task.Run(async () =>
                {
                    try
                    {
                        if (semaphoreSlim != null)
                            await semaphoreSlim.WaitAsync();
                        using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
                        List<MaterialCategoryDefinition> definitions = await client.GetTableDataAsync<MaterialCategoryDefinition>(queryList, includeUnsafeFields: true);
                        foreach (MaterialCategoryDefinition definition in definitions)
                            materialCategoryDefinitionList.Enqueue(definition);
                    }
                    finally
                    {
                        if (semaphoreSlim != null)
                            semaphoreSlim.Release();
                    }
                }));
            }

            return taskList;
        }

        #endregion

        #endregion
    }
}
