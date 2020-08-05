using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Exception
{
    public class RfcInvalidParameterException:RfcException
    {
        public RfcInvalidParameterException(string message) : base(RfcResultCodes.RFC_INVALID_PARAMETER,message)
        {
        }
    }
}
