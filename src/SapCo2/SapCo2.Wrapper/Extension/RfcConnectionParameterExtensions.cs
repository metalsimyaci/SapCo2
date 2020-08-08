using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Extension
{
    public static class RfcConnectionParameterExtensions
    {
        private static readonly ConcurrentDictionary<Type, (string name, Func<object, string> GetValue)[]>
            TypePropertiesCache =
                new ConcurrentDictionary<Type, (string name, Func<object, string> GetValue)[]>();

        public static RfcConnectionParameter[] ToInterop<TParameter>(this TParameter parameters)
        {
            (string Name, Func<object, string> GetValue)[] properties =
                TypePropertiesCache.GetOrAdd(typeof(TParameter), Build);
            return properties.Select(property =>
                    new RfcConnectionParameter {Name = property.Name, Value = property.GetValue(parameters),})
                .Where(parameter => !string.IsNullOrEmpty(parameter.Value))
                .ToArray();
        }

        private static (string Name, Func<object, string> GetValue)[] Build(Type type)
            => type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(propertyInfo =>
                {
                    RfcConnectionPropertyAttribute namePropertyAttribute =
                        propertyInfo.GetCustomAttribute<RfcConnectionPropertyAttribute>();

                    ParameterExpression instanceParameter = Expression.Parameter(typeof(object));
                    var propertyValueResolver = Expression.Lambda<Func<object, string>>(
                        Expression.Property(Expression.Convert(instanceParameter, type), propertyInfo),
                        instanceParameter);

                    return (
                        Name: namePropertyAttribute?.Name ?? propertyInfo.Name.ToUpper(),
                        GetValue: propertyValueResolver.Compile()
                    );
                })
                .ToArray();
    }
}
