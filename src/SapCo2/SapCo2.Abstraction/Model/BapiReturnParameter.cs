using SapCo2.Abstraction.Attributes;

namespace SapCo2.Abstraction.Model
{
    public sealed class BapiReturnParameter
    {
        #region Properties

        [RfcEntityProperty("CODE")]
        public string Code { get; set; }

        [RfcEntityProperty("TYPE")]
        public string MessageType { get; set; }

        [RfcEntityProperty("MESSAGE")]
        public string Message { get; set; }

        [RfcEntityProperty("LOG_NO")]
        public string LogNo { get; set; }

        [RfcEntityProperty("LOG_MSG_NO")]
        public string LogMessageNumber { get; set; }

        [RfcEntityProperty("MESSAGE_V1")]
        public string MessageV1 { get; set; }

        [RfcEntityProperty("MESSAGE_V2")]
        public string MessageV2 { get; set; }

        [RfcEntityProperty("MESSAGE_V3")]
        public string MessageV3 { get; set; }

        [RfcEntityProperty("MESSAGE_V4")]
        public string MessageV4 { get; set; }

        #endregion
    }
}
