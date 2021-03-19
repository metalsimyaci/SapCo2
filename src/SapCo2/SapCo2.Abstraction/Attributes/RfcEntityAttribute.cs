using System;

namespace SapCo2.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RfcEntityAttribute : Attribute
    {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Unsafe { get; set; }

        #endregion

        #region Methods

        #region Constructors

        public RfcEntityAttribute(string name)
        {
            Name = name;
            Description = string.Empty;
            Unsafe = false;
        }

        #endregion

        #endregion
    }
}
