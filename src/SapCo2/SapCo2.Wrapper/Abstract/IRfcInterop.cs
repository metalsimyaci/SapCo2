using System;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Abstract
{
    /// <summary>
    /// SAP NetWeaverRFC SDK 7.50 Implementation
    /// </summary>
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

        #region Function

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

        #endregion

        #region Connection

        /// <summary>
        /// Opens an RFC client connection for invoking ABAP function modules in an R/3 backend.
        /// </summary>
        /// <param name="connectionParams">An array of RFC_CONNECTION_PARAMETERs with the names as described above and the values as necessary in your landscape</param>
        /// <param name="paramCount">Number of parameters in the above array.</param>
        /// <param name="errorInfo">Returns more error details, if the connect attempt fails.</param>
        /// <returns>A handle to an RFC client connection that can be used for invoking ABAP function modules in the backend.</returns>
        IntPtr OpenConnection(RfcConnectionParameter[] connectionParams, uint paramCount, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Closes an RFC connectionCan be used to close client connections as well as server connections, when they are no longer needed.
        /// </summary>
        /// <param name="rfcHandle">Connection to be closed</param>
        /// <param name="errorInfo">rror details in case closing the connection fails. </param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes CloseConnection(IntPtr rfcHandle, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Checks an RFC connectionCan be used to check whether a client/server connection has already been closed, or whether the NW RFC library still "considers" the connection to be open.
        /// </summary>
        /// <remarks>Note that this does not guarantee that the connection is indeed still alive: A firewall may silently have closed the connection without notifying the endpoints. If you want to find out, whether the connection is still alive, you'll have to use the more expensive RfcPing().</remarks>
        /// <param name="rfcHandle">Connection to be checked</param>
        /// <param name="isValid">1, if the connection is still found in the internal connection management, 0 otherwise.</param>
        /// <param name="errorInfo">Error details in case the connection is invalid.</param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes IsConnectionHandleValid(IntPtr rfcHandle, out int isValid, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Ping the remote communication partner through the passed connection handle.
        /// Sends a ping to the backend in order to check, whether the connection is still alive. Can be used on both, client connections as well as server connections.
        /// </summary>
        /// <remarks>Warning! Do not use inside a server function implementation.</remarks>
        /// <param name="rfcHandle">The connection to check</param>
        /// <param name="errorInfo">More error details in case the connection is broken.</param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes Ping(IntPtr rfcHandle, out RfcErrorInfo errorInfo);

        #endregion

        #region Library

        /// <summary>
        /// Get information about currently loaded Sapnwrfc library.
        /// Fills the provided unsigneds with the SAP release values, e.g. *majorVersion = 7500, *minorVersion = 0, *patchLevel = 44.
        /// </summary>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        /// <param name="patchLevel"></param>
        /// <returns>RFC_RC (Version information in string format.)</returns>
        RfcResultCodes GetVersion(out uint majorVersion, out uint minorVersion, out uint patchLevel);

        #endregion

        #region Transaction

        /// <summary>
        /// Retrieves a unique 24-digit transaction ID from the backend.
        /// If you specify NULL as connection handle, the API will attempt to generate a TID locally using the operating system's UUID algorithms. (Currently not possible on AIX systems.)
        /// </summary>
        /// <param name="rfcHandle">Client connection to a backend or NULL, if you want to create a TID locally.</param>
        /// <param name="tid">Will be filled with the transaction ID.</param>
        /// <param name="errorInfo">Error information in case there is a problem with the connection.</param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes GetTransactionId(IntPtr rfcHandle, out string tid, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Creates a container for executing a (multi-step) transactional call.
        /// <para>If queueName is NULL, tRFC will be used, otherwise qRFC. Use RfcInvokeInTransaction() to add one (or more) function modules to the transactional call. When sending this transactional call to the backend via RfcSubmitTransaction(), the backend will then treat all function modules in the RFC_TRANSACTION_HANDLE as one LUW.</para>
        /// </summary>
        /// <param name="rfcHandle">Client connection to the backend, into which you want to send this tRFC/qRFC LUW.</param>
        /// <param name="tid">A unique 24 character ID.</param>
        /// <param name="queueName">For tRFC set this to NULL, for qRFC specify the name of a qRFC inbound queue in the backend.</param>
        /// <param name="errorInfo">Error information in case there is a problem with the connection.</param>
        /// <returns>A data container that can be filled with several function modules.</returns>
        IntPtr CreateTransaction(IntPtr rfcHandle, string tid, string queueName, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Adds a function module call to a transaction.Can be used multiple times on one tHandle.
        /// </summary>
        /// <param name="tHandle">A transaction handle created via RfcCreateTransaction().</param>
        /// <param name="funcHandle">An RFC_FUNCTION_HANDLE, whose IMPORTING, CHANGING and TABLES parameters have been filled.</param>
        /// <param name="errorInfo">Actually there is nothing that can go wrong here except for invalid handles and out of memory.</param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes InvokeInTransaction(IntPtr tHandle, IntPtr funcHandle, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Executes the entire LUW in the backend system as an "atomic unit".
        /// This step can be repeated until it finally succeeds (RFC_OK).
        /// The transaction handling in the backend system protects against duplicates (until you remove the TID from the backend's status tables using RfcConfirmTransaction()).
        /// </summary>
        /// <param name="tHandle"></param>
        /// <param name="errorInfo"></param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes SubmitTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Removes the TID contained in the RFC_TRANSACTION_HANDLE from the backend's ARFCRSTATE table.
        /// After RfcSubmitTransaction() has finally succeeded, call RfcConfirmTransaction() to clean up the transaction handling table in the backend.
        /// </summary>
        /// <remarks>
        /// Attention: after this call, the backend is no longer protected against this TID. So another RfcSubmitTransaction() with the same transaction handle would result in a duplicate.
        /// You may retry the Confirm step, if you get an error here, but do not retry the Submit step!
        /// </remarks>
        /// <param name="tHandle">A transaction handle that has successfully been submitted.</param>
        /// <param name="errorInfo">Additional error information in case of a network problem.</param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes ConfirmTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo);

        /// <summary>
        /// Releases the memory of the transaction container.
        /// </summary>
        /// <param name="tHandle">A transaction handle that is no longer needed.</param>
        /// <param name="errorInfo">Not much that can go wrong here...</param>
        /// <returns>RFC_RC</returns>
        RfcResultCodes DestroyTransaction(IntPtr tHandle, out RfcErrorInfo errorInfo);

        #endregion

        #region Field Description

        IntPtr GetTypeDesc(IntPtr rfcHandle, string typeName, out RfcErrorInfo errorInfo);
        RfcResultCodes GetFieldCount(IntPtr typeDescHandle, out int count, out RfcErrorInfo errorInfo);
        RfcResultCodes GetFieldDescByIndex(IntPtr typeDescHandle, int index, out RfcFieldDescription fieldDesc,
            out RfcErrorInfo errorInfo);
        RfcResultCodes GetFieldDescByName(IntPtr typeDescHandle, string name, out RfcFieldDescription fieldDesc,
            out RfcErrorInfo errorInfo);

        #endregion

        #region Parameter Description

        RfcResultCodes GetParameterCount(IntPtr funcDescHandle, out int count, out RfcErrorInfo errorInfo);
        RfcResultCodes GetParameterDescByIndex(IntPtr funcDescHandle, int index, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo);
        RfcResultCodes GetParameterDescByName(IntPtr funcDescHandle, string name, out RfcParameterDescription paramDesc, out RfcErrorInfo errorInfo);

        #endregion
        
    }
}
