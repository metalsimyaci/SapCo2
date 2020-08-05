using System;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Mappers;

namespace SapCo2.Core
{
    public class RfcFunction : IRfcFunction
    {
        private readonly IRfcInterop _interop;
        private IntPtr _rfcConnectionHandle;
        private IntPtr _functionDescriptionHandle;
        private IntPtr _functionHandle;

        public RfcFunction(IRfcInterop interop)
        {
            _interop = interop;
        }

        public IRfcFunction CreateFunction(IRfcConnection connection, string name)
        {
            _rfcConnectionHandle = connection.GetConnectionHandle();
            _functionDescriptionHandle = GetFunctionDescriptionHandle(name);
            _functionHandle = CreateFromDescriptionHandle(_functionDescriptionHandle);
            return this;
        }
        private IntPtr GetFunctionDescriptionHandle(string name)
        {
            IntPtr functionDescriptionHandle = _interop.GetFunctionDesc(rfcHandle: _rfcConnectionHandle,funcName: name,errorInfo: out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError();
            return functionDescriptionHandle;
        }
        internal IntPtr CreateFromDescriptionHandle(IntPtr functionDescriptionHandle)
        {
            IntPtr functionHandle = _interop.CreateFunction(funcDescHandle: functionDescriptionHandle, errorInfo: out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError();

            return functionHandle;
        }

        public void Dispose()
        {
            RfcResultCodes resultCode = _interop.DestroyFunction(_functionHandle, out RfcErrorInfo errorInfo);
            resultCode.ThrowOnError(errorInfo);
        }

        public void Invoke()
        {
            RfcResultCodes resultCode = _interop.Invoke(_rfcConnectionHandle,funcHandle: _functionHandle, out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public void Invoke(object input)
        {
            InputMapper.Apply(_interop, _functionHandle, input);

            Invoke();
        }

        public TOutput Invoke<TOutput>()
        {
            Invoke();

            return OutputMapper.Extract<TOutput>(_interop, _functionHandle);
        }

        public TOutput Invoke<TOutput>(object input)
        {
            Invoke(input);

            return OutputMapper.Extract<TOutput>(_interop, _functionHandle);
        }

    }
}
