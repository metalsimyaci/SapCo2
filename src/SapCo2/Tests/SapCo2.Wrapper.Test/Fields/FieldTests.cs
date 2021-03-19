using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapCo2.Wrapper.Abstract;

namespace SapCo2.Wrapper.Test.Fields
{
    [TestClass]
    [TestCategory("UnitTest")]
    public sealed class FieldTests
    {
        [TestMethod]
        public void Equals_SameNameAndValue_ShouldReturnTrue()
        {
            // Arrange
            var fieldA = new TestField("abc", true);
            var fieldB = new TestField("abc", true);

            // Act
            var result = fieldA.Equals(fieldB);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Equals_DifferentName_ShouldReturnFalse()
        {
            // Arrange
            var fieldA = new TestField("abc", true);
            var fieldB = new TestField("abcd", true);

            // Act
            var result = fieldA.Equals(fieldB);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Equals_DifferentValue_ShouldReturnFalse()
        {
            // Arrange
            var fieldA = new TestField("abc", true);
            var fieldB = new TestField("abc", false);

            // Act
            var result = fieldA.Equals(fieldB);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Equals_DifferentObjectType_ShouldReturnFalse()
        {
            // Arrange
            var field = new TestField("abc", true);

            // Act
            var result = field.Equals(123);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void GetHashCode_SameNameAndValue_ShouldReturnSameHash()
        {
            // Arrange
            var fieldA = new TestField("abc", true);
            var fieldB = new TestField("abc", true);

            // Act
            var hashA = fieldA.GetHashCode();
            var hashB = fieldB.GetHashCode();

            // Assert
            hashA.Should().Be(hashB);
        }

        [TestMethod]
        public void GetHashCode_DifferentName_ShouldReturnDifferentHash()
        {
            // Arrange
            var fieldA = new TestField("abc", true);
            var fieldB = new TestField("abcd", true);

            // Act
            var hashA = fieldA.GetHashCode();
            var hashB = fieldB.GetHashCode();

            // Assert
            hashA.Should().NotBe(hashB);
        }

        [TestMethod]
        public void GetHashCode_DifferentValue_ShouldReturnDifferentHash()
        {
            // Arrange
            var fieldA = new TestField("abc", true);
            var fieldB = new TestField("abc", false);

            // Act
            var hashA = fieldA.GetHashCode();
            var hashB = fieldB.GetHashCode();

            // Assert
            hashA.Should().NotBe(hashB);
        }

        private class TestField : Field<bool>
        {
            public TestField(string name, bool value)
                : base(name, value)
            {
            }

            public override void Apply(IRfcInterop interop, IntPtr dataHandle)
            {
                throw new NotImplementedException();
            }
        }
    }
}
