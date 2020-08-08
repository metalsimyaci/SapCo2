using System;

namespace SapCo2.Wrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RfcEntityClassAttribute:Attribute
    {
        public string Name { get; private set; }
        public string Description { get; set; } = string.Empty;
        public bool Unsafe { get; set; }

        public RfcEntityClassAttribute(string name)
        {
            Name = name;
        }
    }
}
