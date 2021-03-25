using System;
using System.Collections.Generic;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core
{
    public class RfcFunctionMetaData:IRfcFunctionMetaData
    {
        #region MyRegion

        private readonly IRfcInterop _interop;
        private readonly IntPtr _functionDescriptionHandle;

        #endregion

        #region Methods

        #region Constructor

        public RfcFunctionMetaData(IRfcInterop interop)
        {
            _interop = interop;
        }

        public RfcFunctionMetaData(IRfcInterop interop, IntPtr functionDescriptionHandle)
        {
            _interop = interop;
            _functionDescriptionHandle = functionDescriptionHandle;
        }

        #endregion

        #region #IRfcFunctionMetaData implementation

        public List<RfcParameterDescription> GetParameterDescriptions()
        {
            var parameterDescriptions = new List<RfcParameterDescription>();

            int count = GetParameterCount();
            for (int i = 0; i < count; i++)
            {
                RfcParameterDescription parameterDescription = GetParameterDescriptionByIndex(i);
                parameterDescriptions.Add(parameterDescription);
            }
            return parameterDescriptions;
        }
        public List<RfcFieldDescription> GetFieldDescriptions(IntPtr typeDescriptionHandler)
        {
            if (typeDescriptionHandler == IntPtr.Zero)
                return null;

            var fieldsDescriptions = new List<RfcFieldDescription>();

            var count = GetFieldCount(typeDescriptionHandler);
            for (int i = 0; i < count; i++)
            {
                RfcFieldDescription fieldDescription = GetFieldDescriptionByIndex(typeDescriptionHandler, i);
                fieldsDescriptions.Add(fieldDescription);
            }
            return fieldsDescriptions;
        }

        #endregion

        #region Private Methods

        private int GetParameterCount()
        {
            RfcResultCodes result = _interop.GetParameterCount(_functionDescriptionHandle, out int count, out RfcErrorInfo errorInfo);
            result.ThrowOnError(errorInfo);
            if (count <= 0)
                throw new RfcException("Function have not a any parameter");
            return count;
        }
        private RfcParameterDescription GetParameterDescriptionByIndex(int index)
        {
            RfcResultCodes result = _interop.GetParameterDescByIndex(_functionDescriptionHandle, index, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo);
            result.ThrowOnError(errorInfo);
            return paramDesc;
        }
        private RfcParameterDescription GetParameterDescriptionByName(string parameterName)
        {
            RfcResultCodes result = _interop.GetParameterDescByName(_functionDescriptionHandle, parameterName, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo);
            result.ThrowOnError(errorInfo);
            return paramDesc;
        }
        private int GetFieldCount(IntPtr typeDescriptionHandler)
        {
            RfcResultCodes result = _interop.GetFieldCount(typeDescriptionHandler, out int count, out RfcErrorInfo errorInfo);
            result.ThrowOnError(errorInfo);
            if (count <= 0)
                throw new RfcException("Parameter have not a any field");
            return count;
        }
        private RfcFieldDescription GetFieldDescriptionByIndex(IntPtr typeDescriptionHandler, int index)
        {
            RfcResultCodes result = _interop.GetFieldDescByIndex(typeDescriptionHandler, index, out RfcFieldDescription fieldDesc, out RfcErrorInfo errorInfo);
            result.ThrowOnError(errorInfo);
            return fieldDesc;
        }
        private RfcFieldDescription GetFieldDescriptionByName(IntPtr typeDescriptionHandler, string fieldName)
        {
            RfcResultCodes result = _interop.GetFieldDescByName(typeDescriptionHandler, fieldName, out RfcFieldDescription fieldDesc, out RfcErrorInfo errorInfo);
            result.ThrowOnError(errorInfo);
            return fieldDesc;
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
        }

        #endregion

        #endregion
    }
}
