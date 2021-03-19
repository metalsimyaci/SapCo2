using System;
using SapCo2.Abstraction.Enumerations;

namespace SapCo2.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RfcEntityPropertyAttribute : Attribute
    {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public RfcDataTypes SapDataType { get; set; }
        public int Length { get; set; }
        public string SubTypePropertyName { get; set; }
        public bool IsPartial { get; set; }
        public bool Unsafe { get; set; }

        #endregion

        #region Methods

        #region Constructors

        public RfcEntityPropertyAttribute(string name)
        {
            Name = name;
            Description = string.Empty;
            SapDataType = RfcDataTypes.STRING;
            Length = default;
            SubTypePropertyName = string.Empty;
            IsPartial = false;
            Unsafe = false;
        }

        public RfcEntityPropertyAttribute(string name, string description)
        {
            Name = name;
            Description = description ?? string.Empty;
            SapDataType = RfcDataTypes.STRING;
            Length = default;
            SubTypePropertyName = string.Empty;
            IsPartial = false;
            Unsafe = false;
        }

        public RfcEntityPropertyAttribute(string name, string description = null, RfcDataTypes sapDataType = RfcDataTypes.STRING, int length = default, string subTypePropertyName = null,
            bool isPartial = false, bool unSafe = false)
        {
            Name = name;
            Description = description ?? string.Empty;
            SapDataType = sapDataType;
            Length = length;
            SubTypePropertyName = subTypePropertyName ?? string.Empty;
            IsPartial = isPartial;
            Unsafe = unSafe;
        }

        #endregion

        #endregion
    }
}
