using System;
using System.Diagnostics.CodeAnalysis;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Extension
{
    [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
    public static class RfcResultCodeExtensions
    {
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
