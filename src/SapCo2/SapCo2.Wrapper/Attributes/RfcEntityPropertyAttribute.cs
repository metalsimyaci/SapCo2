using System;

namespace SapCo2.Wrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcEntityPropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public RfcEntityPropertyAttribute(string name)
        {
            Name = name;
        }

        public RfcEntityPropertyAttribute(string name, string description)
        {
            Name = name;
            Description = description ?? string.Empty;
        }
    }
}
