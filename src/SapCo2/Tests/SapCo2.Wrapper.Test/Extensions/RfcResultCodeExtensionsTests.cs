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
    public sealed class RfcResultCodeExtensionsTests
    {
        [TestMethod]
        public void ThrowOnError_NoError_ShouldNotThrow()
        {
            RfcResultCodes resultCode = RfcResultCodes.RFC_OK;
            var errorInfo = default(RfcErrorInfo);

            Action action = () => resultCode.ThrowOnError(errorInfo);

            action.Should().NotThrow();
        }

        [TestMethod]
        public void ThrowOnError_NoError_ShouldNotCallBeforeThrowAction()
        {
            RfcResultCodes resultCode = RfcResultCodes.RFC_OK;
            var errorInfo = default(RfcErrorInfo);
            var beforeThrowActionMock = new Mock<Action>();

            resultCode.ThrowOnError(errorInfo,beforeThrowActionMock.Object);

            beforeThrowActionMock.Verify(x=>x(),Times.Never);
        }

        [TestMethod]
        public void ThrowOnError_Error_ShouldCallBeforeThrowActionAndThrowRfcException()
        {
            RfcResultCodes resultCode = RfcResultCodes.RFC_CANCELED;
            var errorInfo =new RfcErrorInfo{Message = "Connection canceled"};
            var beforeThrowActionMock = new Mock<Action>();

            Action action = () => resultCode.ThrowOnError(errorInfo, beforeThrowActionMock.Object);

            action.Should().Throw<RfcException>().Which.Message.Should().Be("SAP RFC Error: RFC_CANCELED with message: Connection canceled");
            beforeThrowActionMock.Verify(x => x(), Times.Once);
        }

        [TestMethod]
        public void ThrowOnError_CommunicationFailure_ShouldThrowSapCommunicationFailedException()
        {
            RfcResultCodes resultCode = RfcResultCodes.RFC_COMMUNICATION_FAILURE;
            var errorInfo = new RfcErrorInfo { Message = "Failure error message" };

            Action action = () => resultCode.ThrowOnError(errorInfo);

            action.Should().Throw<RfcCommunicationFailedException>()
                .WithMessage("SAP RFC Error: RFC_COMMUNICATION_FAILURE with message: Failure error message");
        }

        [TestMethod]
        public void ThrowOnError_InvalidParameter_ShouldThrowSapInvalidParameterException()
        {
            RfcResultCodes resultCode = RfcResultCodes.RFC_INVALID_PARAMETER;
            var errorInfo = new RfcErrorInfo { Message = "Wrong parameter message" };

            Action action = () => resultCode.ThrowOnError(errorInfo);

            action.Should().Throw<RfcInvalidParameterException>()
                .WithMessage("SAP RFC Error: RFC_INVALID_PARAMETER with message: Wrong parameter message");
        }
    }
}
