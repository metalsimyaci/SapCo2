using System;
using System.Collections.Generic;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core.Abstract
{
    public interface IRfcConnection:IDisposable
    {
        bool IsValid { get; }
        
        bool Ping();
        void Connect();
        void Connect(string serverAlias);
        void Disconnect();
        IntPtr GetConnectionHandle();
        IRfcFunction CreateFunction(string name);
        IRfcTransaction CreateTransaction();
        IRfcFunctionMetaData CreateFunctionMetaData(string name);
        void SetPool(IRfcConnectionPool rfcConnectionPool);
    }
}
