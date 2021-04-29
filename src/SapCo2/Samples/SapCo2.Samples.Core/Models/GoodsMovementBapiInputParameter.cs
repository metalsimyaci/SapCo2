using System;
using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using SapCo2.Abstraction.Enumerations;

namespace SapCo2.Samples.Core.Models
{

    public class GoodsMovementBapiInputParameter : IBapiInput
    {
        [RfcEntityProperty("GOODSMVT_HEADER", "Material Document Header Data")]
        public Header Header { get; set; }

        [RfcEntityProperty("GOODSMVT_CODE", "GM_CODE conversion for stock management process")]
        public MovementType MovementType { get; set; }

        [RfcEntityProperty("GOODSMVT_ITEM","Movement Material Items")]
        public MovementItem[] Items { get; set; }

        [RfcEntityProperty("GOODSMVT_SERIALNUMBER","Material Serial Numbers")]
        public MovementSerialNumber[] SerialNumbers { get; set; }
    }

    public class Header
    {
        [RfcEntityProperty("DOC_DATE","Document Date",RfcDataTypes.DATE_8)]
        public DateTime DocumentDate { get; set; }

        [RfcEntityProperty("PSTNG_DATE", "posting date in document",RfcDataTypes.DATE_8)]
        public DateTime PostingDate { get; set; }
    }

    public class MovementType
    {
        [RfcEntityProperty("GM_CODE", "transaction assignment code for goods movement")]
        public string MovementTypeCode { get; set; }
    }

    public class MovementItem
    {
        [RfcEntityProperty("MATERIAL", "Material Code", RfcDataTypes.CHAR, 18)]
        public string MaterialCode { get; set; }

        [RfcEntityProperty("PLANT", "Production Place Code", RfcDataTypes.CHAR, 4)]
        public string PlantCode { get; set; }

        [RfcEntityProperty("STGE_LOC", "Warehouse Code", RfcDataTypes.CHAR, 4)]
        public string WarehouseCode { get; set; }

        [RfcEntityProperty("MOVE_TYPE", "Operation Type", RfcDataTypes.CHAR, 3)]
        public string MoveType { get; set; }

        [RfcEntityProperty("VAL_TYPE", "Value Type", RfcDataTypes.CHAR, 10)]
        public string ValueType { get; set; }

        [RfcEntityProperty("ENTRY_QNT", "Quantity", RfcDataTypes.QUAN_DOUBLE, 13)]
        public double Quantity { get; set; }

        [RfcEntityProperty("ENTRY_UOM", "Measurement Unit Type", RfcDataTypes.UNIT, 3)]
        public string QuantityMeasurementUnitType { get; set; }

        [RfcEntityProperty("MOVE_MAT", "Material Code", RfcDataTypes.CHAR, 18)]
        public string MovementMaterialCode { get; set; }

        [RfcEntityProperty("SPEC_STOCK", "Special Stock Indicator", RfcDataTypes.CHAR, 1)]
        public char SpecialStockIndicator { get; set; }

        [RfcEntityProperty("VAL_SALES_ORD", "Order number of the evaluated customer order inventory", RfcDataTypes.CHAR, 10)]
        public string SalesOrderNumberOld { get; set; }

        [RfcEntityProperty("AL_S_ORD_ITEM", "Order item of the evaluated customer order inventory", RfcDataTypes.NUMERIC, 6)]
        public string SalesItemNumberOld { get; set; }

        [RfcEntityProperty("SALES_ORD", "Customer Order Number", RfcDataTypes.CHAR, 10)]
        public string OrderNumber { get; set; }
        
        [RfcEntityProperty("S_ORD_ITEM", "Customer Item Number", RfcDataTypes.NUMERIC, 6)]
        public string OrderItemNumber { get; set; }
        
        [RfcEntityProperty("MOVE_PLANT", "Receiving / departing production place code", RfcDataTypes.CHAR, 4)]
        public string MovementPlantCode { get; set; }
        
        [RfcEntityProperty("MOVE_STLOC", "Receiving / departing warehouse code", RfcDataTypes.CHAR, 4)]
        public string MovementWarehouseCode { get; set; }

        [RfcEntityProperty("MOVE_VAL_TYPE", "Valuation type for transport batches", RfcDataTypes.CHAR, 10)]
        public string MovementValueType { get; set; }
    }

    public class MovementSerialNumber
    {
        [RfcEntityProperty("MATDOC_ITM", "Material document item", RfcDataTypes.NUMERIC,4)]
        public string ItemOrderNo { get; set; }

        [RfcEntityProperty("SERIALNO", "SerialNoCode", RfcDataTypes.CHAR, 18)]
        public string SerialNumber { get; set; }
    }
}
