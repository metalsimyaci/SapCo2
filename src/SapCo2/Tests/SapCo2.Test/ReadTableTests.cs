using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapCo2.Abstract;
using SapCo2.Attributes;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Attributes;
using SapCo2.Wrapper.Enumeration;
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
        private static IntPtr TableHandle = (IntPtr)2;
        private static readonly IntPtr LineHandle = (IntPtr)4;
        private static readonly IntPtr RowHandle = (IntPtr)16;

        private static Mock<IRfcConnection> ConnectionMock;
        private static Mock<IRfcInterop> InteropMock;
        private static Mock<IPropertyCache> PropertyCacheMock;

        [TestInitialize]
        public void Initializer()
        {
            InteropMock = new Mock<IRfcInterop>();

            ConnectionMock = new Mock<IRfcConnection>();
            ConnectionMock.Setup(p => p.GetConnectionHandle()).Returns(RfcConnectionHandle);

            RfcErrorInfo errorInfo;
            InteropMock
                .Setup(x => x.GetFunctionDesc(It.IsAny<IntPtr>(), It.IsAny<string>(), out errorInfo))
                .Returns(FunctionDescriptionHandle);
            InteropMock
                .Setup(x => x.CreateFunction(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(FunctionHandle);
            InteropMock
                .Setup(x => x.GetTable(It.IsAny<IntPtr>(), It.IsAny<string>(), out TableHandle, out errorInfo));
            InteropMock
                .Setup(x => x.AppendNewRow(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(LineHandle);
            InteropMock
                .Setup(x => x.GetCurrentRow(It.IsAny<IntPtr>(), out errorInfo))
                .Returns(RowHandle);

            PropertyCacheMock = new Mock<IPropertyCache>();
            PropertyCacheMock.Setup(p => p.GetPropertyInfo(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns((Type t, string s) =>
                {
                    IEnumerable<PropertyInfo> properties = t.GetProperties().Where(x => !x.GetGetMethod().IsVirtual).Select(x => x);
                    foreach (PropertyInfo property in properties)
                    {
                        if (!Attribute.IsDefined(property, typeof(RfcEntityPropertyAttribute)))
                            continue;

                        var attribute =
                            (RfcEntityPropertyAttribute)property.GetCustomAttributes(typeof(RfcEntityPropertyAttribute), false).FirstOrDefault();
                        string field = attribute?.Name;
                        if (field == s)
                            return property;
                    }

                    return properties.FirstOrDefault();
                });
        }

        [TestCleanup]
        public void Cleaner()
        {
            InteropMock = null;
            ConnectionMock = null;
            PropertyCacheMock = null;
        }

        [DataTestMethod]
        [DataRow(1, 12345)]
        [DataRow(2, 45785)]
        public void ReadTable_defaultCondition_ShouldReturnReadTableOutputModel(int tableRowCount, int testResultValue)
        {
            RfcErrorInfo errorInfo;

            string testValueString = testResultValue.ToString();
            uint stringLength = (uint)testValueString.Length;
            uint rowCount = (uint)tableRowCount;

            InteropMock
                .Setup(p => p.GetRowCount(TableHandle, out rowCount, out errorInfo));
            InteropMock
                .Setup(p =>
                    p.GetString(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<char[]>(), It.IsAny<uint>(), out stringLength,
                        out errorInfo))
                .Returns((IntPtr funHandler, string name, char[] buffer, uint bufferLength, uint valueLength, RfcErrorInfo rfcErrorInfo) =>
                {
                    if (bufferLength <= 0)
                        return RfcResultCodes.RFC_BUFFER_TOO_SMALL;
                    var testArray = testValueString.ToCharArray();
                    Buffer.BlockCopy(testArray, 0, buffer, 0, Buffer.ByteLength(testArray));
                    return RfcResultCodes.RFC_OK;
                });

            var readTable = new ReadTable<ReadTableOutputModel>(PropertyCacheMock.Object, InteropMock.Object);

            List<ReadTableOutputModel> resultModel = readTable.GetTable(ConnectionMock.Object);

            resultModel.Should().NotBeNull();
            resultModel.Should().HaveCount((int)rowCount);
            resultModel.First().Value.Should().Be(testResultValue);

            PropertyCacheMock.Verify(x => x.GetPropertyInfo(typeof(ReadTableOutputModel), "TESTPROPERTY"), Times.AtLeast(1));

            //Input Parameter Check
            InteropMock.Verify(x => x.SetString(FunctionHandle, "QUERY_TABLE", "TESTTABLE", 9, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(FunctionHandle, "DELIMITER", "|", 1, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(FunctionHandle, "NO_DATA", "", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetInt(FunctionHandle, "ROWCOUNT", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetInt(FunctionHandle, "ROWSKIPS", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(LineHandle, "FIELDNAME", "TESTPROPERTY", 12, out errorInfo), Times.Once);

            var outputArray = new char[testValueString.ToCharArray().Length + 1];
            Buffer.BlockCopy(testValueString.ToCharArray(), 0, outputArray, 0, Buffer.ByteLength(testValueString.ToCharArray()));
            //Output Parameter Check
            InteropMock.Verify(
                x => x.GetString(RowHandle, "WA", outputArray, (uint)outputArray.Length, out stringLength,
                    out errorInfo), Times.AtLeast(1));

            InteropMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);

        }


        [DataTestMethod]
        [DataRow(1, 12345,"AB EQ 'ss'")]
        public void ReadTable_withStringWhereCondition_ShouldReturnReadTableOutputModel(int tableRowCount, int testResultValue, string whereCondition)
        {
            RfcErrorInfo errorInfo;

            string testValueString = testResultValue.ToString();
            uint stringLength = (uint)testValueString.Length;
            uint rowCount = (uint)tableRowCount;

            InteropMock
                .Setup(p => p.GetRowCount(TableHandle, out rowCount, out errorInfo));
            InteropMock
                .Setup(p =>
                    p.GetString(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<char[]>(), It.IsAny<uint>(), out stringLength,
                        out errorInfo))
                .Returns((IntPtr funHandler, string name, char[] buffer, uint bufferLength, uint valueLength, RfcErrorInfo rfcErrorInfo) =>
                {
                    if (bufferLength <= 0)
                        return RfcResultCodes.RFC_BUFFER_TOO_SMALL;
                    var testArray = testValueString.ToCharArray();
                    Buffer.BlockCopy(testArray, 0, buffer, 0, Buffer.ByteLength(testArray));
                    return RfcResultCodes.RFC_OK;
                });

            var readTable = new ReadTable<ReadTableOutputModel>(PropertyCacheMock.Object, InteropMock.Object);

            List<ReadTableOutputModel> resultModel = readTable.GetTable(ConnectionMock.Object,new List<string>{whereCondition});

            resultModel.Should().NotBeNull();
            resultModel.Should().HaveCount((int)rowCount);
            resultModel.First().Value.Should().Be(testResultValue);

            PropertyCacheMock.Verify(x => x.GetPropertyInfo(typeof(ReadTableOutputModel), "TESTPROPERTY"), Times.AtLeast(1));

            //Input Parameter Check
            InteropMock.Verify(x => x.SetString(FunctionHandle, "QUERY_TABLE", "TESTTABLE", 9, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(FunctionHandle, "DELIMITER", "|", 1, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(FunctionHandle, "NO_DATA", "", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetInt(FunctionHandle, "ROWCOUNT", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetInt(FunctionHandle, "ROWSKIPS", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(LineHandle, "FIELDNAME", "TESTPROPERTY", 12, out errorInfo), Times.Once);

            var outputArray = new char[testValueString.ToCharArray().Length + 1];
            Buffer.BlockCopy(testValueString.ToCharArray(), 0, outputArray, 0, Buffer.ByteLength(testValueString.ToCharArray()));
            //Output Parameter Check
            InteropMock.Verify(
                x => x.GetString(RowHandle, "WA", outputArray, (uint)outputArray.Length, out stringLength,
                    out errorInfo), Times.AtLeast(1));

            InteropMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);

        }


        [DataTestMethod]
        [DataRow(12345)]
        [DataRow(45785)]
        public void ReadStruct_defaultCondition_ShouldReturnReadTableOutputModel(int testResultValue)
        {
            RfcErrorInfo errorInfo;

            string testValueString = testResultValue.ToString();
            uint stringLength = (uint)testValueString.Length;
            uint rowCount = 1;

            InteropMock
                .Setup(p => p.GetRowCount(TableHandle, out rowCount, out errorInfo));
            InteropMock
                .Setup(p =>
                    p.GetString(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<char[]>(), It.IsAny<uint>(), out stringLength,
                        out errorInfo))
                .Returns((IntPtr funHandler, string name, char[] buffer, uint bufferLength, uint valueLength, RfcErrorInfo rfcErrorInfo) =>
                {
                    if (bufferLength <= 0)
                        return RfcResultCodes.RFC_BUFFER_TOO_SMALL;
                    var testArray = testValueString.ToCharArray();
                    Buffer.BlockCopy(testArray, 0, buffer, 0, Buffer.ByteLength(testArray));
                    return RfcResultCodes.RFC_OK;
                });

            var readTable = new ReadTable<ReadTableOutputModel>(PropertyCacheMock.Object, InteropMock.Object);

            ReadTableOutputModel resultModel = readTable.GetStruct(ConnectionMock.Object);

            resultModel.Should().NotBeNull();
            resultModel.Value.Should().Be(testResultValue);

            PropertyCacheMock.Verify(x => x.GetPropertyInfo(typeof(ReadTableOutputModel), "TESTPROPERTY"), Times.AtLeast(1));

            //Input Parameter Check
            InteropMock.Verify(x => x.SetString(FunctionHandle, "QUERY_TABLE", "TESTTABLE", 9, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(FunctionHandle, "DELIMITER", "|", 1, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(FunctionHandle, "NO_DATA", "", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetInt(FunctionHandle, "ROWCOUNT", 1, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetInt(FunctionHandle, "ROWSKIPS", 0, out errorInfo), Times.Once);
            InteropMock.Verify(x => x.SetString(LineHandle, "FIELDNAME", "TESTPROPERTY", 12, out errorInfo), Times.Once);

            var outputArray = new char[testValueString.ToCharArray().Length + 1];
            Buffer.BlockCopy(testValueString.ToCharArray(), 0, outputArray, 0, Buffer.ByteLength(testValueString.ToCharArray()));
            //Output Parameter Check
            InteropMock.Verify(
                x => x.GetString(RowHandle, "WA", outputArray, (uint)outputArray.Length, out stringLength,
                    out errorInfo), Times.AtLeast(1));

            InteropMock.Verify(x => x.Invoke(RfcConnectionHandle, FunctionHandle, out errorInfo), Times.Once);

        }

        [RfcTable("TESTTABLE")]
        private sealed class ReadTableOutputModel
        {
            [RfcTableProperty("TESTPROPERTY")]
            public int Value { get; set; }
        }
    }
}
