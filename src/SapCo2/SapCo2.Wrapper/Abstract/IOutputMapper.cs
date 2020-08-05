using System;

namespace SapCo2.Wrapper.Abstract
{
    public interface IOutputMapper
    {
        TOutput Extract<TOutput>(IRfcInterop interop, IntPtr dataHandle);
    }
}
