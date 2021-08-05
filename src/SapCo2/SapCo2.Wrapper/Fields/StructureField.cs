using System;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Mappers;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class StructureField<TStructure> : Field<TStructure>
    {
        public StructureField(string name, TStructure value) : base(name, value)
        {
        }

        public override void Apply(IRfcInterop interop, IntPtr dataHandle)
        {
            RfcResultCodes resultCode = interop.GetStructure(
                dataHandle: dataHandle,
                name: Name,
                structHandle: out IntPtr structHandle,
                out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            InputMapper.Apply(interop, structHandle, Value);
        }

        public static StructureField<T> Extract<T>(IRfcInterop interop, IntPtr dataHandle, string name)
        {
            RfcResultCodes resultCode = interop.GetStructure(
                dataHandle: dataHandle,
                name: name,
                structHandle: out IntPtr structHandle,
                out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            T structValue = OutputMapper.Extract<T>(interop, structHandle);

            return new StructureField<T>(name, structValue);
        }

        public override string ToString()
            => $"{Name} = {typeof(TStructure)}";
    }
}
