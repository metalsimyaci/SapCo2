using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Abstraction.Enumerations;
using SapCo2.Abstraction.Model;

namespace SapCo2.Samples.Core.Models
{
    public class MaterialSaveDataBapiOutputParameter:IBapiOutput
    {
        [RfcEntityProperty("RETURN")]
        public BapiReturnParameter BapiReturn { get; set; }

        [RfcEntityProperty("RETURNMESSAGES")]
        public ExtendedReturnMessage ExtendedReturnMessage { get; set; }
    }

    public class ExtendedReturnMessage
    {
        [RfcEntityProperty("ID")]
        public string Id { get; set; }

        [RfcEntityProperty("TYPE")]
        public string MessageType { get; set; }

        [RfcEntityProperty("NUMBER")]
        public string Number { get; set; }

        [RfcEntityProperty("MESSAGE")]
        public string Message { get; set; }

        [RfcEntityProperty("LOG_NO")]
        public string LogNo { get; set; }

        [RfcEntityProperty("LOG_MSG_NO")]
        public string LogMessageNumber { get; set; }

        [RfcEntityProperty("MESSAGE_V1", "Additional Message", RfcDataTypes.CHAR, 50)]
        public string MessageV1 { get; set; }

        [RfcEntityProperty("MESSAGE_V2", "Additional Message", RfcDataTypes.CHAR, 50)]
        public string MessageV2 { get; set; }

        [RfcEntityProperty("MESSAGE_V3", "Additional Message", RfcDataTypes.CHAR, 50)]
        public string MessageV3 { get; set; }

        [RfcEntityProperty("MESSAGE_V4", "Additional Message", RfcDataTypes.CHAR, 50)]
        public string MessageV4 { get; set; }

        [RfcEntityProperty("PARAMETER", "Parameter Name", RfcDataTypes.CHAR, 32)]
        public string Parameter { get; set; }

        [RfcEntityProperty("ROW", "Row Number in Parameter", RfcDataTypes.INTEGER, 4)]
        public int Row { get; set; }

        [RfcEntityProperty("FIELD", "Field Name in Parameter", RfcDataTypes.CHAR, 10)]
        public string Field { get; set; }

        [RfcEntityProperty("SYSTEM","",RfcDataTypes.CHAR,10)]
        public string System { get; set; }
    }
}
