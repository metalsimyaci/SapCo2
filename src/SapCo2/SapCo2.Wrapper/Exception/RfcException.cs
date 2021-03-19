using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Exception
{
    public class RfcException : System.Exception
    {
        internal RfcResultCodes ResultCode { get; }

        public RfcException(string message) 
            : base(string.IsNullOrEmpty(message) ? "SAP RFC Error" : $"SAP RFC Error with message: {message}")
        {
            ResultCode = RfcResultCodes.RFC_UNKNOWN_ERROR;
        }

        internal RfcException(RfcResultCodes resultCode, string message) : 
            base(string.IsNullOrEmpty(message) ? $"SAP RFC Error: {resultCode}" : $"SAP RFC Error: {resultCode} with message: {message}")
        {
            ResultCode = resultCode;
        }
    }
}
