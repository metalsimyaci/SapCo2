using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Mappers;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Test.Mapper
{
    [TestClass]
    public sealed class InputMapperTests
    {
        private static readonly Fixture Fixture = new Fixture();
        private static IntPtr DataHandle;
        private Mock<IRfcInterop> _interopMock; 

        static InputMapperTests()
        {
            
        }

        [TestInitialize]
        public void Intitialize()
        {
            DataHandle = (IntPtr)123;
            _interopMock = new Mock<IRfcInterop>();
        }

        [TestMethod]
        public void Apply_NullInput_ShouldNotThrow()
        {
            Action action = () => InputMapper.Apply(_interopMock.Object, DataHandle, null);

            action.Should().NotThrow();
        }

        [TestMethod]
        public void Apply_String_ShouldMapAsString()
        {
            RfcErrorInfo errorInfo;

            InputMapper.Apply(_interopMock.Object, DataHandle, new {  SomeString = (string)"Hello" });

            _interopMock.Verify(x => x.SetString(DataHandle, "SOMESTRING", "Hello", 5, out errorInfo));
        }

        [TestMethod]
        public void Apply_NullString_ShouldNotMapString()
        {
            RfcErrorInfo errorInfo;
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeString = (string)null });
           
            _interopMock.Verify(
                x => x.SetString(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<uint>(), out errorInfo),
                Times.Never);
        }

        [TestMethod]
        public void Apply_Int_ShouldMapAsInt()
        {
            RfcErrorInfo errorInfo;
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeInt = 555 });
           
            _interopMock.Verify(x => x.SetInt(DataHandle, "SOMEINT", 555, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_Long_ShouldMapAsInt8()
        {
            RfcErrorInfo errorInfo;
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeLong = 123L });
           
            _interopMock.Verify(x => x.SetInt8(DataHandle, "SOMELONG", 123L, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_Double_ShouldMapAsFloat()
        {
            RfcErrorInfo errorInfo;
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeDouble = 1234.5d });
           
            _interopMock.Verify(x => x.SetFloat(DataHandle, "SOMEDOUBLE", 1234.5d, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_Decimal_ShouldMapAsFormattedString()
        {
            RfcErrorInfo errorInfo;
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeDecimal = 123.4M });
           
            _interopMock.Verify(x => x.SetString(DataHandle, "SOMEDECIMAL", "123.4", 5, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_DateTime_ShouldMapAsDate()
        {
            RfcErrorInfo errorInfo;
            var date = new DateTime(2020, 4, 1);
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeDate = date });
           
            _interopMock.Verify(x => x.SetDate(
                DataHandle,
                "SOMEDATE",
                It.Is<char[]>(y => y.SequenceEqual("20200401")),
                out errorInfo));
        }

        [TestMethod]
        public void Apply_NullableDateTime_HasValue_ShouldMapAsDate()
        {
            RfcErrorInfo errorInfo;
            DateTime? date = new DateTime(2020, 4, 1);
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeDate = date });
           
            _interopMock.Verify(x => x.SetDate(
                DataHandle,
                "SOMEDATE",
                It.Is<char[]>(y => y.SequenceEqual("20200401")),
                out errorInfo));
        }

        [TestMethod]
        public void Apply_NullableDateTime_NullValue_ShouldMapAsZeroDate()
        {
            RfcErrorInfo errorInfo;
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeDate = (DateTime?)null });
           
            _interopMock.Verify(
                x => x.SetDate(
                    DataHandle,
                    "SOMEDATE",
                    It.Is<char[]>(y => y.SequenceEqual("00000000")),
                    out errorInfo),
                Times.Once);
        }

        [TestMethod]
        public void Apply_TimeSpan_ShouldMapAsTime()
        {
            RfcErrorInfo errorInfo;
            var time = new TimeSpan(23, 45, 16);
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeTime = time });
           
            _interopMock.Verify(
                x => x.SetTime(
                    DataHandle,
                    "SOMETIME",
                    It.Is<char[]>(y => y.SequenceEqual("234516")),
                    out errorInfo),
                Times.Once);
        }

        [TestMethod]
        public void Apply_NullableTimeSpan_HasValue_ShouldMapAsTime()
        {
            RfcErrorInfo errorInfo;
            TimeSpan? time = new TimeSpan(23, 45, 16);
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeTime = time });
           
            _interopMock.Verify(
                x => x.SetTime(
                    DataHandle,
                    "SOMETIME",
                    It.Is<char[]>(y => y.SequenceEqual("234516")),
                    out errorInfo),
                Times.Once);
        }

        [TestMethod]
        public void Apply_NullableTimeSpan_NullValue_ShouldMapAsZeroTime()
        {
            RfcErrorInfo errorInfo;
           
            InputMapper.Apply(_interopMock.Object, DataHandle, new { SomeTime = (TimeSpan?)null });
           
            _interopMock.Verify(
                x => x.SetTime(
                    DataHandle,
                    "SOMETIME",
                    It.Is<char[]>(y => y.SequenceEqual("000000")),
                    out errorInfo),
                Times.Once);
        }

        [TestMethod]
        public void Apply_Array_ShouldMapAsTable()
        {
            RfcErrorInfo errorInfo;
            var model = new { SomeArray = Fixture.CreateMany<ArrayElement>(2).ToArray() };
           
            InputMapper.Apply(_interopMock.Object, DataHandle, model);
           
            IntPtr tableHandle;
            _interopMock.Verify(x => x.GetTable(DataHandle, "SOMEARRAY", out tableHandle, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_Array_ShouldMapRowsAndValues()
        {
            const int NUMBER_OF_ROWS = 5;
            var tableHandle = (IntPtr)1235;
            var lineHandle = (IntPtr)2245;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetTable(DataHandle, "SOMEARRAY", out tableHandle, out errorInfo));
            _interopMock.Setup(x => x.AppendNewRow(It.IsAny<IntPtr>(), out errorInfo)).Returns(lineHandle);
            var model = new { SomeArray = Fixture.CreateMany<ArrayElement>(NUMBER_OF_ROWS).ToArray() };
           
            InputMapper.Apply(_interopMock.Object, DataHandle, model);
           
            _interopMock.Verify(x => x.AppendNewRow(tableHandle, out errorInfo), Times.Exactly(NUMBER_OF_ROWS));
            foreach (ArrayElement element in model.SomeArray)
            {
                var length = (uint)element.Value.Length;
                _interopMock.Verify(
                    x => x.SetString(lineHandle, "VALUE", element.Value, length, out errorInfo),
                    Times.Once);
            }
        }

        [TestMethod]
        public void Apply_Structure_ShouldMapAsStructure()
        {
            var structHandle = (IntPtr)44553;
            RfcErrorInfo errorInfo;
            _interopMock.Setup(x => x.GetStructure(It.IsAny<IntPtr>(), It.IsAny<string>(), out structHandle, out errorInfo));
            var model = new StructureModel { Structure = new Structure { Value = 224 } };
           
            InputMapper.Apply(_interopMock.Object, DataHandle, model);
           
            _interopMock.Verify(x => x.GetStructure(DataHandle, "STRUCTURE", out structHandle, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.SetInt(structHandle, "VALUE", 224, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_ModelWithSapIgnoreAttribute_ShouldIgnorePropertiesWithIgnoreAttribute()
        {
            RfcErrorInfo errorInfo;
            var model = new SapIgnoreAttributeModel { Value = 123, IgnoredProperty = 234 };
           
            InputMapper.Apply(_interopMock.Object, DataHandle, model);
           
            _interopMock.Verify(x => x.SetInt(DataHandle, "VALUE", 123, out errorInfo), Times.Once);
            _interopMock.Verify(x => x.SetInt(DataHandle, "IGNOREDPROPERTY", 234, out errorInfo), Times.Never);
        }

        [TestMethod]
        public void Apply_ModelWithSapNameAttribute_ShouldUseSapNameInsteadOfPropertyName()
        {
            RfcErrorInfo errorInfo;
            var model = new SapNameAttributeModel { Value = 123 };
           
            InputMapper.Apply(_interopMock.Object, DataHandle, model);
           
            _interopMock.Verify(x => x.SetInt(DataHandle, "IN_VAL", 123, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_ModelWithCustomNameAttribute_ShouldUseCustomSapNameInsteadOfPropertyName()
        {
            RfcErrorInfo errorInfo;
            var model = new CustomNameAttributeModel { Value = 123 };
           
            InputMapper.Apply(_interopMock.Object, DataHandle, model);
           
            _interopMock.Verify(x => x.SetInt(DataHandle, "CUSTOM_IN_VAL", 123, out errorInfo), Times.Once);
        }

        [TestMethod]
        public void Apply_UnknownTypeThatCannotBeConstructed_ShouldThrowException()
        {
            Action action = () => InputMapper.Apply(_interopMock.Object, DataHandle, new { UnknownType = 1.0f });
           
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("No matching field constructor found");
        }

        private sealed class ArrayElement
        {
            public string Value { get; set; } = "123";
        }

        private sealed class StructureModel
        {
            public Structure Structure { get; set; }
        }

        private sealed class Structure
        {
            public int Value { get; set; }
        }

        private sealed class SapNameAttributeModel
        {
            [RfcEntityProperty("IN_VAL")]
            public int Value { get; set; }
        }

        private sealed class SapIgnoreAttributeModel
        {
            public int Value { get; set; }

            [RfcEntityIgnoreProperty]
            public int IgnoredProperty { get; set; }
        }

        private sealed class CustomNameAttribute : RfcEntityPropertyAttribute
        {
            public CustomNameAttribute(string customName)
                : base($"CUSTOM_{customName}")
            {
            }
        }

        private sealed class CustomNameAttributeModel
        {
            [CustomNameAttribute("IN_VAL")]
            public int Value { get; set; }
        }

    }
}
