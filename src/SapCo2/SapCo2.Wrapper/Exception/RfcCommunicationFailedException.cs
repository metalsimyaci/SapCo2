namespace SapCo2.Wrapper.Exception
{
    /// <summary>
    /// Exception throw when a RFC communication 
    /// </summary>
    public sealed class RfcCommunicationFailedException:RfcException
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="RfcCommunicationFailedException"/> class
        /// </summary>
        /// <param name="message">Exception Message</param>
        public RfcCommunicationFailedException(string message) : base(message)
        {
        }
    }
}
