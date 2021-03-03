using System;

namespace SapCo2.Core.Models
{
    public class RfcConnectionPoolingOption
    {
        private const int DEFAULT_POOL_SIZE = 16;
        private const int DEFAULT_CONNECTION_IDLE_TIMEOUT_IN_SECONDS = 30;
        private const int DEFAULT_IDLE_DETECTION_INTERVAL_IN_SECONDS = 1;

        public bool Enabled { get; set; }
        public int PoolSize { get; set; }
        public TimeSpan IdleTimeout { get; set; }
        public TimeSpan IdleDetectionInterval { get; set; }

        public RfcConnectionPoolingOption()
        {
            Enabled = default;
            PoolSize = DEFAULT_POOL_SIZE;
            IdleTimeout = TimeSpan.FromSeconds(DEFAULT_CONNECTION_IDLE_TIMEOUT_IN_SECONDS);
            IdleDetectionInterval = TimeSpan.FromSeconds(DEFAULT_IDLE_DETECTION_INTERVAL_IN_SECONDS);
        }
    }
}
