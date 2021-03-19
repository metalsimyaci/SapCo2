using System;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Core.Abstract
{
    public interface IRfcTransaction
    {
        public IntPtr CreateTransaction();
        RfcResultCodes InvokeTransaction();
        RfcResultCodes SubmitTransaction();
        RfcResultCodes ConfirmTransaction();
    }
}
