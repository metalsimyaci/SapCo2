using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Options;
using SapCo2.Core.Abstract;
using SapCo2.Core.Extensions;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core
{
    [SuppressMessage("ReSharper", "FlagArgument")]
    public class RfcConnection : IRfcConnection
    {
        #region Variables

        private readonly IRfcInterop _interop;
        private IRfcConnectionPool _rfcConnectionPool;
        private readonly RfcConfiguration _rfcConfiguration;
        private IntPtr _rfcConnectionHandle = IntPtr.Zero;

        #endregion

        #region Properties

        public bool IsPooled { get; private set; }


        #region IRfcConnection Implementation

        public virtual bool IsValid
        {
            get
            {
                if (_rfcConnectionHandle == IntPtr.Zero)
                    return false;

                RfcResultCodes resultCode = _interop.IsConnectionHandleValid(_rfcConnectionHandle, out int isValid, out _);

                return resultCode == RfcResultCodes.RFC_OK && isValid > 0;
            }
        }

        #endregion

        #endregion


        #region Constructors

        public RfcConnection(IRfcInterop interop, IOptions<RfcConfiguration> options)
        {
            _interop = interop;
            _rfcConfiguration = options.Value;
        }

        #endregion

        #region Destructors

        ~RfcConnection()
        {
            if (IsPooled)
                Dispose();
        }

        #endregion

        #region IRfcConnection Implementation

        public bool Ping()
        {
            if (_rfcConnectionHandle == IntPtr.Zero)
                return false;

            RfcResultCodes resultCode = _interop.Ping(rfcHandle: _rfcConnectionHandle, errorInfo: out _);

            return resultCode == RfcResultCodes.RFC_OK;
        }

        public virtual void Connect()
        {
            string defaultAlias;

            if (string.IsNullOrWhiteSpace(_rfcConfiguration.DefaultServer))
            {
                if (_rfcConfiguration.RfcServers.Count == 1)
                    defaultAlias = _rfcConfiguration.RfcServers.Single().Alias;
                else
                    throw new RfcException("The default SAP server could not be detected.");
            }
            else
            {
                if (_rfcConfiguration.RfcServers.Exists(s => s.Alias == _rfcConfiguration.DefaultServer))
                    defaultAlias = _rfcConfiguration.DefaultServer;
                else
                    throw new RfcException("Default SAP server connection settings were not found.");
            }

            Connect(defaultAlias);
        }

        public virtual void Connect(string sapServerAlias)
        {
            if (!_rfcConfiguration.RfcServers.Exists(s => s.Alias == sapServerAlias))
                throw new RfcException($"SAP server connection settings not found: '{sapServerAlias}'.");

            RfcServer sapServer = _rfcConfiguration.RfcServers.Single(s => s.Alias == sapServerAlias);
            RfcConnectionParameter[] interopParameters = sapServer.ConnectionOptions.ToInterop();

            _rfcConnectionHandle = _interop.OpenConnection(interopParameters, (uint)interopParameters.Length, out RfcErrorInfo errorInfo);
            errorInfo.ThrowOnError(Clear);
        }

        public virtual void Disconnect()
        {
            Disconnect(disposing: false);
        }

        public virtual IntPtr GetConnectionHandle()
        {
            return _rfcConnectionHandle;
        }

        public IRfcFunction CreateFunction(string name)
        {
            IntPtr functionDescriptionHandle = GetFunctionDescription(name);

            return CreateFromDescriptionHandle(_interop, functionDescriptionHandle);
        }

        public IRfcFunctionMetaData CreateFunctionMetaData(string name)
        {
            IntPtr functionDescriptionHandle = GetFunctionDescription(name);
            return new RfcFunctionMetaData(_interop, functionDescriptionHandle);
        }

        public IRfcTransaction CreateTransaction()
        {
            IntPtr result = GetTransactionHandle(_interop);

            return new RfcTransaction(_interop, _rfcConnectionHandle,result);
        }

        public void SetPool(IRfcConnectionPool sapConnectionPool)
        {
            _rfcConnectionPool = sapConnectionPool;
            IsPooled = sapConnectionPool != null;
        }

        #endregion

        #region IDisposable Implementation

        public virtual void Dispose()
        {
            if (IsPooled)
                _rfcConnectionPool.ReturnConnection(this);
            else
                Disconnect(true);
        }

        #endregion

        private IntPtr GetFunctionDescription(string methodName)
        {
            IntPtr functionDescriptionHandle = _interop.GetFunctionDesc(_rfcConnectionHandle, methodName, out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError();
            
            return functionDescriptionHandle;
        }
        private IntPtr GetTransactionHandle(IRfcInterop interop)
        {
            RfcResultCodes resultCode = _interop.GetTransactionId(_rfcConnectionHandle, out string tid, out RfcErrorInfo errorInfo);
            resultCode.ThrowOnError(errorInfo);

            IntPtr result = _interop.CreateTransaction(_rfcConnectionHandle, tid, null, out errorInfo);
            resultCode.ThrowOnError(errorInfo);
            return result;
        }
        private IRfcFunction CreateFromDescriptionHandle(IRfcInterop interop, IntPtr functionDescriptionHandle)
        {
            IntPtr functionHandle = interop.CreateFunction(functionDescriptionHandle, out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError();

            return new RfcFunction(interop, _rfcConnectionHandle, functionHandle);
        }

        private void Disconnect(bool disposing)
        {
            if (_rfcConnectionHandle == IntPtr.Zero)
                return;

            RfcResultCodes resultCode = _interop.CloseConnection(rfcHandle: _rfcConnectionHandle, errorInfo: out RfcErrorInfo errorInfo);

            Clear();

            if (!disposing)
                resultCode.ThrowOnError(errorInfo);
        }

        private void Clear()
        {
            _rfcConnectionHandle = IntPtr.Zero;
        }

    }
}
