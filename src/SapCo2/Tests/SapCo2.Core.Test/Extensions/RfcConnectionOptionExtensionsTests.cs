using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Core.Extensions;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Core.Test.Extensions
{
    [TestClass]
    [TestCategory("UnitTest")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    [SuppressMessage("ReSharper", "TooManyChainedReferences")]
    public sealed class RfcConnectionOptionExtensionsTests
    {
        private static readonly Fixture Fixture = new Fixture();

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

        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void Parse_InvalidConnectionString_ShouldThrowArgumentException(string connectionString)
        {
            Action action = () => new RfcConnectionOption().Parse(connectionString);

            action.Should().Throw<ArgumentException>()
                .Which.ParamName.Should().Be("connectionString");
        }

        [TestMethod]
        public void Parse_ShouldSetProperties()
        {
            const string CONNECTION_STRING = "AppServerHost=MyFancyHost;User= SomeUsername; Password = SomePassword ";

            var parameters = new RfcConnectionOption().Parse(CONNECTION_STRING);

            parameters.Should().NotBeNull();
            parameters.AppServerHost.Should().Be("MyFancyHost");
            parameters.User.Should().Be("SomeUsername");
            parameters.Password.Should().Be("SomePassword");
        }

        [TestMethod]
        public void Parse_AllProperties()
        {
            RfcConnectionOption expectedParameters = Fixture.Create<RfcConnectionOption>();
            string connectionString = typeof(RfcConnectionOption)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Aggregate(new StringBuilder(), (sb, propertyInfo) =>
                {
                    object value = propertyInfo.GetValue(expectedParameters);
                    sb.Append($"{propertyInfo.Name}={value};");
                    return sb;
                })
                .ToString();

            var parameters = new RfcConnectionOption().Parse(connectionString);

            parameters.Should().BeEquivalentTo(expectedParameters);
        }
    }
}
