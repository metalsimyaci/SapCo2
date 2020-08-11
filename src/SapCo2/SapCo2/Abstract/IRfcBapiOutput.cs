using SapCo2.Parameters;
using SapCo2.Wrapper.Attributes;

namespace SapCo2.Abstract
{
    public interface IRfcBapiOutput
    {
        [RfcEntityProperty("RETURN")]
        public RfcBapiOutputParameter BapiReturn { get; set; }
    }
}
