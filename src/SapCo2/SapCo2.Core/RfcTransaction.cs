using System;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core
{
    internal sealed class RfcTransaction : IRfcTransaction
    {
        #region Variables

        private readonly IRfcInterop _interop;
        private readonly IntPtr _transactionHandle;

        #endregion

        #region Methods


        #region Constructors

        public RfcTransaction(IRfcInterop interop, IntPtr transactionHandle)
        {
            _interop = interop;
            _transactionHandle = transactionHandle;
        }

        #endregion

        #region IRfcTransaction Implementation

        public void InvokeTransaction(IntPtr functionHandle)
        {
            var resultCode=_interop.InvokeInTransaction(_transactionHandle, functionHandle, out RfcErrorInfo errorInfo);
            resultCode.ThrowOnError(errorInfo);
        }

        public void SaveChangeTransaction()
        {
            RfcResultCodes resultCode = _interop.SubmitTransaction(_transactionHandle, out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public void CommitTransaction()
        {
            RfcResultCodes resultCode = _interop.ConfirmTransaction(_transactionHandle, out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }


        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            if (_transactionHandle == IntPtr.Zero) return;

            RfcResultCodes resultCode = _interop.DestroyTransaction(_transactionHandle, out RfcErrorInfo errorInfo);
            resultCode.ThrowOnError(errorInfo);
        }

        #endregion

        #endregion


    }
}
