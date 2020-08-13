using SapCo2.Parameters;
using SapCo2.Wrapper.Attributes;

namespace SapCo2.Abstract
{
    public abstract class RfcBapiOutputBase:IRfcBapiOutput
    {
        [RfcEntityProperty("RETURN")]
        public RfcBapiOutputParameter BapiReturn { get; set; }
    }
}
