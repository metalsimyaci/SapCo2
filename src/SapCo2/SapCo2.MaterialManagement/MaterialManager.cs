using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Enumeration;
using SapCo2.MaterialManagement.Abstract;
using SapCo2.MaterialManagement.Entity;
using SapCo2.MaterialManagement.Extensions;
using SapCo2.Query;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.MaterialManagement
{
    public class MaterialManager : IMaterialManager
    {
        #region Variables

        private const int MaxDegreeOfParallelism = 5;
        private const int PartitionCount = 50;
        private readonly IServiceProvider _serviceProvider;
        
        #endregion

        public MaterialManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Material> GetMaterialAsync(string materialCode, MaterialQueryOptions options = null,
            bool getUnsafeFields = true)
        {
            options ??= new MaterialQueryOptions();

            var query = new AbapQuery().Set(QueryOperator.Equal("MATNR", materialCode))
                .And(QueryOperator.NotEqual("LVORM", true, RfcTablePropertySapTypes.BOOLEAN_X)).GetQuery();

            using var connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();

            using var tableFunction = _serviceProvider.GetService<IReadTable<Material>>();
            var result = tableFunction.GetTable(connection, query, rowCount: 1, getUnsafeFields: getUnsafeFields);

            var materials = await SetOptionsAsync(result, options, getUnsafeFields).ConfigureAwait(false);
            return materials.FirstOrDefault();
        }

        public async Task<List<Material>> GetMaterialsAsync(string materialCodePrefix,
            MaterialQueryOptions options = null, bool getUnsafeFields = true, int rowCount = 0)
        {
            options ??= new MaterialQueryOptions();
            var query = new AbapQuery().Set(QueryOperator.StartsWith("MATNR", materialCodePrefix))
                .And(QueryOperator.NotEqual("LVORM", true, RfcTablePropertySapTypes.BOOLEAN_X)).GetQuery();
            
            using var connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();
            
            using var tableFunction = _serviceProvider.GetService<IReadTable<Material>>();
            var result = tableFunction.GetTable(connection, query, rowCount: rowCount, getUnsafeFields: getUnsafeFields);

            return await SetOptionsAsync(result, options, getUnsafeFields).ConfigureAwait(false);
            
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
