using System.Collections.Generic;

namespace SapCo2.Core.Models
{
    public class RfcConfiguration
    {
        public string DefaultServer { get; set; }
        public List<RfcServer> RfcServers { get; set; }

        public RfcConfiguration()
        {
            DefaultServer = string.Empty;
            RfcServers = new List<RfcServer>();
        }
    }
}
