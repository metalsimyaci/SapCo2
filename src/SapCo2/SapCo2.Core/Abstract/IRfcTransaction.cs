using System;

namespace SapCo2.Core.Abstract
{
    public interface IRfcTransaction: IDisposable
    {
        void InvokeTransaction(IntPtr functionHandle);
        void SaveChangeTransaction();
        void CommitTransaction();
    }
}
