using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.MaterialManagement.Entity;
using SapCo2.MaterialManagement.Extensions;
using SapCo2.Query;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement
{
    public class MaterialManager
    {
        private const int MaxDegreeOfParallelism = 5;
        private const int PartitionCount = 50;
        private readonly IServiceProvider _serviceProvider;

        public MaterialManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Material> GetMaterialAsync(string materialCode,MaterialQueryOptions options=null, bool getUnsafeFields = true)
        {
            options ??= new MaterialQueryOptions();

            var query = new AbapQuery().Set(QueryOperator.Equal("MATNR", materialCode))
                .And(QueryOperator.NotEqual("LVORM", true, RfcDataTypes.BooleanX)).GetQuery();

            using var connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();

            using var tableFunction = _serviceProvider.GetService<IReadTable<Material>>();
            var result = tableFunction.GetTable(connection, query, rowCount: 1,getUnsafeFields: getUnsafeFields);

            var materials = await SetOptionsAsync(result, options, getUnsafeFields).ConfigureAwait(false);
            return materials.FirstOrDefault();

        }

        private async Task<List<Material>> SetOptionsAsync(List<Material> materialList, MaterialQueryOptions queryOptions, bool getUnsafeFields = true)
        {
            if (!materialList.Any())
                return materialList;

            var taskList = new List<Task>();
            var definitionList = new ConcurrentQueue<MaterialDefinition>();
            var materialCategoryDefinitionList = new ConcurrentQueue<MaterialCategoryDefinition>();

            using var semaphoreSlim = new SemaphoreSlim(MaxDegreeOfParallelism);

            #region Material Definition

            taskList.AddRange(SetMaterialDefinitionOption(queryOptions, materialList, definitionList));

            #endregion

            #region Material Category

            if (queryOptions.IncludeMaterialCategory)
            {
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
                        await semaphoreSlim.WaitAsync().ConfigureAwait(false);
                        using var connection = _serviceProvider.GetService<IRfcConnection>();
                        connection.Connect();
                        using var tableFunction = _serviceProvider.GetService<IReadTable<MaterialCategoryDefinition>>();
                        var definitions = tableFunction.GetTable(connection, queryList);
                        foreach (MaterialCategoryDefinition definition in definitions)
                            materialCategoryDefinitionList.Enqueue(definition);

                        semaphoreSlim.Release();
                    }));
                }
            }

            #endregion

            if (!taskList.Any())
                return materialList;

            await Task.WhenAll(taskList).ConfigureAwait(false);


            ILookup<string, MaterialDefinition> materialDefinitionLookup = definitionList.ToLookup(x => x.Code);
            ILookup<string, MaterialCategoryDefinition> materialCategoryLookup = materialCategoryDefinitionList.ToLookup(x => x.Code);

            foreach (Material material in materialList)
            {
                material.Definition = materialDefinitionLookup.Contains(material.Code) ? materialDefinitionLookup[material.Code].FirstOrDefault() : null;
                material.MaterialCategory = materialCategoryLookup.Contains(material.MaterialCategoryCode) ? materialCategoryLookup[material.MaterialCategoryCode].FirstOrDefault() : null;
            }

            return materialList;
        }

        private List<Task> SetMaterialDefinitionOption(MaterialQueryOptions queryOptions,List<Material> materialList,ConcurrentQueue<MaterialDefinition> definitionList)
        {
            var taskList = new List<Task>();
            if (queryOptions.IncludeDefinition)
            {
                var list = materialList.Where(x => x.Code != null).Select(x => (object)x.Code).ToList();
                List<object>[] parts = list.Partition(PartitionCount);

                foreach (List<object> part in parts)
                {
                    List<string> query = new AbapQuery()
                        .Set(QueryOperator.In("MATNR", part))
                        .GetQuery();

                    taskList.Add(Task.Run(() =>
                    {
                        using var connection = _serviceProvider.GetService<IRfcConnection>();
                        connection.Connect();
                        using var tableFunction = _serviceProvider.GetService<IReadTable<MaterialDefinition>>();
                        var definitions = tableFunction.GetTable(connection, query);
                        foreach (MaterialDefinition definition in definitions)
                            definitionList.Enqueue(definition);
                    }));
                }
            }
            return taskList;
        }
    }
}
