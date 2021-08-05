using System.Diagnostics.CodeAnalysis;
using SapCo2.Core.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SapCo2.Core.Models
{
    [ExcludeFromCodeCoverage]
    public class RfcConnectionOption
    {
        /// <summary>
        /// Gets or sets the Application Server Host parameter.
        /// </summary>
        [RfcConnectionProperty("ASHOST")]
        public string AppServerHost { get; set; }

        /// <summary>
        /// Gets or sets the SNC Library Path parameter.
        /// </summary>
        [RfcConnectionProperty("SNC_LIB")]
        public string SncLibraryPath { get; set; }

        /// <summary>
        /// Gets or sets the SNC QOP parameter.
        /// </summary>
        [RfcConnectionProperty("SNC_QOP")]
        public string SncQop { get; set; }

        /// <summary>
        /// Gets or sets the Trace parameter.
        /// </summary>
        [RfcConnectionProperty("TRACE")]
        public string Trace { get; set; }

        /// <summary>
        /// Gets or sets the SAP Router parameter.
        /// </summary>
        [RfcConnectionProperty("SAPROUTER")]
        public string SapRouter { get; set; }

        /// <summary>
        /// Gets or sets the No Compression parameter.
        /// </summary>
        [RfcConnectionProperty("NO_COMPRESSION")]
        public string NoCompression { get; set; }

        /// <summary>
        /// Gets or sets the On Character Conversion Error parameter.
        /// </summary>
        [RfcConnectionProperty("ON_CCE")]
        public string OnCharacterConversionError { get; set; }

        /// <summary>
        /// Gets or sets the Character Fault Indicator Token parameter.
        /// </summary>
        [RfcConnectionProperty("CFIT")]
        public string CharacterFaultIndicatorToken { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Pool Size parameter.
        /// </summary>
        [RfcConnectionProperty("MAX_POOL_SIZE")]
        public string MaxPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the Pool Size parameter.
        /// </summary>
        [RfcConnectionProperty("POOL_SIZE")]
        public string PoolSize { get; set; }

        /// <summary>
        /// Gets or sets the SNC Partner Names parameter.
        /// </summary>
        [RfcConnectionProperty("SNC_PARTNER_NAMES")]
        public string SncPartnerNames { get; set; }

        /// <summary>
        /// Gets or sets the Idle Timeout parameter.
        /// </summary>
        [RfcConnectionProperty("IDLE_TIMEOUT")]
        public string IdleTimeout { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Pool Wait Time parameter.
        /// </summary>
        [RfcConnectionProperty("MAX_POOL_WAIT_TIME")]
        public string MaxPoolWaitTime { get; set; }

        /// <summary>
        /// Gets or sets the Registration Count parameter.
        /// </summary>
        [RfcConnectionProperty("REG_COUNT")]
        public string RegistrationCount { get; set; }

        /// <summary>
        /// Gets or sets the Password Change Enforced parameter.
        /// </summary>
        [RfcConnectionProperty("PASSWORD_CHANGE_ENFORCED")]
        public string PasswordChangeEnforced { get; set; }

        /// <summary>
        /// Gets or sets the Name parameter.
        /// </summary>
        [RfcConnectionProperty("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Repository Destination parameter.
        /// </summary>
        [RfcConnectionProperty("REPOSITORY_DESTINATION")]
        public string RepositoryDestination { get; set; }

        /// <summary>
        /// Gets or sets the Repository User parameter.
        /// </summary>
        [RfcConnectionProperty("REPOSITORY_USER")]
        public string RepositoryUser { get; set; }

        /// <summary>
        /// Gets or sets the Repository Password parameter.
        /// </summary>
        [RfcConnectionProperty("REPOSITORY_PASSWD")]
        public string RepositoryPassword { get; set; }

        /// <summary>
        /// Gets or sets the Repository SNC My Name parameter.
        /// </summary>
        [RfcConnectionProperty("REPOSITORY_SNC_MYNAME")]
        public string RepositorySncMyName { get; set; }

        /// <summary>
        /// Gets or sets the Reporitory X509 Certificate parameter.
        /// </summary>
        [RfcConnectionProperty("REPOSITORY_X509CERT")]
        public string RepositoryX509Certificate { get; set; }

        /// <summary>
        /// Gets or sets the Idle Check Time parameter.
        /// </summary>
        [RfcConnectionProperty("IDLE_CHECK_TIME")]
        public string IdleCheckTime { get; set; }

        /// <summary>
        /// Gets or sets the SNC My Name parameter.
        /// </summary>
        [RfcConnectionProperty("SNC_MYNAME")]
        public string SncMyName { get; set; }

        /// <summary>
        /// Gets or sets the SNC Partner Name parameter.
        /// </summary>
        [RfcConnectionProperty("SNC_PARTNERNAME")]
        public string SncPartnerName { get; set; }

        /// <summary>
        /// Gets or sets the Program Id parameter.
        /// </summary>
        [RfcConnectionProperty("PROGRAM_ID")]
        public string ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the App Server Service parameter.
        /// </summary>
        [RfcConnectionProperty("ASSERV")]
        public string AppServerService { get; set; }

        /// <summary>
        /// Gets or sets the Message Server Host parameter.
        /// </summary>
        [RfcConnectionProperty("MSHOST")]
        public string MessageServerHost { get; set; }

        /// <summary>
        /// Gets or sets the Message Server Service parameter.
        /// </summary>
        [RfcConnectionProperty("MSSERV")]
        public string MessageServerService { get; set; }

        /// <summary>
        /// Gets or sets the Logon Group parameter.
        /// </summary>
        [RfcConnectionProperty("GROUP")]
        public string LogonGroup { get; set; }

        /// <summary>
        /// Gets or sets the Gateway Host parameter.
        /// </summary>
        [RfcConnectionProperty("GWHOST")]
        public string GatewayHost { get; set; }

        /// <summary>
        /// Gets or sets the Gateway Service parameter.
        /// </summary>
        [RfcConnectionProperty("GWSERV")]
        public string GatewayService { get; set; }

        /// <summary>
        /// Gets or sets the System Number parameter.
        /// </summary>
        [RfcConnectionProperty("SYSNR")]
        public string SystemNumber { get; set; }

        /// <summary>
        /// Gets or sets the User parameter.
        /// </summary>
        [RfcConnectionProperty("USER")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the Alias User parameter.
        /// </summary>
        [RfcConnectionProperty("ALIAS_USER")]
        public string AliasUser { get; set; }

        /// <summary>
        /// Gets or sets the SNC Mode parameter.
        /// </summary>
        [RfcConnectionProperty("SNC_MODE")]
        public string SncMode { get; set; }

        /// <summary>
        /// Gets or sets the Client parameter.
        /// </summary>
        [RfcConnectionProperty("CLIENT")]
        public string Client { get; set; }

        /// <summary>
        /// Gets or sets the Password parameter.
        /// </summary>
        [RfcConnectionProperty("PASSWD")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Codepage parameter.
        /// </summary>
        [RfcConnectionProperty("CODEPAGE")]
        public string Codepage { get; set; }

        /// <summary>
        /// Gets or sets the Partner Char Size parameter.
        /// </summary>
        [RfcConnectionProperty("PCS")]
        public string PartnerCharSize { get; set; }

        /// <summary>
        /// Gets or sets the System ID parameter.
        /// </summary>
        [RfcConnectionProperty("SYSID")]
        public string SystemId { get; set; }

        /// <summary>
        /// Gets or sets the Systems ID parameter.
        /// </summary>
        [RfcConnectionProperty("SYS_IDS")]
        public string SystemIds { get; set; }

        /// <summary>
        /// Gets or sets the X509 Certificate parameter.
        /// </summary>
        [RfcConnectionProperty("X509CERT")]
        public string X509Certificate { get; set; }

        /// <summary>
        /// Gets or sets the SAP SSO2 Ticket parameter.
        /// </summary>
        [RfcConnectionProperty("MYSAPSSO2")]
        public string SapSso2Ticket { get; set; }

        /// <summary>
        /// Gets or sets the Use SAP GUI parameter.
        /// </summary>
        [RfcConnectionProperty("USE_SAPGUI")]
        public string UseSapGui { get; set; }

        /// <summary>
        /// Gets or sets the ABAP Debug parameter.
        /// </summary>
        [RfcConnectionProperty("ABAP_DEBUG")]
        public string AbapDebug { get; set; }

        /// <summary>
        /// Gets or sets the Logon Check parameter.
        /// </summary>
        [RfcConnectionProperty("LCHECK")]
        public string LogonCheck { get; set; }

        /// <summary>
        /// Gets or sets the Language parameter.
        /// </summary>
        [RfcConnectionProperty("LANG")]
        public string Language { get; set; }
    }
}
