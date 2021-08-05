using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using SapCo2.Abstraction.Enumerations;

namespace SapCo2.Query
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "TooManyArguments")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class QueryOperator : IDisposable
    {
        #region Variables

        private bool _disposed;
        private static bool _isEmpty;
        private static bool _listFilled;
        private static readonly CultureInfo UsCultureInfo = new CultureInfo("en-US");

        #endregion

        #region Properties

        private static string Query { get; set; } = string.Empty;
        private static List<string> QueryList { get; set; } = new List<string>();

        public bool ListFilled
        {
            get => _listFilled;
            set => _listFilled = value;
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set => _isEmpty = value;
        }

        #endregion

        #region Methods

        public string GetQuery()
        {
            return Query;
        }

        public List<string> GetQueryList()
        {
            return QueryList;
        }


        #region Operator Methods

        public static QueryOperator Equal(string field, object value, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            Query = $"{field} EQ '{TypeConverter.ConvertToRfcType(value, tablePropertySapType)}'";
            return new QueryOperator();
        }

        public static QueryOperator NotEqual(string field, object value, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            Query = $"{field} NE '{TypeConverter.ConvertToRfcType(value, tablePropertySapType)}'";
            return new QueryOperator();
        }

        public static QueryOperator GreaterThan(string field, object value, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            Query = $"{field} GT '{TypeConverter.ConvertToRfcType(value, tablePropertySapType)}'";
            return new QueryOperator();
        }

        public static QueryOperator GreaterThanOrEqual(string field, object value, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            Query = $"{field} GE '{TypeConverter.ConvertToRfcType(value, tablePropertySapType)}'";
            return new QueryOperator();
        }

        public static QueryOperator LessThan(string field, object value, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            Query = $"{field} LT '{TypeConverter.ConvertToRfcType(value, tablePropertySapType)}'";
            return new QueryOperator();
        }

        public static QueryOperator LessThanOrEqual(string field, object value, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            Query = $"{field} LE '{TypeConverter.ConvertToRfcType(value, tablePropertySapType)}'";
            return new QueryOperator();
        }

        public static QueryOperator StartsWith(string field, object value)
        {
            Query = $"{field} LIKE '{value}%'";
            return new QueryOperator();
        }

        public static QueryOperator EndsWith(string field, object value)
        {
            Query = $"{field} LIKE '%{value}'";
            return new QueryOperator();
        }

        public static QueryOperator Contains(string field, object value)
        {
            Query = $"{field} LIKE '%{value}%'";
            return new QueryOperator();
        }

        public static QueryOperator NotContains(string field, object value)
        {
            Query = $"{field} NOT LIKE '%{value}%'";
            return new QueryOperator();
        }

        public static QueryOperator Between(string field, object firstValue, object lastValue, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            Query =
                $"{field} BETWEEN '{TypeConverter.ConvertToRfcType(firstValue, tablePropertySapType)}' AND '{TypeConverter.ConvertToRfcType(lastValue, tablePropertySapType)}'";
            return new QueryOperator();
        }

        public static QueryOperator In(string field, List<object> valueList, RfcDataTypes tablePropertySapType = RfcDataTypes.STRING)
        {
            if (!valueList.Any())
            {
                _isEmpty = true;
                return new QueryOperator();
            }

            var queryList = new List<string>();
            for (int no = 0; no <= valueList.Count - 1; no++)
            {
                var query = no == 0
                    ? $"{field.ToUpper(UsCultureInfo)} IN ('{TypeConverter.ConvertToRfcType(valueList[no], tablePropertySapType)}'"
                    : $",'{TypeConverter.ConvertToRfcType(valueList[no], tablePropertySapType)}'";
                if (valueList.Count == 1)
                    query += ")";
                else if (no == valueList.Count - 1)
                    query += ") ";

                queryList.Add(query);
            }

            QueryList = queryList;
            _listFilled = true;
            _isEmpty = false;

            return new QueryOperator();
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Query = string.Empty;
                QueryList.Clear();
            }

            _disposed = true;
        }

        #endregion

        #endregion
    }
}
