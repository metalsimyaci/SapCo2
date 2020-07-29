using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SapCo2.Wrapper.Attributes;

namespace SapCo2.Core.Models
{
    public class ConnectionParameter
    {
        /// <summary>
        /// Gets or sets the Application Server Host parameter.
        /// </summary>
        [RfcProperty("ASHOST")]
        public string AppServerHost { get; set; }

        /// <summary>
        /// Gets or sets the SNC Library Path parameter.
        /// </summary>
        [RfcProperty("SNC_LIB")]
        public string SncLibraryPath { get; set; }

        /// <summary>
        /// Gets or sets the SNC QOP parameter.
        /// </summary>
        [RfcProperty("SNC_QOP")]
        public string SncQop { get; set; }

        /// <summary>
        /// Gets or sets the Trace parameter.
        /// </summary>
        [RfcProperty("TRACE")]
        public string Trace { get; set; }

        /// <summary>
        /// Gets or sets the SAP Router parameter.
        /// </summary>
        [RfcProperty("SAPROUTER")]
        public string SapRouter { get; set; }

        /// <summary>
        /// Gets or sets the No Compression parameter.
        /// </summary>
        [RfcProperty("NO_COMPRESSION")]
        public string NoCompression { get; set; }

        /// <summary>
        /// Gets or sets the On Character Conversion Error parameter.
        /// </summary>
        [RfcProperty("ON_CCE")]
        public string OnCharacterConversionError { get; set; }

