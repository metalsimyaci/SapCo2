using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Test.Extensions
{
    [TestClass]
    [TestCategory("UnitTest")]
    public sealed class RfcErrorInfoExtensionsTests
    {
        [TestMethod]
        public void ThrowOnError_NoError_ShouldNotThrow()
        {
            var errorInfo = new RfcErrorInfo { Code = RfcResultCodes.RFC_OK };

            Action action = () => errorInfo.ThrowOnError();

            action.Should().NotThrow();
        }

        [TestMethod]
        public void ThrowOnError_NoError_ShouldNotCallBeforeThrowAction()
        {
            var errorInfo = new RfcErrorInfo { Code = RfcResultCodes.RFC_OK };
            var beforeThrowActionMock = new Mock<Action>();

            errorInfo.ThrowOnError(beforeThrowActionMock.Object);

            beforeThrowActionMock.Verify(x=>x(),Times.Never);
        }

        [TestMethod]
        public void ThrowOnError_Error_ShouldCallBeforeThrowActionAndThrowRfcException()
        {
            var errorInfo = new RfcErrorInfo {Code = RfcResultCodes.RFC_CANCELED, Message = "Connection cancelled"};
            var beforeThrowActionMock = new Mock<Action>();

            Action action = () => errorInfo.ThrowOnError(beforeThrowActionMock.Object);

            action.Should().Throw<RfcException>().Which.Message.Should().Be("SAP RFC Error: RFC_CANCELED with message: Connection cancelled");
            beforeThrowActionMock.Verify(x=>x(),Times.Once);
        }
    }
}
