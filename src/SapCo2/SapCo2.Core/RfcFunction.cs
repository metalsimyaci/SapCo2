using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;

namespace SapCo2.Core
{
    public sealed class RfcFunction : RfcFunctionBase
    {
        public RfcFunction(IRfcInterop interop):base(interop)
        {
        }
    }
}
