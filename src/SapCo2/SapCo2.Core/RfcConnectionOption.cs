using SapCo2.Wrapper.Attributes;

namespace SapCo2.Core
{
    public class RfcConnectionOption
    {
        /// <summary>
        /// Gets or sets the Application Server Host parameter.
        /// </summary>
        [RfcConnection("ASHOST")]
        public string AppServerHost { get; set; }

        /// <summary>
        /// Gets or sets the SNC Library Path parameter.
        /// </summary>
        [RfcConnection("SNC_LIB")]
        public string SncLibraryPath { get; set; }

        /// <summary>
        /// Gets or sets the SNC QOP parameter.
        /// </summary>
        [RfcConnection("SNC_QOP")]
        public string SncQop { get; set; }

        /// <summary>
        /// Gets or sets the Trace parameter.
        /// </summary>
        [RfcConnection("TRACE")]
        public string Trace { get; set; }

        /// <summary>
        /// Gets or sets the SAP Router parameter.
        /// </summary>
        [RfcConnection("SAPROUTER")]
        public string SapRouter { get; set; }

        /// <summary>
        /// Gets or sets the No Compression parameter.
        /// </summary>
        [RfcConnection("NO_COMPRESSION")]
        public string NoCompression { get; set; }

        /// <summary>
        /// Gets or sets the On Character Conversion Error parameter.
        /// </summary>
        [RfcConnection("ON_CCE")]
        public string OnCharacterConversionError { get; set; }

