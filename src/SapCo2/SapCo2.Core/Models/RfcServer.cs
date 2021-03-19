namespace SapCo2.Core.Models
{
    public class RfcServer
    {
        public string Alias { get; set; }
        public RfcConnectionPoolingOption ConnectionPooling { get; set; }
        public string ConnectionString { get; set; }
        public RfcConnectionOption ConnectionOptions { get; set; }

        public RfcServer()
        {
            Alias = string.Empty;
            ConnectionPooling = new RfcConnectionPoolingOption();
            ConnectionString = string.Empty;
            ConnectionOptions = new RfcConnectionOption();
        }
    }
}
