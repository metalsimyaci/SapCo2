using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Core.Abstract;
using SapCo2.Core.Models;

namespace SapCo2.Core
{
    public class RfcConnectionPool:IRfcConnectionPool
    {
        #region Variables

        private readonly IServiceProvider _serviceProvider;
        private readonly RfcServer _rfcServer;
        private readonly object _syncRoot = new object();
        private readonly ConcurrentQueue<(IRfcConnection Connection, DateTime ExpiresAtUtc)> _idleConnections = new ConcurrentQueue<(IRfcConnection Connection, DateTime ExpiresAtUtc)>();
        private readonly SemaphoreSlim _idleConnectionSemaphore = new SemaphoreSlim(0);
        private readonly Timer _idleDetectionTimer;
        private int _openConnectionCount;

        #endregion

        #region Properties

        #region ISapConnectionPool Implementation

        public string ServerAlias { get; set; }

        #endregion

        #endregion

        public RfcConnectionPool(IServiceProvider serviceProvider, string sapServerAlias, RfcConfiguration rfcConfiguration)
        {
            _serviceProvider = serviceProvider;
            ServerAlias = sapServerAlias;
            //_sapConfiguration = sapConfiguration;
            _rfcServer = rfcConfiguration.RfcServers.Single(s => s.Alias == sapServerAlias);
            _idleDetectionTimer = new Timer(
                callback: _ => DisposeIdleConnections(),
                state: null,
                dueTime: Convert.ToInt32(_rfcServer.ConnectionPooling.IdleDetectionInterval.TotalMilliseconds),
                period: Convert.ToInt32(_rfcServer.ConnectionPooling.IdleDetectionInterval.TotalMilliseconds));
        }
        

        public IRfcConnection GetConnection()
        {
            if (_idleConnectionSemaphore.Wait(TimeSpan.Zero))
            {
                lock (_syncRoot)
                    if (_idleConnections.TryDequeue(out (IRfcConnection Connection, DateTime ExpiresAtUtc) idleConnection))
                        return idleConnection.Connection;
            }

            while (true)
            {
                if (_openConnectionCount < _rfcServer.ConnectionPooling.PoolSize)
                {
                    IRfcConnection connection = null;

                    lock (_syncRoot)
                    {
                        if (_openConnectionCount < _rfcServer.ConnectionPooling.PoolSize)
                        {
                            _openConnectionCount++;
                            connection = _serviceProvider.GetRequiredService<IRfcConnection>();
                            connection.SetPool(this);
                        }
                    }

                    if (connection != null)
                    {
                        connection.Connect(ServerAlias);
                        return connection;
                    }
                }

                _idleConnectionSemaphore.Wait();

                lock (_syncRoot)
                    if (_idleConnections.TryDequeue(out (IRfcConnection Connection, DateTime ExpiresAtUtc) idleConnection))
                        return idleConnection.Connection;
            }
        }
        public void ReturnConnection(IRfcConnection connection)
        {
            DateTime expiresAtUtc = DateTime.UtcNow + _rfcServer.ConnectionPooling.IdleTimeout;

            _idleConnections.Enqueue((connection, expiresAtUtc));
            _idleConnectionSemaphore.Release();
        }
        public void ForgetConnection(IRfcConnection connection)
        {
            connection.Dispose();

            lock (_syncRoot)
                _openConnectionCount--;

            _idleConnectionSemaphore.Release();
        }
        private void DisposeIdleConnections()
        {
            while (true)
            {
                if (!_idleConnections.TryPeek(out (IRfcConnection Connection, DateTime ExpiresAtUtc) idleConnection) || idleConnection.ExpiresAtUtc > DateTime.UtcNow)
                    break;

                lock (_syncRoot)
                {
                    if (!_idleConnections.TryPeek(out idleConnection) || idleConnection.ExpiresAtUtc > DateTime.UtcNow)
                        break;

                    _idleConnections.TryDequeue(out idleConnection);
                    _idleConnectionSemaphore.Wait();
                    idleConnection.Connection.SetPool(null);
                    idleConnection.Connection.Dispose();

                    Debug.Assert(_openConnectionCount > 0, "Open connection count must be greater than zero");
                    _openConnectionCount--;
                }
            }
        }
        public void Dispose()
        {
            _idleDetectionTimer.Dispose();

            while (_idleConnections.TryDequeue(out (IRfcConnection Connection, DateTime ExpiresAtUtc) idleConnection))
                idleConnection.Connection.Dispose();
        }
    }
}
