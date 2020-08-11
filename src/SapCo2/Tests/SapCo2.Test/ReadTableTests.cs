using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Abstract;
using SapCo2.Core;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class ReadTableTests
    {
        private static readonly IntPtr RfcConnectionHandle = (IntPtr)12;
        private static readonly IntPtr FunctionDescriptionHandle = (IntPtr)34;
        private static readonly IntPtr FunctionHandle = (IntPtr)56;
        private static readonly Mock<IRfcConnection> ConnectionMock = new Mock<IRfcConnection>();
        private static Mock<IRfcInterop> _interopMock = new Mock<IRfcInterop>();
        private static Mock<IPropertyCache> _propertyCacheMock = new Mock<IPropertyCache>();

        [ClassInitialize]
        public static void ClassInitializer(TestContext context)
        {
            ConnectionMock.Setup(p => p.GetConnectionHandle()).Returns(RfcConnectionHandle);
            RfcErrorInfo errorInfo;
            _interopMock
                .Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(),It.IsAny<string>(),  out errorInfo))
                .Returns(FunctionDescriptionHandle);
            _interopMock
                .Setup(x => x.CreateFunction(It.IsAny<IntPtr>(),  out errorInfo))
                .Returns(FunctionHandle);
        }
        
        
        [TestMethod]
        public void ReadTable_ReadTableOutputModel_Should()
        {
            var readTable=new ReadTable<ReadTableOutputModel>(_propertyCacheMock.Object,_interopMock.Object);
            
            List<ReadTableOutputModel> resultModel = readTable.GetTable(ConnectionMock.Object);

            resultModel.Should().NotBeNull();
            _interopMock.Verify(x => x.CreateFunction(FunctionDescriptionHandle, out errorInfo), Times.Once);
        }
        private sealed class ReadTableOutputModel
        {
            public int Value { get; set; }
        }
    }
    
    
}
