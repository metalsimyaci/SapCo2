namespace SapCo2.Wrapper.Exception
{
    public sealed class RfcCommunicationFailedException : RfcException
    {
        public RfcCommunicationFailedException(string message) : base(message)
        {
        }
    }
}
