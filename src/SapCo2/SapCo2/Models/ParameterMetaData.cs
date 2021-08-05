using System.Collections.Generic;

namespace SapCo2.Models
{
    public class ParameterMetaData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Direction { get; set; }
        public int NumericLength { get; set; }
        public int UcLength { get; set; }
        public int Decimals { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public bool Optional { get; set; }

        public List<FieldMetaData> Fields { get; set; }

    }

    public class FieldMetaData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int NucLength { get; set; }
        public int NucOffset { get; set; }
        public int UcLength { get; set; }
        public int UcOffset { get; set; }
        public int Decimals { get; set; }
    }
}
