using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SapCo2.Abstract;
using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Core.Abstract;
using SapCo2.Core.Models;
using SapCo2.Extensions;
using SapCo2.Models;
using SapCo2.Query;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Struct;

namespace SapCo2
{
    public class RfcClient:IRfcClient
    {
        #region Constants

        private const string READ_TABLE_FUNCTION_NAME = "RFC_READ_TABLE";

        #endregion

        #region Variables

        private bool _disposed;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPropertyCache _propertyCache;
        private readonly RfcConfiguration _rfcConfiguration;
        private string _activeServer;

        #endregion

        #region Methods

        #region Constructors

        public RfcClient(IServiceProvider serviceProvider, IOptions<RfcConfiguration> configurationOptions)
        {
            _serviceProvider = serviceProvider;
            _rfcConfiguration = configurationOptions.Value;
            _propertyCache = _serviceProvider.GetRequiredService<IPropertyCache>();

            IRfcNetWeaverLibrary rfcNetWeaverLibrary = serviceProvider.GetRequiredService<IRfcNetWeaverLibrary>();

            rfcNetWeaverLibrary.EnsureLibraryPresent();
        }

        #endregion

        #region IRfcClient Implementation

        public void UseServer(string serverAlias)
        {
            if (_rfcConfiguration.RfcServers != null && _rfcConfiguration.RfcServers.All(s => s.Alias != serverAlias))
                throw new RfcException($"SAP server connection settings not found: '{serverAlias}'.");

            _activeServer = serverAlias;
        }

        public IReadOnlyList<ParameterMetaData> ReadFunctionMetaData(string name)
        {
            using IRfcConnection rfcConnection = GetConnection();
            IRfcFunctionMetaData functionMetaData = rfcConnection.CreateFunctionMetaData(name);

            List<RfcParameterDescription> parameters = functionMetaData.GetParameterDescriptions();
            List<FieldMetaData> GetFields(IntPtr parameterTypeDescriptionHandler)
            {
                return functionMetaData.GetFieldDescriptions(parameterTypeDescriptionHandler)
                    .Select(s => new FieldMetaData
                    {
                        Name = s.Name,
                        Decimals = s.Decimals,
                        NucLength = s.NucLength,
                        NucOffset = s.NucOffset,
                        UcLength = s.UcLength,
                        UcOffset = s.UcOffset,
                        Type = s.Type.ToString()
                    })
                    .ToList();
            }

            return parameters.Select(parameter => new ParameterMetaData()
                {
                    Name = parameter.Name,
                    Description = parameter.ParameterText,
                    DefaultValue = parameter.DefaultValue,
                    Direction = parameter.Direction.ToString(),
                    Decimals = parameter.Decimals,
                    NumericLength = parameter.NucLength,
                    Optional = parameter.Optional,
                    Type = parameter.Type.ToString(),
                    UcLength = parameter.UcLength,
                    Fields = GetFields(parameter.TypeDescHandle)
            })
                .ToList().AsReadOnly();
        }

        public IRfcTransaction CreateTransaction()
        {
            using IRfcConnection rfcConnection = GetConnection();
            return rfcConnection.CreateTransaction();
        }

        public void ExecuteRfc(string name)
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            rfcFunction.Invoke();
        }

