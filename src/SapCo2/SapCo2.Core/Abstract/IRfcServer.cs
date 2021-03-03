using SapCo2.Core.Models;

namespace SapCo2.Core.Abstract
{
    public interface IRfcServer
    {
        string Alias { get; set; }
        RfcConnectionPoolingOption ConnectionPooling { get; set; }
        string ConnectionString { get; set; }
        RfcConnectionOption ConnectionOptions { get; set; }
    }
}
