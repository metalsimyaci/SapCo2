using System;

namespace SapCo2.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RfcTableAttribute:Attribute
    {
        public string Name { get; private set; }
        public string Description { get; set; } = string.Empty;
        public bool Unsafe { get; set; }

        public RfcTableAttribute(string name)
        {
            Name = name;
        }
    }
}
