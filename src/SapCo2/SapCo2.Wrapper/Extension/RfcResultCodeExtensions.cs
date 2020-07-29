using System;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Extension
{
    public static class RfcResultCodeExtensions
    {
        /// <summary>
        ///  Exception throw according to resultCode
        /// </summary>
        /// <param name="resultCode">ABAB Result Code</param>
        /// <param name="errorInfo">Error information</param>
        /// <param name="beforeThrow">actions to be carried out before throwing error</param>
        public static void ThrowOnError(this RfcResultCodes resultCode, RfcErrorInfo errorInfo, Action beforeThrow = null)
        {
            if(resultCode == RfcResultCodes.RFC_OK)
                return;

            beforeThrow?.Invoke();

            if (resultCode == RfcResultCodes.RFC_COMMUNICATION_FAILURE)
                throw new RfcCommunicationFailedException(errorInfo.Message);

            if (resultCode == RfcResultCodes.RFC_INVALID_PARAMETER)
                throw new RfcInvalidParameterException(errorInfo.Message);

            throw new RfcException(resultCode, errorInfo.Message);
        }
    }
}
