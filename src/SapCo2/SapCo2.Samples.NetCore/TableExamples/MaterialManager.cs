using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Enumeration;
using SapCo2.Mm;
using SapCo2.Query;
using SapCo2.Samples.NetCore.Extensions;
using SapCo2.Samples.NetCore.TableExamples.Models;

namespace SapCo2.Samples.NetCore.TableExamples
{
    public sealed class MaterialManager
    {
        private readonly IServiceProvider _serviceProvider;
        private const int PartitionCount = 50;

        public MaterialManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<List<Material>> GetMaterialsByPrefixAsync(string materialCodePrefix,
            MaterialQueryOptions options = null, bool getUnsafeFields = true, int rowCount = 0)
        {
            options ??= new MaterialQueryOptions();
            var query = new AbapQuery().Set(QueryOperator.StartsWith("MATNR", materialCodePrefix))
                .And(QueryOperator.NotEqual("LVORM", true, RfcTablePropertySapTypes.BOOLEAN_X)).GetQuery();

            using IRfcConnection connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();

            using IReadTable<Material> tableFunction = _serviceProvider.GetService<IReadTable<Material>>();
            List<Material> result = tableFunction.GetTable(connection, query, rowCount: rowCount, getUnsafeFields: getUnsafeFields);

            return await SetOptionsAsync(result, options, getUnsafeFields).ConfigureAwait(false);
        }

        public void Print(List<Material> materials)
        {
            if (!materials.Any())
                Console.WriteLine("Material Not Found!");

            Console.WriteLine($"======= Material List ================");
            Console.WriteLine("".PadLeft(20, '='));
            foreach (var material in materials)
                Console.WriteLine($"{material.Code}\t{material.Definition.Definition}\t{material.MaterialCategoryCode}-{material.MaterialCategory?.Definition}");
        }


        #region Private Methods

        private async Task<List<Material>> SetOptionsAsync(List<Material> materialList,
           MaterialQueryOptions queryOptions, bool getUnsafeFields = true)
        {
            if (!materialList.Any())
                return materialList;

            var taskList = new List<Task>();
            var definitionList = new ConcurrentQueue<MaterialDefinition>();
            var materialCategoryDefinitionList = new ConcurrentQueue<MaterialCategoryDefinition>();

            var materialDefinitionTaskList = SetMaterialDefinitionOption(queryOptions, materialList, definitionList);
            if (materialDefinitionTaskList.Any())
                taskList.AddRange(materialDefinitionTaskList);

            var materialCategoryTaskList =
                SetMaterialCategoryOption(queryOptions, materialList, materialCategoryDefinitionList);
            if (materialCategoryTaskList.Any())
                taskList.AddRange(materialCategoryTaskList);

            if (!taskList.Any())
                return materialList;

            await Task.WhenAll(taskList).ConfigureAwait(false);

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

        private List<Task> SetMaterialDefinitionOption(MaterialQueryOptions queryOptions, List<Material> materialList,
            ConcurrentQueue<MaterialDefinition> definitionList)
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

        private List<Task> SetMaterialCategoryOption(MaterialQueryOptions queryOptions, List<Material> materialList,
            ConcurrentQueue<MaterialCategoryDefinition> materialCategoryDefinitionList)
        {
            var taskList = new List<Task>();
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
                        using var connection = _serviceProvider.GetService<IRfcConnection>();
                        connection.Connect();
                        using var tableFunction = _serviceProvider.GetService<IReadTable<MaterialCategoryDefinition>>();
                        var definitions = tableFunction.GetTable(connection, queryList);
                        foreach (MaterialCategoryDefinition definition in definitions)
                            materialCategoryDefinitionList.Enqueue(definition);
                    }));
                }
            }

            return taskList;
        }

        #endregion
    }
}