        /// <summary>
        /// Gets or sets the Character Fault Indicator Token parameter.
        /// </summary>
        [RfcConnection("CFIT")]
        public string CharacterFaultIndicatorToken { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Pool Size parameter.
        /// </summary>
        [RfcConnection("MAX_POOL_SIZE")]
        public string MaxPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the Pool Size parameter.
        /// </summary>
        [RfcConnection("POOL_SIZE")]
        public string PoolSize { get; set; }

        /// <summary>
        /// Gets or sets the SNC Partner Names parameter.
        /// </summary>
        [RfcConnection("SNC_PARTNER_NAMES")]
        public string SncPartnerNames { get; set; }

        /// <summary>
        /// Gets or sets the Idle Timeout parameter.
        /// </summary>
        [RfcConnection("IDLE_TIMEOUT")]
        public string IdleTimeout { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Pool Wait Time parameter.
        /// </summary>
        [RfcConnection("MAX_POOL_WAIT_TIME")]
        public string MaxPoolWaitTime { get; set; }

        /// <summary>
        /// Gets or sets the Registration Count parameter.
        /// </summary>
        [RfcConnection("REG_COUNT")]
        public string RegistrationCount { get; set; }

        /// <summary>
        /// Gets or sets the Password Change Enforced parameter.
        /// </summary>
        [RfcConnection("PASSWORD_CHANGE_ENFORCED")]
        public string PasswordChangeEnforced { get; set; }

        /// <summary>
        /// Gets or sets the Name parameter.
        /// </summary>
        [RfcConnection("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Repository Destination parameter.
        /// </summary>
        [RfcConnection("REPOSITORY_DESTINATION")]
        public string RepositoryDestination { get; set; }

        /// <summary>
        /// Gets or sets the Repository User parameter.
        /// </summary>
        [RfcConnection("REPOSITORY_USER")]
        public string RepositoryUser { get; set; }

        /// <summary>
        /// Gets or sets the Repository Password parameter.
        /// </summary>
        [RfcConnection("REPOSITORY_PASSWD")]
        public string RepositoryPassword { get; set; }

        /// <summary>
        /// Gets or sets the Repository SNC My Name parameter.
        /// </summary>
        [RfcConnection("REPOSITORY_SNC_MYNAME")]
        public string RepositorySncMyName { get; set; }

        /// <summary>
        /// Gets or sets the Reporitory X509 Certificate parameter.
        /// </summary>
        [RfcConnection("REPOSITORY_X509CERT")]
        public string RepositoryX509Certificate { get; set; }

        /// <summary>
        /// Gets or sets the Idle Check Time parameter.
        /// </summary>
        [RfcConnection("IDLE_CHECK_TIME")]
        public string IdleCheckTime { get; set; }

        /// <summary>
        /// Gets or sets the SNC My Name parameter.
        /// </summary>
        [RfcConnection("SNC_MYNAME")]
        public string SncMyName { get; set; }

        /// <summary>
        /// Gets or sets the SNC Partner Name parameter.
        /// </summary>
        [RfcConnection("SNC_PARTNERNAME")]
        public string SncPartnerName { get; set; }

        /// <summary>
        /// Gets or sets the Program Id parameter.
        /// </summary>
        [RfcConnection("PROGRAM_ID")]
        public string ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the App Server Service parameter.
        /// </summary>
        [RfcConnection("ASSERV")]
        public string AppServerService { get; set; }

        /// <summary>
        /// Gets or sets the Message Server Host parameter.
        /// </summary>
        [RfcConnection("MSHOST")]
        public string MessageServerHost { get; set; }

        /// <summary>
        /// Gets or sets the Message Server Service parameter.
        /// </summary>
        [RfcConnection("MSSERV")]
        public string MessageServerService { get; set; }

        /// <summary>
        /// Gets or sets the Logon Group parameter.
        /// </summary>
        [RfcConnection("GROUP")]
        public string LogonGroup { get; set; }

        /// <summary>
        /// Gets or sets the Gateway Host parameter.
        /// </summary>
        [RfcConnection("GWHOST")]
        public string GatewayHost { get; set; }

        /// <summary>
        /// Gets or sets the Gateway Service parameter.
        /// </summary>
        [RfcConnection("GWSERV")]
        public string GatewayService { get; set; }

        /// <summary>
        /// Gets or sets the System Number parameter.
        /// </summary>
        [RfcConnection("SYSNR")]
        public string SystemNumber { get; set; }

        /// <summary>
        /// Gets or sets the User parameter.
        /// </summary>
        [RfcConnection("USER")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the Alias User parameter.
        /// </summary>
        [RfcConnection("ALIAS_USER")]
        public string AliasUser { get; set; }

        /// <summary>
        /// Gets or sets the SNC Mode parameter.
        /// </summary>
        [RfcConnection("SNC_MODE")]
        public string SncMode { get; set; }

        /// <summary>
        /// Gets or sets the Client parameter.
        /// </summary>
        [RfcConnection("CLIENT")]
        public string Client { get; set; }

        /// <summary>
        /// Gets or sets the Password parameter.
        /// </summary>
        [RfcConnection("PASSWD")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Codepage parameter.
        /// </summary>
        [RfcConnection("CODEPAGE")]
        public string Codepage { get; set; }

        /// <summary>
        /// Gets or sets the Partner Char Size parameter.
        /// </summary>
        [RfcConnection("PCS")]
        public string PartnerCharSize { get; set; }

        /// <summary>
        /// Gets or sets the System ID parameter.
        /// </summary>
        [RfcConnection("SYSID")]
        public string SystemId { get; set; }

        /// <summary>
        /// Gets or sets the Systems ID parameter.
        /// </summary>
        [RfcConnection("SYS_IDS")]
        public string SystemIds { get; set; }

        /// <summary>
        /// Gets or sets the X509 Certificate parameter.
        /// </summary>
        [RfcConnection("X509CERT")]
        public string X509Certificate { get; set; }

        /// <summary>
        /// Gets or sets the SAP SSO2 Ticket parameter.
        /// </summary>
        [RfcConnection("MYSAPSSO2")]
        public string SapSso2Ticket { get; set; }

        /// <summary>
        /// Gets or sets the Use SAP GUI parameter.
        /// </summary>
        [RfcConnection("USE_SAPGUI")]
        public string UseSapGui { get; set; }

        /// <summary>
        /// Gets or sets the ABAP Debug parameter.
        /// </summary>
        [RfcConnection("ABAP_DEBUG")]
        public string AbapDebug { get; set; }

        /// <summary>
        /// Gets or sets the Logon Check parameter.
        /// </summary>
        [RfcConnection("LCHECK")]
        public string LogonCheck { get; set; }

        /// <summary>
        /// Gets or sets the Language parameter.
        /// </summary>
        [RfcConnection("LANG")]
        public string Language { get; set; }
    }
}
