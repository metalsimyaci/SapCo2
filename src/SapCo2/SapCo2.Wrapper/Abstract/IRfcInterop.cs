using System;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Abstract
{
    public interface IRfcInterop
    {
        #region Fields
            
        RfcResultCodes GetString(IntPtr dataHandle, string name, char[] stringBuffer, uint bufferLength, out uint stringLength, 
            out RfcErrorInfo errorInfo);
        RfcResultCodes SetString(IntPtr dataHandle, string name, string value, uint valueLength, out RfcErrorInfo errorInfo);
        RfcResultCodes GetInt(IntPtr dataHandle, string name, out int value, out RfcErrorInfo errorInfo);
        RfcResultCodes SetInt(IntPtr dataHandle, string name, int value, out RfcErrorInfo errorInfo);
        RfcResultCodes GetInt8(IntPtr dataHandle, string name, out long value, out RfcErrorInfo errorInfo);
        RfcResultCodes SetInt8(IntPtr dataHandle, string name, long value, out RfcErrorInfo errorInfo);
        RfcResultCodes GetFloat(IntPtr dataHandle, string name, out double value, out RfcErrorInfo errorInfo);
        RfcResultCodes SetFloat(IntPtr dataHandle, string name, double value, out RfcErrorInfo errorInfo);
        RfcResultCodes GetDate(IntPtr dataHandle, string name, char[] emptyDate, out RfcErrorInfo errorInfo);
        RfcResultCodes SetDate(IntPtr dataHandle, string name, char[] date, out RfcErrorInfo errorInfo);
        RfcResultCodes GetTime(IntPtr dataHandle, string name, char[] emptyTime, out RfcErrorInfo errorInfo);
        RfcResultCodes SetTime(IntPtr dataHandle, string name, char[] time, out RfcErrorInfo errorInfo);
        
        #endregion

        RfcResultCodes GetStructure(IntPtr dataHandle, string name, out IntPtr structHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes GetTable(IntPtr dataHandle, string name, out IntPtr tableHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes GetRowCount(IntPtr tableHandle, out uint rowCount, out RfcErrorInfo errorInfo);
        IntPtr GetCurrentRow(IntPtr tableHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes MoveToNextRow(IntPtr tableHandle, out RfcErrorInfo errorInfo);
        IntPtr AppendNewRow(IntPtr tableHandle, out RfcErrorInfo errorInfo);
        IntPtr GetFunctionDesc(IntPtr rfcHandle, string funcName, out RfcErrorInfo errorInfo);
        IntPtr CreateFunction(IntPtr funcDescHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes DestroyFunction(IntPtr funcHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes Invoke(IntPtr rfcHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo);

        IntPtr OpenConnection(RfcConnectionParameter[] connectionParams, uint paramCount, out RfcErrorInfo errorInfo);
        RfcResultCodes CloseConnection(IntPtr rfcHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes IsConnectionHandleValid(IntPtr rfcHandle, out int isValid, out RfcErrorInfo errorInfo);
        RfcResultCodes Ping(IntPtr rfcHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

        #region Transaction

        RfcResultCodes GetTransactionId(IntPtr rfcHandle, out string tid, out RfcErrorInfo errorInfo);
        IntPtr CreateTransaction(IntPtr rfcHandle, string tid, string queueName, out RfcErrorInfo errorInfo);
        RfcResultCodes InvokeInTransaction(IntPtr rfcTransactionHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes SubmitTransaction(IntPtr rfcTransactionHandle, out RfcErrorInfo errorInfo);
        RfcResultCodes ConfirmTransaction(IntPtr rfcTransactionHandle, out RfcErrorInfo errorInfo);

        #endregion
    }
}
