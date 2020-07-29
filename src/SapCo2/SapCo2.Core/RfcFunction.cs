using System;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Mappers;

namespace SapCo2.Core
{
    public sealed class RfcFunction:IRfcFunction
    {
        private readonly IRfcInterop _interop;
        private readonly IntPtr _rfcConnectionHandle;
        private readonly IntPtr _functionHandle;

        private RfcFunction(IRfcInterop interop, IntPtr rfcConnectionHandle, IntPtr functionHandle)
        {
            _interop = interop;
            _rfcConnectionHandle = rfcConnectionHandle;
            _functionHandle = functionHandle;
        }

        internal static IRfcFunction CreateFromDescriptionHandle(IRfcInterop interop, IntPtr rfcConnectionHandle, IntPtr functionDescriptionHandle)
        {
            IntPtr functionHandle = interop.CreateFunction(
                funcDescHandle: functionDescriptionHandle,
                errorInfo: out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError();

            return new RfcFunction(
                interop: interop,
                rfcConnectionHandle: rfcConnectionHandle,
                functionHandle: functionHandle);
        }

        public void Dispose()
        {
           
        }

        public void Invoke()
        {
            RfcResultCodes resultCode = _interop.DestroyFunction(
                funcHandle: _functionHandle,
                out RfcErrorInfo errorInfo);

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
            InputMapper.Apply(_interop, _functionHandle, input);
            Invoke();

            return OutputMapper.Extract<TOutput>(_interop, _functionHandle);
        }
    }
}
