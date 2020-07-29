using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Exception
{
    public class RfcException : System.Exception
    {
        internal RfcResultCodes ResultCode { get; set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="RfcException"/> class
        /// </summary>
        /// <param name="message">Exception Message</param>
        public RfcException(string message) : base(string.IsNullOrEmpty(message) ? "SAP RFC Error" : $"SAP Error With message:{message}")
        {
            ResultCode = RfcResultCodes.RFC_UNKNOWN_ERROR;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RfcException"/> class
        /// </summary>
        /// <param name="resultCode">ABAP Result Code</param>
        /// <param name="message">Exception Message</param>
        internal RfcException(RfcResultCodes resultCode, string message) : base(string.IsNullOrEmpty(message)
            ? $"SAP RFC Error: {resultCode}"
            : $"SAP RFC Error: {resultCode} with message: {message}")
        {
            ResultCode = resultCode;
        }
    }
}
