using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Core.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class RfcNetWeaverLibraryTests
    {
        private readonly Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();

        [TestMethod]
        public void EnsureLibraryPresent_ShouldSuccess()
        {
            uint major=1, minor=2, patch = 0;
            _interopMock.Setup(s => s.GetVersion(out major, out minor, out patch)).Returns(RfcResultCodes.RFC_OK);

            var library = new RfcNetWeaverLibrary(_interopMock.Object);
            library.EnsureLibraryPresent();

            library.LibraryVersion.Major.Should().Be(major);
            library.LibraryVersion.Minor.Should().Be(minor);
            library.LibraryVersion.Patch.Should().Be(patch);
            _interopMock.Verify(s=>s.GetVersion(out major,out minor,out patch),Times.Once);
        }

        [TestMethod]
        public void GetVersion_ShouldSuccess()
        {
            uint major = 1, minor = 2, patch = 0;
            _interopMock.Setup(s => s.GetVersion(out major, out minor, out patch)).Returns(RfcResultCodes.RFC_OK);
            var library = new RfcNetWeaverLibrary(_interopMock.Object);
            
            var version=library.GetVersion();

            version.Major.Should().Be(major);
            version.Minor.Should().Be(minor);
            version.Patch.Should().Be(patch);
            _interopMock.Verify(s => s.GetVersion(out major, out minor, out patch), Times.Once);
        }

        [TestMethod]
        public void GetVersion_ShouldThrowRfcLibraryNotFoundException()
        {
            uint major = 1, minor = 2, patch = 0;
            var notFoundMessage = "The SAP RFC libraries were not found in the output folder or in a folder contained in the systems .* environment variable";
            _interopMock.Setup(s => s.GetVersion(out major, out minor, out patch)).Throws(new DllNotFoundException());
            var library = new RfcNetWeaverLibrary(_interopMock.Object);

            Action action = () => library.GetVersion();
            action.Should().Throw<RfcLibraryNotFoundException>().And.Message.Should().MatchRegex(notFoundMessage);
        }

        [TestMethod]
        public void LibraryVersion_ShouldRfcNetWeaverLibrary()
        {
            uint major = 1, minor = 2, patch = 0;
            _interopMock.Setup(s => s.GetVersion(out major, out minor, out patch)).Returns(RfcResultCodes.RFC_OK);

            var library = new RfcNetWeaverLibrary(_interopMock.Object);

            library.LibraryVersion.Major.Should().Be(major);
            library.LibraryVersion.Minor.Should().Be(minor);
            library.LibraryVersion.Patch.Should().Be(patch);
        }
    }
}
