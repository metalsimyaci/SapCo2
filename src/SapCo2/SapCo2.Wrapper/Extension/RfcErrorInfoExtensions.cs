using System;
using System.Diagnostics.CodeAnalysis;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Extension
{
    [ExcludeFromCodeCoverage]
    public static class RfcErrorInfoExtensions
    {
        public static void ThrowOnError(this RfcErrorInfo errorInfo, Action beforeThrow = null) =>
            errorInfo.Code.ThrowOnError(errorInfo, beforeThrow);
    }
}
