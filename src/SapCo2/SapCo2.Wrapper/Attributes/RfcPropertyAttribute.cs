using System;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcPropertyAttribute:Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public RfcDataTypes DataType { get; set; } = RfcDataTypes.String;
        public int Length { get; set; } = 0;
        public string SubTypePropertyName { get; set; } = string.Empty;
        public bool IsPartial { get; set; }
        public bool Unsafe { get; set; }

        public RfcPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
