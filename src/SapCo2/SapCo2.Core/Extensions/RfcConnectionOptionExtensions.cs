using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core.Extensions
{
    public static class RfcConnectionOptionExtensions
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

        internal static RfcConnectionOption Parse(this RfcConnectionOption connectionOption, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Value cannot be null or empty", nameof(connectionString));

            IReadOnlyDictionary<string, string> parts = connectionString
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(entry => Regex.Match(entry, @"^\s*(?<key>\S+)\s*=\s*(?<value>\S+)\s*$"))
                .Where(match => match.Success)
                .ToDictionary(match => match.Groups["key"].Value, match => match.Groups["value"].Value);

            return typeof(RfcConnectionOption)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Aggregate(connectionOption, (parameters, propertyInfo) =>
                {
                    if (parts.ContainsKey(propertyInfo.Name) && propertyInfo.CanWrite)
                        propertyInfo.SetValue(parameters, parts[propertyInfo.Name]);
                    return parameters;
                });
        }
    }
}
