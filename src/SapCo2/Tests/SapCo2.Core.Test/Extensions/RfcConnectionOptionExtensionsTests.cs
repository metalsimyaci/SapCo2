using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Core.Extensions;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Core.Test.Extensions
{
    [TestClass]
    [TestCategory("UnitTest")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public sealed class RfcConnectionOptionExtensionsTests
    {
        [TestMethod]
        public void ToInterop_NoValueSet_ShouldReturnEmptyArray()
        {
            var option = new RfcConnectionOption();

            RfcConnectionParameter[] interopParameters = option.ToInterop();

            interopParameters.Should().BeEmpty();
        }

        [TestMethod]
        public void ToInterop_ShouldMapNonNullValues()
        {
            var option = new RfcConnectionOption
            {
                Name = "Test",
                Language = "TR",
            };

            RfcConnectionParameter[] interopParameters = option.ToInterop();

            interopParameters.Should().HaveCount(2);
            interopParameters.First().Should().BeEquivalentTo(new { Name = "NAME", Value = "Test" });
            interopParameters.Last().Should().BeEquivalentTo(new { Name = "LANG", Value = "TR" });
        }

        [TestMethod]
        public void ToInterop_ShouldUseNameFromAttribute()
        {
            var option=new RfcConnectionOption
            {
                RepositoryPassword = "SomeRepoPassword"
            };

            RfcConnectionParameter[] interopParameters = option.ToInterop();

            interopParameters.First().Name.Should().Be("REPOSITORY_PASSWD");
        }
    }
}
