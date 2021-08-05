using System;

namespace SapCo2.Wrapper.Abstract
{
    public interface IInputMapper
    {
        void Apply(IRfcInterop interop, IntPtr dataHandle, object input);
    }
}
