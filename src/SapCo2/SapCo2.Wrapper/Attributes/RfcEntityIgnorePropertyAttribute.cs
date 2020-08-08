using System;

namespace SapCo2.Wrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcEntityIgnorePropertyAttribute:Attribute
    {
        public RfcEntityIgnorePropertyAttribute()
        {
            
        }
    }
}
