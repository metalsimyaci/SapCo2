using System;

namespace SapCo2.Wrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcPropertyAttribute:Attribute
    {
        /// <summary>
        /// SAP RFC property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="RfcPropertyAttribute"/> class
        /// </summary>
        /// <param name="name">SAP RFC property name</param>
        public RfcPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
