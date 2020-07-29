using System;
using SapCo2.Wrapper.Abstract;

namespace SapCo2.Wrapper.Fields.Abstract
{
    internal interface IField
    {
        void Apply(IRfcInterop interop, IInputMapper inputMapper, IntPtr dataHandle);
    }
}
