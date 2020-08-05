using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SapCo2.Abstract;
using SapCo2.Wrapper.Attributes;

namespace SapCo2.Utility
{
    public class PropertyCache:IPropertyCache
    {
        private ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> Cache { get; set; }

        public PropertyCache()
        {
            Cache = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();
        }
        public void AddToPropertyCache(Type type)
        {
            if (Cache.ContainsKey(type))
                return;

            IEnumerable<PropertyInfo> properties = type.GetProperties().Where(x => !x.GetGetMethod().IsVirtual).Select(x => x);
            Dictionary<string, PropertyInfo> propertyDictionary = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo info in properties)
            {
                if (!Attribute.IsDefined(info, typeof(RfcPropertyAttribute)))
                    continue;

                var attribute = (RfcPropertyAttribute)info.GetCustomAttributes(typeof(RfcPropertyAttribute), false).FirstOrDefault();
                string field = attribute?.Name;

                if (field != null)
                    propertyDictionary.Add(field, info);
            }

            Cache.TryAdd(type, propertyDictionary);
        }

        public PropertyInfo GetPropertyInfo(Type type, string key)
        {
            AddToPropertyCache(type);

            if (!Cache.ContainsKey(type))
                return null;

            Cache.TryGetValue(type, out Dictionary<string, PropertyInfo> result);

            if(result?.ContainsKey(key)??false)
                return result?[key];
            
            return null;
        }

        public Dictionary<string, PropertyInfo> GetPropertyInfos(Type type)
        {
            AddToPropertyCache(type);

            if (!Cache.ContainsKey(type))
                return null;

            Cache.TryGetValue(type, out Dictionary<string, PropertyInfo> result);
            return result;
        }
    }
}
