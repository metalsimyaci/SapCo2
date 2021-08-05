using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;

namespace SapCo2.Samples.Core.Models
{
    public sealed class BomOutputParameter : IRfcOutput
    {
        [RfcEntityProperty("STB")]
        public Stb[] StbData { get; set; }

        [RfcEntityProperty("TOPMAT")]
        public Topmat Topmat { get; set; }
    }
}
