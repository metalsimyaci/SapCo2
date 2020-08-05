using System;

namespace SapCo2.Wrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcConnectionAttribute: Attribute
    {
        /// <summary>
        /// SAP RFC connection property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="RfcConnectionAttribute"/> class
        /// </summary>
        /// <param name="name">SAP RFC Connection property name</param>
        public RfcConnectionAttribute(string name)
        {
            Name = name;
        }
    }

}
