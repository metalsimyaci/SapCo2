using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class RfcFunctionTests
    {
        private static readonly IntPtr RfcConnectionHandle = (IntPtr)12;
        //private static readonly IntPtr FunctionDescriptionHandle = (IntPtr)34;
        private static readonly IntPtr FunctionHandle = (IntPtr)56;
        private readonly Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();

        [ClassInitialize]
        public static void ClassInitializer(TestContext context)
        {
            //ConnectionMock.Setup(p => p.GetConnectionHandle()).Returns(RfcConnectionHandle);
        }

        [TestMethod]
        public void Invoke_NoInput_NoOutput_ShouldInvokeFunction()
        {
            RfcErrorInfo errorInfo;
            IRfcFunction function = new RfcFunction(_interopMock.Object,RfcConnectionHandle,FunctionHandle);
            function.Invoke();

            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }
        [TestMethod]
        public async Task Invoke_NoInput_NoOutput_ShouldInvokeFunctionAsync()
        {
            RfcErrorInfo errorInfo;
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);
            var result= await function.InvokeAsync();

            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Invoke_WithInput_NoOutput_ShouldMapInput()
        {
            RfcErrorInfo errorInfo;
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);

            function.Invoke(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }
        [TestMethod]
        public async Task Invoke_WithInput_NoOutput_ShouldMapInputAsync()
        {
            RfcErrorInfo errorInfo;
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);

            var result=await function.InvokeAsync(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Invoke_NoInput_WithOutput_ShouldMapOutput()
        {
            int value = 456;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo));
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);

            OutputModel result = function.Invoke<OutputModel>();

            result.Should().NotBeNull();
            result.Value.Should().Be(value);
            _interopMock.Verify(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
        }
        [TestMethod]
        public async Task Invoke_NoInput_WithOutput_ShouldMapOutputAsync()
        {
            int value = 456;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo));
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);

            OutputModel result = await function.InvokeAsync<OutputModel>();

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
            _interopMock.Setup(x => x.GetInt(It.IsAny<IntPtr>(), It.IsAny<string>(), out value, out errorInfo));
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);

            OutputModel result = function.Invoke<OutputModel>(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo), Times.Once);
        }
        [TestMethod]
        public async Task Apply_WithInput_WithOutput_ShouldMapInputAndOutputAsync()
        {
            int value = 456;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetInt(It.IsAny<IntPtr>(), It.IsAny<string>(), out value, out errorInfo));
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);

            OutputModel result =await function.InvokeAsync<OutputModel>(new { Value = 123 });

            _interopMock.Verify(x => x.SetInt(FunctionHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.GetInt(FunctionHandle, "VALUE", out value, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldDestroyFunction()
        {
            RfcErrorInfo errorInfo;
            IRfcFunction function = new RfcFunction(_interopMock.Object, RfcConnectionHandle, FunctionHandle);

            function.Dispose();

            _interopMock.Verify(x => x.DestroyFunction(FunctionHandle, out errorInfo), Times.Once);
        }

        private sealed class OutputModel
        {
            public int Value { get; set; }
        }
    }
}
