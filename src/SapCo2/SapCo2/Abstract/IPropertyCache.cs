using System;
using System.Collections.Generic;
using System.Reflection;

namespace SapCo2.Abstract
{
    public interface IPropertyCache
    {
        void AddToPropertyCache(Type type);
        PropertyInfo GetPropertyInfo(Type type, string key);
        Dictionary<string, PropertyInfo> GetPropertyInfos(Type type);
    }
}
