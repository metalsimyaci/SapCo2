using System;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Fields.Abstract;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class DoubleField : Field<double>
    {
        public DoubleField(string name, double value)
            : base(name, value)
        {
        }

        public override void Apply(RfcInterop interop, IntPtr dataHandle)
        {
            RfcResultCodes resultCode = interop.SetFloat(
                dataHandle: dataHandle,
                name: Name,
                value: Value,
                errorInfo: out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public static DoubleField Extract(RfcInterop interop, IntPtr dataHandle, string name)
        {
            RfcResultCodes resultCode = interop.GetFloat(
                dataHandle: dataHandle,
                name: name,
                out double value,
                out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            return new DoubleField(name, value);
        }

        public override string ToString()
            => $"{Name} = {Value}";
    }
}
