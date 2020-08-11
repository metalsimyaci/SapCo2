using SapCo2.Parameters;

namespace SapCo2.Abstract
{
    public abstract class RfcBapiOutputBase:IRfcBapiOutput
    {
        public RfcBapiOutputParameter BapiReturn { get; set; }
    }
}
