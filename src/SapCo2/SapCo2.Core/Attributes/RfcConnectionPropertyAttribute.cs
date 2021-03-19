using System;

namespace SapCo2.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcConnectionPropertyAttribute: Attribute
    {
        internal string Name { get; set; }
        
        public RfcConnectionPropertyAttribute(string name)
        {
            Name = name;
        }
    }

}
