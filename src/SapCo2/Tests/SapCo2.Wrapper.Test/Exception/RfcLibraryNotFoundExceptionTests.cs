using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Wrapper.Test.Exception
{
    [TestClass]
    [TestCategory("UnitTest")]
    public sealed class RfcLibraryNotFoundExceptionTests
    {
        [TestMethod]
        public void Constructor_ShouldSetInnerException()
        {
            var innerException = new DllNotFoundException();

            var exception = new RfcLibraryNotFoundException(innerException);

            exception.InnerException.Should().Be(innerException);
        }

        [TestMethod]
        public void Constructor_ShouldSetMessage()
        {
            var exception = new RfcLibraryNotFoundException(innerException: default);

            exception.Message.Should().MatchRegex("The SAP RFC libraries were not found in the output folder or in a folder contained in the systems .* environment variable");
        }
    }
}
