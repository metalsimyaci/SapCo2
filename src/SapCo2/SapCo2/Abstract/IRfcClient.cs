using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SapCo2.Abstraction;
using SapCo2.Core.Abstract;
using SapCo2.Models;

namespace SapCo2.Abstract
{
    public interface IRfcClient:IDisposable
    {
        void UseServer(string sapServerAlias);
        void ExecuteRfc(string name);
        Task<bool> ExecuteRfcAsync(string name);
        void ExecuteRfc<TInput>(string name, TInput inputParameter) where TInput : class, IRfcInput;
        Task<bool> ExecuteRfcAsync<TInput>(string name, TInput inputParameter) where TInput : class, IRfcInput;
        TOutput ExecuteRfc<TOutput>(string name) where TOutput : class, IRfcOutput;
        Task<TOutput> ExecuteRfcAsync<TOutput>(string name) where TOutput : class, IRfcOutput;
        TOutput ExecuteRfc<TInput, TOutput>(string name, TInput input) where TInput : class, IRfcInput where TOutput : class, IRfcOutput;
        Task<TOutput> ExecuteRfcAsync<TInput, TOutput>(string name, TInput input) where TInput : class, IRfcInput where TOutput : class, IRfcOutput;
        TOutput ExecuteBapi<TOutput>(string name) where TOutput : class, IBapiOutput;
        Task<TOutput> ExecuteBapiAsync<TOutput>(string name) where TOutput : class, IBapiOutput;
        TOutput ExecuteBapi<TInput, TOutput>(string name, TInput input) where TInput : class, IBapiInput where TOutput : class, IBapiOutput;
        Task<TOutput> ExecuteBapiAsync<TInput, TOutput>(string name, TInput input) where TInput : class, IBapiInput where TOutput : class, IBapiOutput;
        List<TOutput> GetTableData<TOutput>(List<string> options, bool includeUnsafeFields = false, int rowCount = 0, int skip = 0, string delimiter = "|", string noData = "")
            where TOutput : class, ISapTable;
        Task<List<TOutput>> GetTableDataAsync<TOutput>(List<string> options = null, bool includeUnsafeFields = false, int rowCount = 0, int skip = 0, string delimiter = "|",
            string noData = "") where TOutput : class, ISapTable;
        TOutput GetStruct<TOutput>(List<string> options, bool includeUnsafeFields = false, string delimiter = "|", string noData = "") where TOutput : class, ISapTable;
        Task<TOutput> GetStructAsync<TOutput>(List<string> options, bool includeUnsafeFields = false, string delimiter = "|", string noData = "") where TOutput : class, ISapTable;
        IReadOnlyList<ParameterMetaData> ReadFunctionMetaData(string name);
        IRfcTransaction CreateTransaction();
    }
}
