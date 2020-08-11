using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Core.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class RfcFunctionTests
    {
        private static readonly IntPtr RfcConnectionHandle = (IntPtr)12;
        private static readonly IntPtr FunctionDescriptionHandle = (IntPtr)34;
        private static readonly IntPtr FunctionHandle = (IntPtr)56;
        private static readonly Mock<IRfcConnection> ConnectionMock = new Mock<IRfcConnection>();
        private readonly Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();

        [ClassInitialize]
        public static void ClassInitializer(TestContext context)
        {
            ConnectionMock.Setup(p => p.GetConnectionHandle()).Returns(RfcConnectionHandle);
        }

        [TestMethod]
        public void CreateFunction_ShouldReturnCreatedFunction()
        {
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(),It.IsAny<string>(),  out errorInfo))
                .Returns(FunctionDescriptionHandle);
            _interopMock
                .Setup(x => x.CreateFunction(It.IsAny<IntPtr>(),  out errorInfo))
                .Returns(FunctionHandle);

            var rfcFunction=new RfcFunction(_interopMock.Object);
            
            IRfcFunction function = rfcFunction.CreateFunction(ConnectionMock.Object, "FunctonA");

            function.Should().NotBeNull();
            _interopMock.Verify(x => x.CreateFunction(FunctionDescriptionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void CreateFunction_CreationFailed_ShouldThrowException()
        {
            var errorInfo = new RfcErrorInfo { Code = RfcResultCodes.RFC_NOT_FOUND };
            _interopMock.Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo));

            var rfcFunction = new RfcFunction(_interopMock.Object);
            Action action = () => rfcFunction.CreateFunction(ConnectionMock.Object, "FunctonA");

            action.Should().Throw<RfcException>().WithMessage("SAP RFC Error: RFC_NOT_FOUND");
        }

        [TestMethod]
        public void Invoke_NoInput_NoOutput_ShouldInvokeFunction()
        {
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(), It.IsAny<string>(), out errorInfo)).Returns(FunctionDescriptionHandle);
            _interopMock.Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo)).Returns(FunctionHandle);

            var rfcFunction = new RfcFunction(_interopMock.Object);
            IRfcFunction function = rfcFunction.CreateFunction(ConnectionMock.Object, "FunctonA");

            function.Invoke();

            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Invoke_WithInput_NoOutput_ShouldMapInput()
        {
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(), It.IsAny<string>(), out errorInfo)).Returns(FunctionDescriptionHandle);
            _interopMock.Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo)).Returns(FunctionHandle);
            var rfcFunction = new RfcFunction(_interopMock.Object);
            IRfcFunction function = rfcFunction.CreateFunction(ConnectionMock.Object, "FunctonA");

            function.Invoke(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Invoke_NoInput_WithOutput_ShouldMapOutput()
        {
            int value = 456;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(), It.IsAny<string>(), out errorInfo)).Returns(FunctionDescriptionHandle);
            _interopMock.Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo)).Returns(FunctionHandle);
            _interopMock.Setup(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo));
            var rfcFunction = new RfcFunction(_interopMock.Object);
            IRfcFunction function = rfcFunction.CreateFunction(ConnectionMock.Object, "FunctonA");

            OutputModel result = function.Invoke<OutputModel>();

            result.Should().NotBeNull();
            result.Value.Should().Be(value);
            _interopMock.Verify(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_WithInput_WithOutput_ShouldMapInputAndOutput()
        {
            int value = 456;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(), It.IsAny<string>(), out errorInfo)).Returns(FunctionDescriptionHandle);
            _interopMock.Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo)).Returns(FunctionHandle);
            _interopMock.Setup(x => x.GetInt(It.IsAny<IntPtr>(), It.IsAny<string>(), out value, out errorInfo));
            var rfcFunction = new RfcFunction(_interopMock.Object);
            IRfcFunction function = rfcFunction.CreateFunction(ConnectionMock.Object, "FunctonA");

            OutputModel result = function.Invoke<OutputModel>(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldDestroyFunction()
        {
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(), It.IsAny<string>(), out errorInfo)).Returns(FunctionDescriptionHandle);
            _interopMock.Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo)).Returns(FunctionHandle);
            var rfcFunction = new RfcFunction(_interopMock.Object);
            IRfcFunction function = rfcFunction.CreateFunction(ConnectionMock.Object, "FunctonA");

            function.Dispose();

            _interopMock.Verify(x => x.DestroyFunction(FunctionHandle, out errorInfo), Times.Once);
        }

        private sealed class OutputModel
        {
            public int Value { get; set; }
        }
    }
}
