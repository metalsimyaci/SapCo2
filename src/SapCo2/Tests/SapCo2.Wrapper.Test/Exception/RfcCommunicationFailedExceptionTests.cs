using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Wrapper.Test.Exception
{
    [TestClass]
    [TestCategory("UnitTest")]
    public sealed class RfcCommunicationFailedExceptionTests
    {
        [TestMethod]
        public void Constructor_Message_ShouldSetMessageAndSetResultCode()
        {
            var exception = new RfcCommunicationFailedException("Test message");

            exception.Message.Should().Be("SAP RFC Error: RFC_COMMUNICATION_FAILURE with message: Test message");
            exception.ResultCode.Should().Be(RfcResultCodes.RFC_COMMUNICATION_FAILURE);
        }

        [TestMethod]
        public void Constructor_NullMessage_ShouldSetFixedMessageAndSetResultCode()
        {
            var exception = new RfcCommunicationFailedException( null);

            exception.Message.Should().Be("SAP RFC Error: RFC_COMMUNICATION_FAILURE");
            exception.ResultCode.Should().Be(RfcResultCodes.RFC_COMMUNICATION_FAILURE);
        }
    }
}
