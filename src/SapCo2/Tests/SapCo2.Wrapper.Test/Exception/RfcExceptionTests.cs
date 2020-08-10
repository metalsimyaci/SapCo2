using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Wrapper.Test.Exception
{
    [TestClass]
    [TestCategory("UnitTest")]
    public sealed class RfcExceptionTests
    {
        [TestMethod]
        public void ShouldInheritFromException()
        {
            typeof(System.Exception).IsAssignableFrom(typeof(RfcException)).Should().BeTrue();
        }

        [TestMethod]
        public void Constructor_MessageOnly_ShouldSetMessageAndSetResultCodeToUnknownError()
        {
            var exception = new RfcException("Test message");

            exception.Message.Should().Be("SAP RFC Error with message: Test message");
            exception.ResultCode.Should().Be(RfcResultCodes.RFC_UNKNOWN_ERROR);
        }

        [TestMethod]
        public void Constructor_NullMessage_ShouldSetFixedMessageAndSetResultCodeToUnknownError()
        {
            var exception = new RfcException(null);

            exception.Message.Should().Be("SAP RFC Error");
            exception.ResultCode.Should().Be(RfcResultCodes.RFC_UNKNOWN_ERROR);
        }

        [TestMethod]
        public void Constructor_MessageAndCode_ShouldSetMessageAndSetResultCode()
        {
            var exception = new RfcException(RfcResultCodes.RFC_ABAP_EXCEPTION, "Test message");

            exception.Message.Should().Be("SAP RFC Error: RFC_ABAP_EXCEPTION with message: Test message");
            exception.ResultCode.Should().Be(RfcResultCodes.RFC_ABAP_EXCEPTION);
        }

        [TestMethod]
        public void Constructor_NullMessageAndCode_ShouldSetFixedMessageAndSetResultCode()
        {
            var exception = new RfcException(RfcResultCodes.RFC_CANCELED, null);

            exception.Message.Should().Be("SAP RFC Error: RFC_CANCELED");
            exception.ResultCode.Should().Be(RfcResultCodes.RFC_CANCELED);
        }

    }
}
