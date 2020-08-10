using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Exception
{
    public sealed class RfcCommunicationFailedException : RfcException
    {
        public RfcCommunicationFailedException(string message) : base(RfcResultCodes.RFC_COMMUNICATION_FAILURE, message)
        {
        }
    }
}
