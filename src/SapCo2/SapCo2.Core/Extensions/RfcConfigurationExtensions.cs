using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Exception;
// ReSharper disable InconsistentNaming

namespace SapCo2.Core.Extensions
{
    public static class RfcConfigurationExtensions
    {
        #region Constants

        private const string DEFAULT_CONFIGURATION_KEY = "SapCo2";
        private const string DEFAULT_CONNECTION_KEY = "DefaultServer";
        private const string RFC_SERVERS_KEY = "RfcConnections";
        private const string ALIAS_KEY = "Alias";
        private const string CONNECTION_POOLING_KEY = "ConnectionPooling";
        private const string CONNECTION_STRING_KEY = "ConnectionString";
        private const string CONNECTION_OPTIONS_KEY = "ConnectionOptions";

        #endregion

        #region Methods

        public static RfcConfiguration ReadFromConfiguration(this RfcConfiguration rfcConfiguration, IConfiguration configuration)
        {
            return ConfigureByConfiguration(rfcConfiguration, configuration, DEFAULT_CONFIGURATION_KEY);
        }

        private static RfcConfiguration CreateConfiguration(this RfcConfiguration rfcConfiguration, string defaultAlias, IEnumerable<RfcServer> rfcConnections)
        {
            return ConfigureByRfcConnection(rfcConfiguration, defaultAlias, rfcConnections);
        }

        private static RfcConfiguration CreateConfiguration(this RfcConfiguration rfcConfiguration, string defaultAlias, string connectionString)
        {
            return ConfigureByRfcConnectionString(rfcConfiguration, defaultAlias, connectionString);
        }

        public static RfcConfiguration ReadFromConfiguration(this RfcConfiguration rfcConfiguration, IConfiguration configuration, string section)
        {
            return ConfigureByConfiguration(rfcConfiguration, configuration, section);
        }

        private static RfcConfiguration ConfigureByConfiguration(RfcConfiguration rfcConfiguration, IConfiguration configuration, string section)
        {
            IConfigurationSection configurationSection = configuration.GetSection(section);

            if (configurationSection == null || !configurationSection.Exists()) return rfcConfiguration;

            rfcConfiguration.DefaultServer = configurationSection.GetValue<string>(DEFAULT_CONNECTION_KEY);
            rfcConfiguration.RfcServers ??= new List<RfcServer>();
            rfcConfiguration.RfcServers.Clear();

            IConfigurationSection connectionsSection = configurationSection.GetSection(RFC_SERVERS_KEY);

            if (connectionsSection == null || !connectionsSection.Exists()) return rfcConfiguration;

            foreach (IConfigurationSection rfcServerConnectionSection in connectionsSection.GetChildren())
            {
                string alias = rfcServerConnectionSection.GetValue<string>(ALIAS_KEY);
                if (rfcConfiguration.RfcServers.Any(s => s.Alias == alias))
                    throw new RfcException("Application settings are not configured correctly. Multiple servers have the same 'Alias' value.");
              
                if (string.IsNullOrWhiteSpace(alias))
                    throw new RfcException("Application settings are not configured correctly. Required value is not entered in server settings: 'Alias'");

                var rfcServerConnection= new RfcServer { Alias = alias };

                IConfigurationSection poolingSection = rfcServerConnectionSection.GetSection(CONNECTION_POOLING_KEY);

                if (poolingSection != null && poolingSection.Exists())
                    poolingSection.Bind(rfcServerConnection.ConnectionPooling);

                rfcServerConnection.ConnectionString = rfcServerConnectionSection.GetValue<string>(CONNECTION_STRING_KEY);

                if (!string.IsNullOrWhiteSpace(rfcServerConnection.ConnectionString))
                    rfcServerConnection.ConnectionOptions.Parse(rfcServerConnection.ConnectionString);

                IConfigurationSection connectionOptionsSection = rfcServerConnectionSection.GetSection(CONNECTION_OPTIONS_KEY);

                if (connectionOptionsSection != null && connectionOptionsSection.Exists())
                    connectionOptionsSection.Bind(rfcServerConnection.ConnectionOptions);

                rfcConfiguration.RfcServers.Add(rfcServerConnection);
            }

            if (rfcConfiguration.RfcServers.Count > 1 && !rfcConfiguration.RfcServers.Exists(s => s.Alias == rfcConfiguration.DefaultServer))
                throw new RfcException("Application settings are not configured correctly. The default server value is not included in the server definitions.");

            return rfcConfiguration;
        }

        private static RfcConfiguration ConfigureByRfcConnection(RfcConfiguration rfcConfiguration, string defaultConnectionAlias, IEnumerable<RfcServer> rfcConnections)
        {
            rfcConfiguration.DefaultServer = defaultConnectionAlias;
            rfcConfiguration.RfcServers ??= new List<RfcServer>();
            rfcConfiguration.RfcServers.Clear();

            foreach (var rfcConnection in rfcConnections)
            {
                if (rfcConfiguration.RfcServers.Any(s => s.Alias == rfcConnection.Alias))
                    throw new RfcException("Multiple servers have the same 'Alias' value.");

                if (string.IsNullOrWhiteSpace(rfcConnection.Alias))
                    throw new RfcException("Required value is not entered in server settings: 'Alias'");

                var rfcServerConnection = new RfcServer { Alias = rfcConnection.Alias };

                if (rfcConnection.ConnectionPooling != null)
                    rfcServerConnection.ConnectionPooling = rfcConnection.ConnectionPooling;

                if (!string.IsNullOrWhiteSpace(rfcConnection.ConnectionString))
                    rfcServerConnection.ConnectionOptions.Parse(rfcConnection.ConnectionString);

                if (rfcConnection.ConnectionOptions != null)
                    rfcServerConnection.ConnectionOptions = rfcConnection.ConnectionOptions;
                
                rfcConfiguration.RfcServers.Add(rfcServerConnection);
            }

            if (rfcConfiguration.RfcServers.Count > 1 && !rfcConfiguration.RfcServers.Exists(s => s.Alias == rfcConfiguration.DefaultServer))
                throw new RfcException("The default server value is not included in the server definitions.");

            return rfcConfiguration;
        }

        private static RfcConfiguration ConfigureByRfcConnectionString(RfcConfiguration rfcConfiguration, string defaultAlias,string connectionString)
        {
            rfcConfiguration.DefaultServer = defaultAlias;
            rfcConfiguration.RfcServers ??= new List<RfcServer>();
            rfcConfiguration.RfcServers.Clear();


            if (string.IsNullOrWhiteSpace(defaultAlias))
                throw new RfcException("Required value is not entered in server settings: 'Alias'");

            var rfcServerConnection = new RfcServer { Alias = defaultAlias };

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new RfcException("'ConnectionString' is required, please check your 'ConnectionString' information and try again.");

            rfcServerConnection.ConnectionOptions.Parse(connectionString);
            rfcConfiguration.RfcServers.Add(rfcServerConnection);

            if (rfcConfiguration.RfcServers.Count > 1 && !rfcConfiguration.RfcServers.Exists(s => s.Alias == rfcConfiguration.DefaultServer))
                throw new RfcException("The default server value is not included in the server definitions.");

            return rfcConfiguration;
        }

        #endregion
    }
}
