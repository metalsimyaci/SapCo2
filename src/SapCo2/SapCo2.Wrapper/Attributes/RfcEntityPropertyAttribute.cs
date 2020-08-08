using System;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcEntityPropertyAttribute:Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public RfcEntityPropertySapTypes EntityPropertySapType { get; set; } = RfcEntityPropertySapTypes.STRING;
        public int Length { get; set; } = 0;
        public string SubTypePropertyName { get; set; } = string.Empty;
        public bool IsPartial { get; set; }
        public bool Unsafe { get; set; }

        public RfcEntityPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