        public async Task<bool> ExecuteRfcAsync(string name)
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            return await rfcFunction.InvokeAsync();
        }

        public void ExecuteRfc<TInput>(string name, TInput input) where TInput : class, IRfcInput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            rfcFunction.Invoke(input);
        }

        public async Task<bool> ExecuteRfcAsync<TInput>(string name, TInput input) where TInput : class, IRfcInput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            return await rfcFunction.InvokeAsync(input);
        }

        public TOutput ExecuteRfc<TOutput>(string name) where TOutput : class, IRfcOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            return rfcFunction.Invoke<TOutput>();
        }

        public async Task<TOutput> ExecuteRfcAsync<TOutput>(string name) where TOutput : class, IRfcOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            return await rfcFunction.InvokeAsync<TOutput>();
        }

        public TOutput ExecuteRfc<TInput, TOutput>(string name, TInput input) where TInput : class, IRfcInput where TOutput : class, IRfcOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            return rfcFunction.Invoke<TOutput>(input);
        }

        public async Task<TOutput> ExecuteRfcAsync<TInput, TOutput>(string name, TInput input) where TInput : class, IRfcInput where TOutput : class, IRfcOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);
            return await rfcFunction.InvokeAsync<TOutput>(input);
        }

        public TOutput ExecuteBapi<TOutput>(string name) where TOutput : class, IBapiOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);

            TOutput result = rfcFunction.Invoke<TOutput>();

            result.BapiReturn.ThrowOnError();
            return result;
        }

        public async Task<TOutput> ExecuteBapiAsync<TOutput>(string name) where TOutput : class, IBapiOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);

            TOutput result = await rfcFunction.InvokeAsync<TOutput>();

            result.BapiReturn.ThrowOnError();
            return result;
        }

        public TOutput ExecuteBapi<TInput, TOutput>(string name, TInput input) where TInput : class, IBapiInput where TOutput : class, IBapiOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);

            TOutput result = rfcFunction.Invoke<TOutput>(input);

            result.BapiReturn.ThrowOnError();
            return result;
        }

        public async Task<TOutput> ExecuteBapiAsync<TInput, TOutput>(string name, TInput input) where TInput : class, IBapiInput where TOutput : class, IBapiOutput
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(name);

            TOutput result = await rfcFunction.InvokeAsync<TOutput>(input);

            result.BapiReturn.ThrowOnError();
            return result;
        }

        public List<TOutput> GetTableData<TOutput>(List<string> options, bool includeUnsafeFields = false, int rowCount = 0, int skip = 0, string delimiter = "|", string noData = "")
            where TOutput : class, ISapTable
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(READ_TABLE_FUNCTION_NAME);

            List<string> fieldNames = GetRfcEntityFieldNames(typeof(TOutput), includeUnsafeFields);

            var inputParameters = new TableInputParameter
            {
                Query = GetRfcTableName<TOutput>(),
                Delimiter = delimiter,
                NoData = noData,
                RowCount = rowCount,
                RowSkips = skip,
                Fields = fieldNames?.Select(f => new TableField { FieldName = f }).ToArray(),
                Options = options?.Select(o => new TableOption { Text = o }).ToArray()
            };

            TableOutputParameter result = rfcFunction.Invoke<TableOutputParameter>(inputParameters);

            return ConvertToList<TOutput>(result, delimiter, fieldNames);
        }

        public async Task<List<TOutput>> GetTableDataAsync<TOutput>(List<string> options = null, bool includeUnsafeFields = false, int rowCount = 0, int skip = 0, string delimiter = "|",
            string noData = "")
            where TOutput : class, ISapTable
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(READ_TABLE_FUNCTION_NAME);

            List<string> fieldNames = GetRfcEntityFieldNames(typeof(TOutput), includeUnsafeFields);

            var inputParameters = new TableInputParameter
            {
                Query = GetRfcTableName<TOutput>(),
                Delimiter = delimiter,
                NoData = noData,
                RowCount = rowCount,
                RowSkips = skip,
                Fields = fieldNames?.Select(f => new TableField { FieldName = f }).ToArray(),
                Options = options?.Select(o => new TableOption { Text = o }).ToArray()
            };

            TableOutputParameter result = await rfcFunction.InvokeAsync<TableOutputParameter>(inputParameters);

            return ConvertToList<TOutput>(result, delimiter, fieldNames);
        }

        public TOutput GetStruct<TOutput>(List<string> options, bool includeUnsafeFields = false, string delimiter = "|", string noData = "")
            where TOutput : class, ISapTable
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(READ_TABLE_FUNCTION_NAME);

            List<string> fieldNames = GetRfcEntityFieldNames(typeof(TOutput), includeUnsafeFields);

            var inputParameters = new TableInputParameter
            {
                Query = GetRfcTableName<TOutput>(),
                Delimiter = delimiter,
                NoData = noData,
                RowCount = 1,
                RowSkips = 0,
                Fields = fieldNames?.Select(f => new TableField { FieldName = f }).ToArray(),
                Options = options?.Select(o => new TableOption { Text = o }).ToArray()
            };

            TableOutputParameter result = rfcFunction.Invoke<TableOutputParameter>(inputParameters);

            return ConvertToStruct<TOutput>(result, delimiter, fieldNames);
        }

        public async Task<TOutput> GetStructAsync<TOutput>(List<string> options, bool includeUnsafeFields = false, string delimiter = "|", string noData = "")
            where TOutput : class, ISapTable
        {
            using IRfcConnection rfcConnection = GetConnection();
            using IRfcFunction rfcFunction = rfcConnection.CreateFunction(READ_TABLE_FUNCTION_NAME);

            List<string> fieldNames = GetRfcEntityFieldNames(typeof(TOutput), includeUnsafeFields);

            var inputParameters = new TableInputParameter
            {
                Query = GetRfcTableName<TOutput>(),
                Delimiter = delimiter,
                NoData = noData,
                RowCount = 1,
                RowSkips = 0,
                Fields = fieldNames?.Select(f => new TableField { FieldName = f }).ToArray(),
                Options = options?.Select(o => new TableOption { Text = o }).ToArray()
            };

            TableOutputParameter result = await rfcFunction.InvokeAsync<TableOutputParameter>(inputParameters);

            return ConvertToStruct<TOutput>(result, delimiter, fieldNames);
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }

            _disposed = true;
        }

        #endregion

        #endregion

        #region Private Functions

        private IRfcConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(_activeServer))
            {
                if (string.IsNullOrWhiteSpace(_rfcConfiguration.DefaultServer))
                {
                    if (_rfcConfiguration.RfcServers.Count == 1)
                        _activeServer = _rfcConfiguration.RfcServers.Single().Alias;
                    else
                        throw new RfcException("The default SAP server could not be detected.");
                }
                else
                {
                    if (_rfcConfiguration.RfcServers.Exists(s => s.Alias == _rfcConfiguration.DefaultServer))
                        _activeServer = _rfcConfiguration.DefaultServer;
                    else
                        throw new RfcException("Default SAP server connection settings were not found.");
                }
            }

            RfcServer server = _rfcConfiguration.RfcServers.Single(s => s.Alias == _activeServer);

            if (server.ConnectionPooling.Enabled)
            {
                IRfcConnectionPoolServiceFactory factory = _serviceProvider.GetRequiredService<IRfcConnectionPoolServiceFactory>();
                IRfcConnectionPool connectionPool = factory.GetService(_activeServer);

                return connectionPool.GetConnection();
            }

            IRfcConnection rfcConnection = _serviceProvider.GetRequiredService<IRfcConnection>();

            rfcConnection.Connect(_activeServer);
            return rfcConnection;
        }

        private string GetRfcTableName<TEntity>()
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(TEntity));
            RfcEntityAttribute attribute = (RfcEntityAttribute)attributes.FirstOrDefault(p => p is RfcEntityAttribute);

            return attribute?.Name ?? nameof(TEntity).ToUpperInvariant();
        }

        private List<string> GetRfcEntityFieldNames(Type type, bool includeUnsafeFields)
        {
            var fieldList = new List<string>();

            foreach (PropertyInfo p in type.GetProperties().Where(x => !x.GetGetMethod().IsVirtual))
            {
                object attributes = p.GetCustomAttributes(true).FirstOrDefault(x => x is RfcEntityPropertyAttribute);

                if (attributes == null)
                    continue;

                if (((RfcEntityPropertyAttribute)attributes).IsPartial)
                    fieldList.AddRange(GetRfcEntityFieldNames(p.PropertyType, includeUnsafeFields));
                else
                {
                    if (includeUnsafeFields)
                        fieldList.Add(((RfcEntityPropertyAttribute)attributes).Name);
                    else
                    {
                        if (!((RfcEntityPropertyAttribute)attributes).Unsafe)
                            fieldList.Add(((RfcEntityPropertyAttribute)attributes).Name);
                    }
                }
            }

            if (!fieldList.Any())
                throw new MissingFieldException("No property marked with RfcEntityPropertyAttribute found");

            return fieldList.OrderBy(x => x).ToList();
        }

        private List<T> ConvertToList<T>(TableOutputParameter outputParameter, string delimiter, List<string> fieldNames)
        {
            return outputParameter.Data?.Select(x => ConvertTo<T>(x, delimiter, fieldNames)).ToList() ?? Activator.CreateInstance<List<T>>();
        }

        private T ConvertToStruct<T>(TableOutputParameter outputParameter, string delimiter, List<string> fieldList)
        {
            if (outputParameter?.Data == null)
                return default;

            if (outputParameter.Data.Length < 1)
                return Activator.CreateInstance<T>();

            return ConvertTo<T>(outputParameter.Data.First(), delimiter, fieldList);
        }

        private T ConvertTo<T>(TableData line, string delimiter, List<string> fieldNames)
        {
            T instance = Activator.CreateInstance<T>();

            if (line == null)
                return instance;

            return ConvertTo(line.Wa, delimiter, fieldNames, instance);
        }

        private T ConvertTo<T>(string line, string delimiter, List<string> fieldList, T instance)
        {
            if (string.IsNullOrEmpty(line))
                return instance;

            string[] values = line.Split(delimiter.ToCharArray());

            for (int i = 0; i < fieldList.Count; i++)
                SetValue(fieldList[i], values[i], instance);

            return instance;
        }

        private void SetValue(string fieldName, object value, object baseInstance, bool lookForPartial = true)
        {
            PropertyInfo property = _propertyCache.GetPropertyInfo(baseInstance.GetType(), fieldName);

            if (property == null)
            {
                if (!lookForPartial)
                    return;

                var propertyList = baseInstance.GetType().GetProperties().Where(x =>
                    ((RfcEntityPropertyAttribute)(x.GetCustomAttributes(true)
                        .First(y => y is RfcEntityPropertyAttribute))).IsPartial && !x.GetGetMethod().IsVirtual).ToList();

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
                object attribute = property.GetCustomAttributes(true).FirstOrDefault(x => x is RfcEntityPropertyAttribute);

                if (attribute == null)
                    return;

                var attr = ((RfcEntityPropertyAttribute)attribute);

                if (attr.Name != fieldName)
                    return;

                string subTypeFieldName = attr.SubTypePropertyName;

                if (!string.IsNullOrEmpty(subTypeFieldName))
                {
                    Type subType = property.PropertyType;
                    object subInstance = Activator.CreateInstance(subType);
                    PropertyInfo subProperty = subInstance?.GetType().GetProperty(subTypeFieldName);

                    subProperty?.SetValue(subInstance, value, null);
                    property.SetValue(baseInstance, subInstance);
                }
                else
                {
                    if (!string.IsNullOrEmpty(((RfcEntityPropertyAttribute)attribute).Name))
                        TypeConverter.ConvertFromRfcType(baseInstance, property, value, attr.SapDataType);
                }
            }
        }

        #endregion

        #endregion

    }
}
