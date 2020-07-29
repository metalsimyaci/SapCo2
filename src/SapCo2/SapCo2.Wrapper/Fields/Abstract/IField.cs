using System;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Fields.Abstract
{
    internal interface IField
    {
        void Apply(RfcInterop interop, IntPtr dataHandle);
    }
}
