using System;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Extension
{
    public static class RfcErrorInfoExtensions
    {
        public static void ThrowOnError(this RfcErrorInfo errorInfo, Action beforeThrow = null) =>
            errorInfo.Code.ThrowOnError(errorInfo, beforeThrow);
    }
}
