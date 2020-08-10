using System;
using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcTablePropertyAttribute : RfcEntityPropertyAttribute
    {
        public RfcEntityPropertySapTypes EntityPropertySapType { get; set; }
        public int Length { get; set; } = 0;
        public string SubTypePropertyName { get; set; }
        public bool IsPartial { get; set; }
        public bool Unsafe { get; set; }


        public RfcTablePropertyAttribute(string name, string description = null, RfcEntityPropertySapTypes sapTypes = RfcEntityPropertySapTypes.STRING, int length = 0, string subTypePropertyName = null, bool isPartial = false, bool unSafe = false) : base(name)
        {
            Name = name;
            Description = description ?? string.Empty;
            EntityPropertySapType = sapTypes;
            Length = length;
            SubTypePropertyName = subTypePropertyName ?? string.Empty;
            IsPartial = isPartial;
            Unsafe = unSafe;
        }

    }
}
