using System;
using System.Runtime.InteropServices;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Interop
{
    internal sealed partial class RfcInterop
    {
        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetString(IntPtr dataHandle, string name, char[] stringBuffer,
            uint bufferLength, out uint stringLength, out RfcErrorInfo errorInfo);

        public RfcResultCodes GetString(IntPtr dataHandle, string name, char[] stringBuffer, uint bufferLength,
            out uint stringLength, out RfcErrorInfo errorInfo)
            => RfcGetString(dataHandle, name, stringBuffer, bufferLength, out stringLength, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcSetString(IntPtr dataHandle, string name, string value,
            uint valueLength, out RfcErrorInfo errorInfo);

        public RfcResultCodes SetString(IntPtr dataHandle, string name, string value, uint valueLength,
            out RfcErrorInfo errorInfo)
            => RfcSetString(dataHandle, name, value, valueLength, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetInt(IntPtr dataHandle, string name, out int value,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetInt(IntPtr dataHandle, string name, out int value, out RfcErrorInfo errorInfo)
            => RfcGetInt(dataHandle, name, out value, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcSetInt(IntPtr dataHandle, string name, int value,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes SetInt(IntPtr dataHandle, string name, int value, out RfcErrorInfo errorInfo)
            => RfcSetInt(dataHandle, name, value, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetInt8(IntPtr dataHandle, string name, out long value,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetInt8(IntPtr dataHandle, string name, out long value, out RfcErrorInfo errorInfo)
            => RfcGetInt8(dataHandle, name, out value, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcSetInt8(IntPtr dataHandle, string name, long value,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes SetInt8(IntPtr dataHandle, string name, long value, out RfcErrorInfo errorInfo)
            => RfcSetInt8(dataHandle, name, value, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetFloat(IntPtr dataHandle, string name, out double value,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetFloat(IntPtr dataHandle, string name, out double value, out RfcErrorInfo errorInfo)
            => RfcGetFloat(dataHandle, name, out value, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcSetFloat(IntPtr dataHandle, string name, double value,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes SetFloat(IntPtr dataHandle, string name, double value, out RfcErrorInfo errorInfo)
            => RfcSetFloat(dataHandle, name, value, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetDate(IntPtr dataHandle, string name, char[] emptyDate,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetDate(IntPtr dataHandle, string name, char[] emptyDate, out RfcErrorInfo errorInfo)
            => RfcGetDate(dataHandle, name, emptyDate, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcSetDate(IntPtr dataHandle, string name, char[] date,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes SetDate(IntPtr dataHandle, string name, char[] date, out RfcErrorInfo errorInfo)
            => RfcSetDate(dataHandle, name, date, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetTime(IntPtr dataHandle, string name, char[] emptyTime,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetTime(IntPtr dataHandle, string name, char[] emptyTime, out RfcErrorInfo errorInfo)
            => RfcGetTime(dataHandle, name, emptyTime, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcSetTime(IntPtr dataHandle, string name, char[] time,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes SetTime(IntPtr dataHandle, string name, char[] time, out RfcErrorInfo errorInfo)
            => RfcSetTime(dataHandle, name, time, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetStructure(IntPtr dataHandle, string name, out IntPtr structHandle,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetStructure(IntPtr dataHandle, string name, out IntPtr structHandle,
            out RfcErrorInfo errorInfo)
            => RfcGetStructure(dataHandle, name, out structHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib, CharSet = CharSet.Unicode)]
        private static extern RfcResultCodes RfcGetTable(IntPtr dataHandle, string name, out IntPtr tableHandle,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetTable(IntPtr dataHandle, string name, out IntPtr tableHandle,
            out RfcErrorInfo errorInfo)
            => RfcGetTable(dataHandle, name, out tableHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcGetRowCount(IntPtr tableHandle, out uint rowCount,
            out RfcErrorInfo errorInfo);

        public RfcResultCodes GetRowCount(IntPtr tableHandle, out uint rowCount, out RfcErrorInfo errorInfo)
            => RfcGetRowCount(tableHandle, out rowCount, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern IntPtr RfcGetCurrentRow(IntPtr tableHandle, out RfcErrorInfo errorInfo);

        public IntPtr GetCurrentRow(IntPtr tableHandle, out RfcErrorInfo errorInfo)
            => RfcGetCurrentRow(tableHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern RfcResultCodes RfcMoveToNextRow(IntPtr tableHandle, out RfcErrorInfo errorInfo);

        public RfcResultCodes MoveToNextRow(IntPtr tableHandle, out RfcErrorInfo errorInfo)
            => RfcMoveToNextRow(tableHandle, out errorInfo);

        [DllImport(NetWeaverRfcLib)]
        private static extern IntPtr RfcAppendNewRow(IntPtr tableHandle, out RfcErrorInfo errorInfo);

        public IntPtr AppendNewRow(IntPtr tableHandle, out RfcErrorInfo errorInfo)
            => RfcAppendNewRow(tableHandle, out errorInfo);
    }
}
