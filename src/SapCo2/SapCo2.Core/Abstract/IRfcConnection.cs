using System;

namespace SapCo2.Core.Abstract
{
    public interface IRfcConnection:IDisposable
    {
        void Connect();
        void Disconnect();
        bool IsValid { get; }
        bool Ping();
        IntPtr GetConnectionHandle();
    }
}
