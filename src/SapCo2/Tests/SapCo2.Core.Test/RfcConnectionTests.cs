using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public sealed class RfcConnectionTests
    {
        private static readonly IntPtr RfcConnectionHandle = (IntPtr)12;
        private readonly Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();
        private RfcConfiguration _rfcConfiguration;
        private IOptions<RfcConfiguration> _rfcConfigurationOption;

        [TestInitialize]
        public void Initializer()
        {
            _rfcConfiguration = new RfcConfiguration()
            {
                DefaultServer = "TEST",
                RfcServers = new List<RfcServer>
                {
                    new RfcServer()
                    {
                        Alias = "TEST",
                        ConnectionString =
                            "Name:TEST;AppServerHost=test.domain.com;User=username;Password=p4ssw00rd;SystemId:xxx;SystemNumber=00; Client=100; Language=TR; PoolSize=50;MaxPoolSize:100;IdleTimeout:600;Trace=0",
                        ConnectionPooling =
                        {
                            Enabled = false, PoolSize = 0, IdleTimeout = TimeSpan.FromSeconds(30), IdleDetectionInterval = TimeSpan.FromSeconds(1)
                        }
                    },
                    new RfcServer()
                    {
                        Alias = "TEST2",
                        ConnectionOptions = new RfcConnectionOption()
                        {
                            Name = "TEST2",
                            AppServerHost = "test.domain.com",
                            User = "username",
                            Password = "P4ssw00rd",
                            SystemId = "xxx",
                            SystemNumber = "00",
                            Client = "100",
                            Language = "TR",
                            PoolSize = "50",
                            IdleTimeout = "600",
                            Trace = "0"
                        },
                        ConnectionPooling =
                        {
                            Enabled = false,
                            PoolSize = 0,
                            IdleTimeout = TimeSpan.FromSeconds(30),
                            IdleDetectionInterval = TimeSpan.FromSeconds(1)
                        }
                    }
                }
            };
            _rfcConfigurationOption = Options.Create(_rfcConfiguration);
        }

        [TestMethod]
        public void Connect_ConnectionSucceeds_ShouldOpenConnection()
        {
            var alias =_rfcConfiguration.DefaultServer;
            var configuration = _rfcConfiguration.RfcServers.First(x => x.Alias == alias);
            uint counter = GetRfcConnectionOptionPropertyCount(configuration.ConnectionOptions);
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            connection.Connect();

            RfcErrorInfo errorInfo;
            _interopMock.Verify(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), counter, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Connect_ConnectionSucceeds_ShouldOpenConnectionWithAlias()
        {
            var alias = "TEST2";
            var configuration = _rfcConfiguration.RfcServers.First(x => x.Alias == alias);
            uint counter = GetRfcConnectionOptionPropertyCount(configuration.ConnectionOptions);

            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            connection.Connect(alias);

            RfcErrorInfo errorInfo;
            _interopMock.Verify(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), counter, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Connect_ConnectionFailed_ShouldThrow()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            var errorInfo = new RfcErrorInfo { Code = RfcResultCodes.RFC_TIMEOUT };
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo));

            Action action = () => connection.Connect();

            action.Should().Throw<RfcException>().WithMessage("SAP RFC Error: RFC_TIMEOUT");
        }

        [TestMethod]
        public void Disconnect_NotConnected_ShouldNotDisconnect()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);

            connection.Disconnect();

            RfcErrorInfo errorInfo;
            _interopMock.Verify(x => x.CloseConnection(It.IsAny<IntPtr>(), out errorInfo), Times.Never);
        }

        [TestMethod]
        public void Disconnect_Connected_ShouldDisconnect()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);

            connection.Connect();

            connection.Disconnect();

            _interopMock.Verify(x => x.CloseConnection(RfcConnectionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Disconnect_DisconnectionFailed_ShouldThrowException()
        {
            // Arrange
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            _interopMock
                .Setup(x => x.CloseConnection(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(RfcResultCodes.RFC_CANCELED);

            connection.Connect();

            // Act
            Action action = () => connection.Disconnect();

            // Assert
            action.Should().Throw<RfcException>()
                .WithMessage("SAP RFC Error: RFC_CANCELED");
        }

        [TestMethod]
        public void Dispose_ShouldDisconnect()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);

            connection.Connect();

            connection.Dispose();

            _interopMock.Verify(x => x.CloseConnection(RfcConnectionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Dispose_DisconnectionFailed_ShouldNotThrow()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            _interopMock
                .Setup(x => x.CloseConnection(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(RfcResultCodes.RFC_CANCELED);

            connection.Connect();

            Action action = () => connection.Dispose();

            action.Should().NotThrow();
        }

        [TestMethod]
        public void IsConnected_Connected_ShouldReturnTrue()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            int isValid = 1;
            _interopMock
                .Setup(x => x.IsConnectionHandleValid(RfcConnectionHandle, out isValid, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            connection.Connect();

            bool isConnected = connection.IsValid;

            isConnected.Should().BeTrue();
        }

        [TestMethod]
        public void IsConnected_Disconnected_ShouldReturnFalse()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            int isValid = 1;
            _interopMock
                .Setup(x => x.IsConnectionHandleValid(RfcConnectionHandle, out isValid, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            connection.Connect();
            connection.Disconnect();

            bool isConnected = connection.IsValid;

            isConnected.Should().BeFalse();
        }

        [TestMethod]
        public void IsConnected_ConnectedButLibraryReturnsError_ShouldReturnFalse()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            int isValid = 1;
            _interopMock
                .Setup(x => x.IsConnectionHandleValid(RfcConnectionHandle, out isValid, out errorInfo))
                .Returns(RfcResultCodes.RFC_CLOSED);

            connection.Connect();

            bool isConnected = connection.IsValid;

            isConnected.Should().BeFalse();
        }

        [TestMethod]
        public void IsConnected_ConnectedButLibrarySaysWereDisconnected_ShouldReturnFalse()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            int isValid = 0;
            _interopMock
                .Setup(x => x.IsConnectionHandleValid(RfcConnectionHandle, out isValid, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            connection.Connect();

            bool isConnected = connection.IsValid;

            isConnected.Should().BeFalse();
        }

        [TestMethod]
        public void IsValid_ConnectionHandleValid_ShouldReturnTrue()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            int isValidValue = 1;
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            _interopMock
                .Setup(x => x.IsConnectionHandleValid(RfcConnectionHandle, out isValidValue, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            connection.Connect();

            var isValid = connection.IsValid;

            isValid.Should().BeTrue();
        }

        [TestMethod]
        public void IsValid_ConnectionHandleInvalid_ShouldReturnFalse()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            int isValidValue = 0;
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            _interopMock
                .Setup(x => x.IsConnectionHandleValid(RfcConnectionHandle, out isValidValue, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            connection.Connect();

            var isValid = connection.IsValid;

            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void Ping_NotConnected_ShouldReturnFalse()
        {
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);

            var pingResult = connection.Ping();

            pingResult.Should().BeFalse();
        }

        [TestMethod]
        public void Ping_Connected_SuccessfulPing_ShouldReturnTrue()
        {
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            _interopMock
                .Setup(x => x.Ping(RfcConnectionHandle, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            connection.Connect();

            var pingResult = connection.Ping();

            pingResult.Should().BeTrue();
        }

        [TestMethod]
        public void Ping_Connected_PingTimeout_ShouldReturnFalse()
        {
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.OpenConnection(It.IsAny<RfcConnectionParameter[]>(), It.IsAny<uint>(), out errorInfo))
                .Returns(RfcConnectionHandle);
            _interopMock
                .Setup(x => x.Ping(RfcConnectionHandle, out errorInfo))
                .Returns(RfcResultCodes.RFC_TIMEOUT);
            var connection = new RfcConnection(_interopMock.Object, _rfcConfigurationOption);
            connection.Connect();

            var pingResult = connection.Ping();

            pingResult.Should().BeFalse();
        }

        private uint GetRfcConnectionOptionPropertyCount(RfcConnectionOption connectionOptions)
        {
            PropertyInfo[] properties = connectionOptions.GetType().GetProperties();
            uint counter = 0;
            foreach (PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(connectionOptions, null);
                if (value != null)
                    counter++;
            }

            return counter;
        }
    }
}
