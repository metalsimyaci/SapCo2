using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Extension;

namespace SapCo2.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSapCo2Core(this IServiceCollection serviceCollection, Action<RfcConnectionOption> connectionAction)
        {
            serviceCollection.AddSapCo2Wrapper();

            serviceCollection.AddOptions<RfcConnectionOption>();
            serviceCollection.Configure(connectionAction);

            serviceCollection.TryAddTransient<IRfcConnection, RfcConnection>();
            serviceCollection.TryAddTransient<IRfcFunction, RfcFunction>();
            serviceCollection.TryAddSingleton<IRfcLibrary, RfcLibrary>();

            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
        public static IServiceCollection AddSapCo2Core(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddSapCo2Wrapper();

            serviceCollection.AddOptions<RfcConnectionOption>();
            serviceCollection.Configure<RfcConnectionOption>(o => o.Parse(connectionString));

            serviceCollection.TryAddTransient<IRfcConnection, RfcConnection>();
            serviceCollection.TryAddTransient<IRfcFunction, RfcFunction>();
            serviceCollection.TryAddSingleton<IRfcLibrary, RfcLibrary>();

            serviceCollection.BuildServiceProvider();
            return serviceCollection;
        }
        private static RfcConnectionOption Parse(this RfcConnectionOption connectionOption, string connectionString)
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
