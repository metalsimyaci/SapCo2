using System;
using System.Globalization;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class DecimalField : Field<decimal>
    {
        public DecimalField(string name, decimal value)
            : base(name, value)
        {
        }

        public override void Apply(IRfcInterop interop, IntPtr dataHandle)
        {
            var stringValue = Value.ToString(CultureInfo.InvariantCulture);

            RfcResultCodes resultCode = interop.SetString(
                dataHandle: dataHandle,
                name: Name,
                value: stringValue,
                valueLength: (uint)stringValue.Length,
                errorInfo: out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public static DecimalField Extract(IRfcInterop interop, IntPtr dataHandle, string name)
        {
            RfcResultCodes resultCode = interop.GetString(
                dataHandle: dataHandle,
                name: name,
                stringBuffer: Array.Empty<char>(),
                bufferLength: 0,
                stringLength: out uint stringLength,
                errorInfo: out RfcErrorInfo errorInfo);

            if (resultCode != RfcResultCodes.RFC_BUFFER_TOO_SMALL)
            {
                resultCode.ThrowOnError(errorInfo);
                return new DecimalField(name, 0);
            }

            var buffer = new char[stringLength + 1];
            resultCode = interop.GetString(
                dataHandle: dataHandle,
                name: name,
                stringBuffer: buffer,
                bufferLength: (uint)buffer.Length,
                stringLength: out _,
                errorInfo: out errorInfo);

            resultCode.ThrowOnError(errorInfo);

            var decimalValue = decimal.Parse(new string(buffer, 0, (int)stringLength), CultureInfo.InvariantCulture);

            return new DecimalField(name, decimalValue);
        }

        public override string ToString()
            => $"{Name} = {Value.ToString(CultureInfo.InvariantCulture)}M";
    }
}
