using System;
using System.Threading.Tasks;
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
    public class RfcTransactionFunctionTests
    {
        private static readonly IntPtr RfcTransactionHandle = (IntPtr)56;
        private static readonly IntPtr FunctionHandle = (IntPtr)56;
        private readonly Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();

       
        [TestInitialize]
        public void Initializer()
        {
            //ConnectionMock.Setup(p => p.GetConnectionHandle()).Returns(RfcConnectionHandle);
        }


        [TestMethod]
        public void Invoke_NoInput_ShouldInvokeInTransactionFunction()
        {
            RfcErrorInfo errorInfo;
            IRfcTransactionFunction function = new RfcTransactionFunction(_interopMock.Object, RfcTransactionHandle, FunctionHandle);
            function.Invoke();

            _interopMock.Verify(x => x.InvokeInTransaction(RfcTransactionHandle, FunctionHandle, out errorInfo), Times.Once);
        }
        [TestMethod]
        public async Task Invoke_NoInput_ShouldInvokeInTransactionFunctionAsync()
        {
            RfcErrorInfo errorInfo;
            IRfcTransactionFunction function = new RfcTransactionFunction(_interopMock.Object, RfcTransactionHandle, FunctionHandle);
            var result = await function.InvokeAsync();

            _interopMock.Verify(x => x.InvokeInTransaction(RfcTransactionHandle, FunctionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Invoke_WithInput_ShouldMapInput()
        {
            RfcErrorInfo errorInfo;
            IRfcTransactionFunction function = new RfcTransactionFunction(_interopMock.Object, RfcTransactionHandle, FunctionHandle);

            function.Invoke(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.InvokeInTransaction(RfcTransactionHandle, FunctionHandle, out errorInfo), Times.Once);
        }
        [TestMethod]
        public async Task Invoke_WithInput_ShouldMapInputAsync()
        {
            RfcErrorInfo errorInfo;
            IRfcTransactionFunction function = new RfcTransactionFunction(_interopMock.Object, RfcTransactionHandle, FunctionHandle);

            var result = await function.InvokeAsync(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.InvokeInTransaction(RfcTransactionHandle, FunctionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void ReadSubmitResult_WithOutput_ShouldMapOutput()
        {
            int value = 456;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo));
            IRfcTransactionFunction function = new RfcTransactionFunction(_interopMock.Object, RfcTransactionHandle, FunctionHandle);

            OutputModel result = function.ReadSubmitResult<OutputModel>();

            result.Should().NotBeNull();
            result.Value.Should().Be(value);
            _interopMock.Verify(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldDestroyFunction()
        {
            RfcErrorInfo errorInfo;
            IRfcTransactionFunction function = new RfcTransactionFunction(_interopMock.Object, RfcTransactionHandle, FunctionHandle);
            _interopMock.Setup(x => x.DestroyFunction(It.IsAny<IntPtr>(), out errorInfo)).Returns(RfcResultCodes.RFC_OK);
            function.Dispose();

            _interopMock.Verify(x => x.DestroyFunction(FunctionHandle, out errorInfo), Times.Once);
        }

        private sealed class OutputModel
        {
            public int Value { get; set; }
        }
    }
}
