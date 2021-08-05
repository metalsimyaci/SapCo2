using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class RfcTransactionTests
    {
        private static readonly IntPtr RfcConnectionHandle = (IntPtr)12;
        private static readonly IntPtr FunctionDescriptionHandle = (IntPtr)20;
        private static readonly IntPtr FunctionHandle = (IntPtr)32;
        private static readonly IntPtr TransactionHandle = (IntPtr)56;
        private readonly Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();

        [TestMethod]
        public void CreateFunction_functionName_ShouldReturnIRfcTransactionFunction()
        {
            RfcErrorInfo errorInfo;
            var functionName = "TEST";
            var transaction = new RfcTransaction(_interopMock.Object, RfcConnectionHandle, TransactionHandle);

            _interopMock
                .Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(), It.IsAny<string>(), out errorInfo))
                .Returns(FunctionDescriptionHandle);

            _interopMock
                .Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(FunctionHandle);

            IRfcTransactionFunction function = transaction.CreateFunction(functionName);

            function.Should().NotBeNull();
        }

        [TestMethod]
        public void SubmitTransaction_ShouldSuccess()
        {
            RfcErrorInfo errorInfo;
            var transaction = new RfcTransaction(_interopMock.Object, RfcConnectionHandle, TransactionHandle);

            _interopMock
                .Setup(x => x.SubmitTransaction(It.IsAny<IntPtr>(),  out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            transaction.SubmitTransaction();

            _interopMock.Verify(x => x.SubmitTransaction(TransactionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void ConfirmTransaction_ShouldSuccess()
        {
            RfcErrorInfo errorInfo;
            var transaction = new RfcTransaction(_interopMock.Object, RfcConnectionHandle, TransactionHandle);

            _interopMock
                .Setup(x => x.ConfirmTransaction(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            transaction.ConfirmTransaction();

            _interopMock.Verify(x => x.ConfirmTransaction(TransactionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldDisconnectFunction()
        {
            RfcErrorInfo errorInfo;
            var transaction = new RfcTransaction(_interopMock.Object, RfcConnectionHandle, TransactionHandle);
            _interopMock
                .Setup(x => x.DestroyTransaction(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            transaction.Dispose();

            _interopMock.Verify(x => x.DestroyTransaction(TransactionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldNotRfcConnectionHandler()
        {
            RfcErrorInfo errorInfo;
            var transaction = new RfcTransaction(_interopMock.Object, RfcConnectionHandle, TransactionHandle);
            _interopMock
                .Setup(x => x.DestroyTransaction(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            transaction.Dispose();

            _interopMock.Verify(x => x.DestroyTransaction(TransactionHandle, out errorInfo), Times.Never);
        }

    }
}
