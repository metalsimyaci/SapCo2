using System;

namespace SapCo2.Core.Abstract
{
    public interface IRfcConnectionPool:IDisposable
    {
        public string ServerAlias { get; set; }

        IRfcConnection GetConnection();
        void ReturnConnection(IRfcConnection connection);
        void ForgetConnection(IRfcConnection connection);
    }
}
