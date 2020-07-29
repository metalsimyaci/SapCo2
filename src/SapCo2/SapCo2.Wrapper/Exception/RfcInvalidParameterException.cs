using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Exception
{
    /// <summary>
    /// Exception throw when an invalid parameter is being passed
    /// </summary>
    public class RfcInvalidParameterException:RfcException
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="RfcInvalidParameterException"/> class
        /// </summary>
        /// <param name="message">Exception Message</param>
        public RfcInvalidParameterException(string message) : base(RfcResultCodes.RFC_INVALID_PARAMETER,message)
        {
        }
    }
}
