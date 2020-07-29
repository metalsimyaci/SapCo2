using System;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Fields.Abstract;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class LongField : Field<long>
    {
        public LongField(string name, long value)
            : base(name, value)
        {
        }

        public override void Apply(RfcInterop interop, IntPtr dataHandle)
        {
            RfcResultCodes resultCode = interop.SetInt8(
                dataHandle: dataHandle,
                name: Name,
                value: Value,
                errorInfo: out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public static LongField Extract(RfcInterop interop, IntPtr dataHandle, string name)
        {
            RfcResultCodes resultCode = interop.GetInt8(
                dataHandle: dataHandle,
                name: name,
                out long value,
                out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            return new LongField(name, value);
        }

        public override string ToString()
            => $"{Name} = {Value}L";
    }
}
