using System;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Extension
{
    public static class RfcErrorInfoExtensions
    {
        /// <summary>
        /// Exception throw according to resultCode in the error information
        /// </summary>
        /// <param name="errorInfo">Error information</param>
        /// <param name="beforeThrow">actions to be carried out before throwing error </param>
        public static void ThrowOnError(this RfcErrorInfo errorInfo, Action beforeThrow = null) =>
            errorInfo.Code.ThrowOnError(errorInfo, beforeThrow);
    }
}
