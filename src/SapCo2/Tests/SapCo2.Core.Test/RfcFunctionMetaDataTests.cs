using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class RfcFunctionMetaDataTests
    {
        private static readonly IntPtr FunctionDescriptionHandle = (IntPtr)20;
        private static readonly IntPtr TypeDescriptionHandle = (IntPtr)55;
        private readonly Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();

        [TestMethod]
        public void GetParameterDescriptions_ShouldRfcParameterDescriptionModelList()
        {
            int parameterCount=1;
            RfcErrorInfo errorInfo;
            var parameterDescription = new RfcParameterDescription {Name = "Parameter Name"};

            _interopMock.Setup(s => s.GetParameterCount(It.IsAny<IntPtr>(), out parameterCount, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);
            
            _interopMock.Setup(s => s.GetParameterDescByIndex(It.IsAny<IntPtr>(), It.IsAny<int>(),out parameterDescription, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);
          
            var functionMetaData = new RfcFunctionMetaData(_interopMock.Object, FunctionDescriptionHandle);

            List<RfcParameterDescription> result = functionMetaData.GetParameterDescriptions();

            result.Should().HaveCount(parameterCount);
            result.First().ParameterText.Should().Be(parameterDescription.ParameterText);
        }

        [TestMethod]
        public void GetParameterDescriptions_typeDescriptionHandle_ShouldRfcParameterDescriptionModelList()
        {
            int fieldCount = 1;
            RfcErrorInfo errorInfo;
            var fieldDescription = new RfcFieldDescription {Name = "Field Name"};

            _interopMock.Setup(s => s.GetFieldCount(It.IsAny<IntPtr>(), out fieldCount, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);

            _interopMock.Setup(s => s.GetFieldDescByIndex(It.IsAny<IntPtr>(), It.IsAny<int>(), out fieldDescription, out errorInfo))
                .Returns(RfcResultCodes.RFC_OK);
            var functionMetaData = new RfcFunctionMetaData(_interopMock.Object, FunctionDescriptionHandle);

            List<RfcFieldDescription> result = functionMetaData.GetFieldDescriptions(TypeDescriptionHandle);

            result.Should().HaveCount(fieldCount);
            result.First().Name.Should().Be(fieldDescription.Name);
        }
    }
}
