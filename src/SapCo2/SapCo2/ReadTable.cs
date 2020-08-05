using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Helper;
using SapCo2.Models;
using SapCo2.Wrapper.Attributes;
// ReSharper disable SuggestVarOrType_Elsewhere

namespace SapCo2.Core
{
    public class ReadTable<T> : IReadTable<T> where T : class
    {
        private const string ReadTableFunctionName = "RFC_READ_TABLE";

        private readonly IPropertyCache _cache;
        private readonly IRfcFunction _function;

        public ReadTable(IPropertyCache cache, IRfcFunction function)
        {
            _cache = cache;
            _function = function;
        }

      
        public List<T> GetTable(IRfcConnection connection, List<string> whereClause = null, bool getUnsafeFields = false, int rowCount = 0, int rowSkips = 0, string delimiter = "|", string noData = "")
        {

            using var function = _function.CreateFunction(connection, ReadTableFunctionName);

            var tableFields = GetTableFields(typeof(T), getUnsafeFields);
            var tableName = GetSapTableName<T>();

            var inputParameters = new RfcReadTableInputParameter
            {
                Query = tableName,
                Delimiter = delimiter,
                NoData = noData,
                RowCount = rowCount,
                RowSkips = rowSkips,
                Fields = tableFields?.Select(x => new RfcReadTableField { FieldName = x })?.ToArray(),
                Options = whereClause?.Select(x => new RfcReadTableOption { Text = x }).ToArray()
            };

            var result = function.Invoke<RfcReadTableOutputParameter>(inputParameters);
            return ConvertToList(result, delimiter, tableFields);

        }
        public void Dispose()
        {
        }

        #region Private Methods

        

        #endregion
        private string GetSapTableName<TEntity>()
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(TEntity));
            var attribute = (RfcClassAttribute)attributes.FirstOrDefault(p => p is RfcClassAttribute);

            return attribute?.Name;
        }
        private List<string> GetTableFields(Type type, bool getUnsafeFields)
        {
            var fieldList = new List<string>();

            foreach (PropertyInfo p in type.GetProperties().Where(x => !x.GetGetMethod().IsVirtual))
            {
                object attributes = p.GetCustomAttributes(true).FirstOrDefault(x => x is RfcPropertyAttribute);

                if (attributes == null)
                    continue;

                if (((RfcPropertyAttribute)attributes).IsPartial)
                    fieldList.AddRange(GetTableFields(p.PropertyType, getUnsafeFields));
                else
                {
                    if (getUnsafeFields)
                        fieldList.Add(((RfcPropertyAttribute)attributes).Name);
                    else
                    {
                        if (!((RfcPropertyAttribute)attributes).Unsafe)
                            fieldList.Add(((RfcPropertyAttribute)attributes).Name);
                    }
                }
            }

            return fieldList.OrderBy(x => x).ToList();
        }
        private List<T> ConvertToList(RfcReadTableOutputParameter outputParameter, string delimiter, List<string> fieldList)
        {
            return outputParameter.Data?.Select(x => ConvertTo(x, delimiter, fieldList))?.ToList() ?? Activator.CreateInstance<List<T>>();
        }
        private T ConvertTo(RfcReadTableData line, string delimiter, List<string> fieldList)
        {
            T instance = Activator.CreateInstance<T>();

            if (line == null)
                return instance;

            if (string.IsNullOrEmpty(line.Wa))
                return instance;

            if (line.Wa.IndexOf(delimiter, StringComparison.Ordinal) <= -1)
                return instance;
            var values = line.Wa.Split(delimiter.ToCharArray());
            for (int i = 0; i < fieldList.Count; i++)
                SetValue(fieldList[i], values[i], instance);

            return instance;
        }
        private void SetValue(string fieldName, object value, object baseInstance, bool lookForPartial = true)
        {
            PropertyInfo property = _cache.GetPropertyInfo(baseInstance.GetType(), fieldName);

            if (property == null)
            {
                if (!lookForPartial)
                    return;

                var propertyList = baseInstance.GetType().GetProperties().Where(x =>
                    ((RfcPropertyAttribute)(x.GetCustomAttributes(true)
                        .First(y => y is RfcPropertyAttribute))).IsPartial && !x.GetGetMethod().IsVirtual).ToList();

                foreach (PropertyInfo prop in propertyList)
                {
                    object base1 = baseInstance.GetType().GetProperty(prop.Name)?.GetValue(baseInstance);

                    if (base1 == null)
                    {
                        object subInstance = Activator.CreateInstance(prop.PropertyType);

                        prop.SetValue(baseInstance, subInstance);
                        SetValue(fieldName, value, subInstance);
                    }
                    else
                        SetValue(fieldName, value, base1);
                }
            }
            else
            {
                object attribute = property.GetCustomAttributes(true).FirstOrDefault(x => x is RfcPropertyAttribute);

                if (attribute == null)
                    return;

                var attr = ((RfcPropertyAttribute)attribute);

                if (attr.Name != fieldName)
                    return;

                string subTypeFieldName = attr.SubTypePropertyName;

                if (!string.IsNullOrEmpty(subTypeFieldName))
                {
                    Type subType = property.PropertyType;
                    object subInstance = Activator.CreateInstance(subType);
                    PropertyInfo subProperty = subInstance.GetType().GetProperty(subTypeFieldName);

                    subProperty?.SetValue(subInstance, value, null);
                    property.SetValue(baseInstance, subInstance);
                }
                else
                {
                    if (!string.IsNullOrEmpty(((RfcPropertyAttribute)attribute).Name))
                        TypeConversionHelper.ConvertFromRfcType(baseInstance, property, value, attr.DataType);
                }
            }
        }

        
    }
}
