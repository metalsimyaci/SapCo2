using System.Collections.Generic;
using SapCo2.Core.Abstract;

namespace SapCo2.Core.Models
{
    public class RfcConfiguration
    {
        public string DefaultServer { get; set; }
        public List<IRfcServer> RfcServers { get; set; }

        public RfcConfiguration()
        {
            DefaultServer = string.Empty;
            RfcServers = new List<IRfcServer>();
        }
    }
}
