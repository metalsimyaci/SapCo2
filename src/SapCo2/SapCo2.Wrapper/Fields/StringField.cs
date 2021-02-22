using System;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Fields.Abstract;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class StringField : Field<string>
    {
        public StringField(string name, string value)
            : base(name, value)
        {
        }

        public override void Apply(IRfcInterop interop, IntPtr dataHandle)
        {
            if (Value == null)
                return;

            RfcResultCodes resultCode = interop.SetString(
                dataHandle: dataHandle,
                name: Name,
                value: Value,
                valueLength: (uint)Value.Length,
                errorInfo: out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public static StringField Extract(IRfcInterop interop, IntPtr dataHandle, string name)
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
                return new StringField(name, string.Empty);
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

            return new StringField(name, new string(buffer, 0, (int)stringLength));
        }

        public override string ToString()
            => $"{Name} = \"{Value}\"";
    }
}