        /// <summary>
        /// Gets or sets the Character Fault Indicator Token parameter.
        /// </summary>
        [RfcProperty("CFIT")]
        public string CharacterFaultIndicatorToken { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Pool Size parameter.
        /// </summary>
        [RfcProperty("MAX_POOL_SIZE")]
        public string MaxPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the Pool Size parameter.
        /// </summary>
        [RfcProperty("POOL_SIZE")]
        public string PoolSize { get; set; }

        /// <summary>
        /// Gets or sets the SNC Partner Names parameter.
        /// </summary>
        [RfcProperty("SNC_PARTNER_NAMES")]
        public string SncPartnerNames { get; set; }

        /// <summary>
        /// Gets or sets the Idle Timeout parameter.
        /// </summary>
        [RfcProperty("IDLE_TIMEOUT")]
        public string IdleTimeout { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Pool Wait Time parameter.
        /// </summary>
        [RfcProperty("MAX_POOL_WAIT_TIME")]
        public string MaxPoolWaitTime { get; set; }

        /// <summary>
        /// Gets or sets the Registration Count parameter.
        /// </summary>
        [RfcProperty("REG_COUNT")]
        public string RegistrationCount { get; set; }

        /// <summary>
        /// Gets or sets the Password Change Enforced parameter.
        /// </summary>
        [RfcProperty("PASSWORD_CHANGE_ENFORCED")]
        public string PasswordChangeEnforced { get; set; }

        /// <summary>
        /// Gets or sets the Name parameter.
        /// </summary>
        [RfcProperty("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Repository Destination parameter.
        /// </summary>
        [RfcProperty("REPOSITORY_DESTINATION")]
        public string RepositoryDestination { get; set; }

        /// <summary>
        /// Gets or sets the Repository User parameter.
        /// </summary>
        [RfcProperty("REPOSITORY_USER")]
        public string RepositoryUser { get; set; }

        /// <summary>
        /// Gets or sets the Repository Password parameter.
        /// </summary>
        [RfcProperty("REPOSITORY_PASSWD")]
        public string RepositoryPassword { get; set; }

        /// <summary>
        /// Gets or sets the Repository SNC My Name parameter.
        /// </summary>
        [RfcProperty("REPOSITORY_SNC_MYNAME")]
        public string RepositorySncMyName { get; set; }

        /// <summary>
        /// Gets or sets the Reporitory X509 Certificate parameter.
        /// </summary>
        [RfcProperty("REPOSITORY_X509CERT")]
        public string RepositoryX509Certificate { get; set; }

        /// <summary>
        /// Gets or sets the Idle Check Time parameter.
        /// </summary>
        [RfcProperty("IDLE_CHECK_TIME")]
        public string IdleCheckTime { get; set; }

        /// <summary>
        /// Gets or sets the SNC My Name parameter.
        /// </summary>
        [RfcProperty("SNC_MYNAME")]
        public string SncMyName { get; set; }

        /// <summary>
        /// Gets or sets the SNC Partner Name parameter.
        /// </summary>
        [RfcProperty("SNC_PARTNERNAME")]
        public string SncPartnerName { get; set; }

        /// <summary>
        /// Gets or sets the Program Id parameter.
        /// </summary>
        [RfcProperty("PROGRAM_ID")]
        public string ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the App Server Service parameter.
        /// </summary>
        [RfcProperty("ASSERV")]
        public string AppServerService { get; set; }

        /// <summary>
        /// Gets or sets the Message Server Host parameter.
        /// </summary>
        [RfcProperty("MSHOST")]
        public string MessageServerHost { get; set; }

        /// <summary>
        /// Gets or sets the Message Server Service parameter.
        /// </summary>
        [RfcProperty("MSSERV")]
        public string MessageServerService { get; set; }

        /// <summary>
        /// Gets or sets the Logon Group parameter.
        /// </summary>
        [RfcProperty("GROUP")]
        public string LogonGroup { get; set; }

        /// <summary>
        /// Gets or sets the Gateway Host parameter.
        /// </summary>
        [RfcProperty("GWHOST")]
        public string GatewayHost { get; set; }

        /// <summary>
        /// Gets or sets the Gateway Service parameter.
        /// </summary>
        [RfcProperty("GWSERV")]
        public string GatewayService { get; set; }

        /// <summary>
        /// Gets or sets the System Number parameter.
        /// </summary>
        [RfcProperty("SYSNR")]
        public string SystemNumber { get; set; }

        /// <summary>
        /// Gets or sets the User parameter.
        /// </summary>
        [RfcProperty("USER")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the Alias User parameter.
        /// </summary>
        [RfcProperty("ALIAS_USER")]
        public string AliasUser { get; set; }

        /// <summary>
        /// Gets or sets the SNC Mode parameter.
        /// </summary>
        [RfcProperty("SNC_MODE")]
        public string SncMode { get; set; }

        /// <summary>
        /// Gets or sets the Client parameter.
        /// </summary>
        [RfcProperty("CLIENT")]
        public string Client { get; set; }

        /// <summary>
        /// Gets or sets the Password parameter.
        /// </summary>
        [RfcProperty("PASSWD")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Codepage parameter.
        /// </summary>
        [RfcProperty("CODEPAGE")]
        public string Codepage { get; set; }

        /// <summary>
        /// Gets or sets the Partner Char Size parameter.
        /// </summary>
        [RfcProperty("PCS")]
        public string PartnerCharSize { get; set; }

        /// <summary>
        /// Gets or sets the System ID parameter.
        /// </summary>
        [RfcProperty("SYSID")]
        public string SystemId { get; set; }

        /// <summary>
        /// Gets or sets the Systems ID parameter.
        /// </summary>
        [RfcProperty("SYS_IDS")]
        public string SystemIds { get; set; }

        /// <summary>
        /// Gets or sets the X509 Certificate parameter.
        /// </summary>
        [RfcProperty("X509CERT")]
        public string X509Certificate { get; set; }

        /// <summary>
        /// Gets or sets the SAP SSO2 Ticket parameter.
        /// </summary>
        [RfcProperty("MYSAPSSO2")]
        public string SapSso2Ticket { get; set; }

        /// <summary>
        /// Gets or sets the Use SAP GUI parameter.
        /// </summary>
        [RfcProperty("USE_SAPGUI")]
        public string UseSapGui { get; set; }

        /// <summary>
        /// Gets or sets the ABAP Debug parameter.
        /// </summary>
        [RfcProperty("ABAP_DEBUG")]
        public string AbapDebug { get; set; }

        /// <summary>
        /// Gets or sets the Logon Check parameter.
        /// </summary>
        [RfcProperty("LCHECK")]
        public string LogonCheck { get; set; }

        /// <summary>
        /// Gets or sets the Language parameter.
        /// </summary>
        [RfcProperty("LANG")]
        public string Language { get; set; }

        /// <summary>
        /// Parses a connection string into a <see cref="ConnectionParameter"/> object.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="ConnectionParameter"/> instance.</returns>
        public static ConnectionParameter Parse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Value cannot be null or empty", nameof(connectionString));

            IReadOnlyDictionary<string, string> parts = connectionString
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(entry => Regex.Match(entry, @"^\s*(?<key>\S+)\s*=\s*(?<value>\S+)\s*$"))
                .Where(match => match.Success)
                .ToDictionary(match => match.Groups["key"].Value, match => match.Groups["value"].Value);

            return typeof(ConnectionParameter)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Aggregate(new ConnectionParameter(), (parameters, propertyInfo) =>
                {
                    if (parts.ContainsKey(propertyInfo.Name) && propertyInfo.CanWrite)
                        propertyInfo.SetValue(parameters, parts[propertyInfo.Name]);
                    return parameters;
                });

        }
    }
}
