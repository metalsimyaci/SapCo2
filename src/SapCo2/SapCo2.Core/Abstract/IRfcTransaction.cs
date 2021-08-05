using System;

namespace SapCo2.Core.Abstract
{
    public interface IRfcTransaction: IDisposable
    {
        IRfcTransactionFunction CreateFunction(string name);
        void SubmitTransaction();
        void ConfirmTransaction();
    }
}
