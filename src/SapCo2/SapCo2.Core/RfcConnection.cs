using System;
using SapCo2.Core.Abstract;
using SapCo2.Core.Models;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Extension;
namespace SapCo2.Core
{
    public class RfcConnection:IRfcConnection
    {
        private readonly IRfcInterop _interop;
        private readonly ConnectionParameter _parameters;
        private IntPtr _rfcConnectionHandle = IntPtr.Zero;


        /// <summary>
        /// Initializes a new instance of the <see cref="RfcConnection"/> class with the given connection parameters.
        /// </summary>
        /// <param name="parameters">The connection parameters.</param>
        public RfcConnection(ConnectionParameter parameters, IRfcInterop interop)
            : this(interop, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RfcConnection"/> class with the given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public RfcConnection(string connectionString, IRfcInterop interop)
            : this(interop, ConnectionParameter.Parse(connectionString))
        {
        }

        // Constructor for unit-testing
        internal RfcConnection(IRfcInterop interop, ConnectionParameter parameters)
        {
            _interop = interop;
            _parameters = parameters;
        }

        /// <inheritdoc cref="IRfcConnection"/>
        public void Dispose()
        {
            Disconnect(disposing: true);
        }

        /// <inheritdoc cref="IRfcConnection"/>
        public void Connect()
        {
            RfcConnectionParameter[] interopParameters = _parameters.ToInterop();

            _rfcConnectionHandle = _interop.OpenConnection(
                connectionParams: interopParameters,
                paramCount: (uint)interopParameters.Length,
                errorInfo: out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError(beforeThrow: Clear);
        }

        /// <inheritdoc cref="IRfcConnection"/>
        public void Disconnect()
        {
            Disconnect(disposing: false);
        }
        private void Disconnect(bool disposing)
        {
            if (_rfcConnectionHandle == IntPtr.Zero)
                return;

            RfcResultCodes resultCode = _interop.CloseConnection(
                rfcHandle: _rfcConnectionHandle,
                errorInfo: out RfcErrorInfo errorInfo);

            Clear();

            if (!disposing)
                resultCode.ThrowOnError(errorInfo);
        }

        /// <inheritdoc cref="IRfcConnection"/>
        public bool IsValid
        {
            get
            {
                if (_rfcConnectionHandle == IntPtr.Zero)
                    return false;

                RfcResultCodes resultCode = _interop.IsConnectionHandleValid(_rfcConnectionHandle, out int isValid, out _);
                return resultCode == RfcResultCodes.RFC_OK && isValid > 0;
            }
        }

        /// <inheritdoc cref="IRfcConnection"/>
        public bool Ping()
        {
            if (_rfcConnectionHandle == IntPtr.Zero)
                return false;

            RfcResultCodes resultCode = _interop.Ping(
                rfcHandle: _rfcConnectionHandle,
                errorInfo: out _);

            return resultCode == RfcResultCodes.RFC_OK;
        }

        /// <inheritdoc cref="IRfcConnection"/>
        public IRfcFunction CreateFunction(string name)
        {
            IntPtr functionDescriptionHandle = _interop.GetFunctionDesc(
                rfcHandle: _rfcConnectionHandle,
                funcName: name,
                errorInfo: out RfcErrorInfo errorInfo);

            errorInfo.ThrowOnError();

            return RfcFunction.CreateFromDescriptionHandle(
                interop: _interop,
                rfcConnectionHandle: _rfcConnectionHandle,
                functionDescriptionHandle: functionDescriptionHandle);
        }

        private void Clear()
        {
            _rfcConnectionHandle = IntPtr.Zero;
        }
    }
}
