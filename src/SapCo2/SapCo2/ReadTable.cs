using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SapCo2.Abstract;
using SapCo2.Attributes;
using SapCo2.Core.Abstract;
using SapCo2.Helper;
using SapCo2.Parameters;
using SapCo2.Wrapper.Abstract;

namespace SapCo2
{
    public class ReadTable<T> : RfcFunctionBase,IReadTable<T> where T : class
    {
        private const string ReadTableFunctionName = "RFC_READ_TABLE";
        private readonly IPropertyCache _cache;

        public ReadTable(IPropertyCache cache, IRfcInterop interop):base(interop)
        {
            _cache = cache;
        }

        public List<T> GetTable(IRfcConnection connection, List<string> whereClause = null, bool getUnsafeFields = false, int rowCount = 0, int rowSkips = 0, string delimiter = "|", string noData = "")
        {
            using var function = CreateFunction(connection, ReadTableFunctionName);

            var tableFields = GetTableFields(typeof(T), getUnsafeFields);
            var tableName = GetTableName<T>();

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
        public T GetStruct(IRfcConnection connection, List<string> whereClause = null, bool getUnsafeFields = false,string delimiter = "|", string noData = "")
        {
            using var function = CreateFunction(connection, ReadTableFunctionName);

            List<string> tableFields = GetTableFields(typeof(T), getUnsafeFields);
            var tableName = GetTableName<T>();

            var inputParameters = new RfcReadTableInputParameter
            {
                Query = tableName,
                Delimiter = delimiter,
                NoData = noData,
                RowCount = 1,
                RowSkips = 0,
                Fields = tableFields?.Select(x => new RfcReadTableField { FieldName = x })?.ToArray(),
                Options = whereClause?.Select(x => new RfcReadTableOption { Text = x }).ToArray()
            };

            RfcReadTableOutputParameter result = function.Invoke<RfcReadTableOutputParameter>(inputParameters);
            return ConvertToStruct(result, delimiter, tableFields);

        }

        
        #region Private Methods
        
        private string GetTableName<TEntity>()
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(TEntity));
            var attribute = (RfcTableAttribute)attributes.FirstOrDefault(p => p is RfcTableAttribute);

            return attribute?.Name;
        }

        private List<string> GetTableFields(Type type, bool getUnsafeFields)
        {
            var fieldList = new List<string>();

            foreach (PropertyInfo p in type.GetProperties().Where(x => !x.GetGetMethod().IsVirtual))
            {
                object attributes = p.GetCustomAttributes(true).FirstOrDefault(x => x is RfcTablePropertyAttribute);

                if (attributes == null)
                    continue;

                if (((RfcTablePropertyAttribute)attributes).IsPartial)
                    fieldList.AddRange(GetTableFields(p.PropertyType, getUnsafeFields));
                else
                {
                    if (getUnsafeFields)
                        fieldList.Add(((RfcTablePropertyAttribute)attributes).Name);
                    else
                    {
                        if (!((RfcTablePropertyAttribute)attributes).Unsafe)
                            fieldList.Add(((RfcTablePropertyAttribute)attributes).Name);
                    }
                }
            }

            return fieldList.OrderBy(x => x).ToList();
        }
        
        private List<T> ConvertToList(RfcReadTableOutputParameter outputParameter, string delimiter, List<string> fieldList)
        {
            return outputParameter.Data?.Select(x => ConvertTo(x, delimiter, fieldList))?.ToList() ?? Activator.CreateInstance<List<T>>();
        }

        private T ConvertToStruct(RfcReadTableOutputParameter outputParameter, string delimiter, List<string> fieldList)
        {
            if (outputParameter?.Data == null)
                return null;
            if (outputParameter.Data.Length < 1)
                return Activator.CreateInstance<T>();

            return ConvertTo(outputParameter.Data.First(), delimiter, fieldList);
        }
        
        private T ConvertTo(RfcReadTableData line, string delimiter, List<string> fieldList)
        {
            T instance = Activator.CreateInstance<T>();

            if (line == null)
                return instance;

            return ConvertTo(line.Wa, delimiter, fieldList, instance);
        }

        private T ConvertTo(string line, string delimiter, List<string> fieldList,T instance)
        {
            if (string.IsNullOrEmpty(line))
                return instance;

            if (line.IndexOf(delimiter, StringComparison.Ordinal) <= -1)
                return instance;

            var values = line.Split(delimiter.ToCharArray());
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
                    ((RfcTablePropertyAttribute)(x.GetCustomAttributes(true)
                        .First(y => y is RfcTablePropertyAttribute))).IsPartial && !x.GetGetMethod().IsVirtual).ToList();

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
                object attribute = property.GetCustomAttributes(true).FirstOrDefault(x => x is RfcTablePropertyAttribute);

                if (attribute == null)
                    return;

                var attr = ((RfcTablePropertyAttribute)attribute);

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
                    if (!string.IsNullOrEmpty(((RfcTablePropertyAttribute)attribute).Name))
                        TypeConversionHelper.ConvertFromRfcType(baseInstance, property, value, attr.TablePropertySapType);
                }
            }
        }

        #endregion

    }
}
