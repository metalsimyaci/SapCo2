using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Interop
{
    internal sealed partial class RfcInterop
    {
        #region Metadata For structures/line-tipes

        [DllImport(NetWeaverRfcLib)]
        private static extern IntPtr RfcGetTypeDesc(IntPtr rfcHandle, string typeName, out RfcErrorInfo errorInfo);

        public IntPtr GetTypeDesc(IntPtr rfcHandle, string typeName, out RfcErrorInfo errorInfo)
            => RfcGetTypeDesc(rfcHandle, typeName, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetFieldCount(IntPtr typeDescHandle, out int count, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetFieldCount(IntPtr typeDescHandle, out int count, out RfcErrorInfo errorInfo)
            => RfcGetFieldCount(typeDescHandle, out count, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetFieldDescByIndex(IntPtr typeDescHandle, int index, out RfcFieldDescription fieldDesc, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetFieldDescByIndex(IntPtr typeDescHandle, int index, out RfcFieldDescription fieldDesc,
            out RfcErrorInfo errorInfo)
            => RfcGetFieldDescByIndex(typeDescHandle, index, out fieldDesc, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetFieldDescByName(IntPtr typeDescHandle, string name, out RfcFieldDescription fieldDesc, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetFieldDescByName(IntPtr typeDescHandle, string name, out RfcFieldDescription fieldDesc,
            out RfcErrorInfo errorInfo)
            => RfcGetFieldDescByName(typeDescHandle, name, out fieldDesc, out errorInfo);

        #endregion

        #region Metadata For function

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetParameterCount(IntPtr funcDescHandle, out int count, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetParameterCount(IntPtr funcDescHandle, out int count, out RfcErrorInfo errorInfo)
            => RfcGetParameterCount(funcDescHandle, out count, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetParameterDescByIndex(IntPtr funcDescHandle, int index, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetParameterDescByIndex(IntPtr funcDescHandle, int index, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo)
            => RfcGetParameterDescByIndex(funcDescHandle, index, out paramDesc, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetParameterDescByName(IntPtr funcDescHandle, string name, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetParameterDescByName(IntPtr funcDescHandle, string name, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo)
            => RfcGetParameterDescByName(funcDescHandle, name, out paramDesc, out errorInfo);

        #endregion


        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern IntPtr RfcGetFunctionDesc(IntPtr rfcHandle, string funcName, out RfcErrorInfo errorInfo);

        public IntPtr GetFunctionDesc(IntPtr rfcHandle, string funcName, out RfcErrorInfo errorInfo)
            => RfcGetFunctionDesc(rfcHandle, funcName, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern IntPtr RfcCreateFunction(IntPtr funcDescHandle, out RfcErrorInfo errorInfo);

        public IntPtr CreateFunction(IntPtr funcDescHandle, out RfcErrorInfo errorInfo)
            => RfcCreateFunction(funcDescHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcDestroyFunction(IntPtr funcHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes DestroyFunction(IntPtr funcHandle, out RfcErrorInfo errorInfo)
            => RfcDestroyFunction(funcHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcInvoke(IntPtr rfcHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes Invoke(IntPtr rfcHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo)
            => RfcInvoke(rfcHandle, funcHandle, out errorInfo);

        

    }
}
