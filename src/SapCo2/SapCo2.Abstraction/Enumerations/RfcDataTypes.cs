namespace SapCo2.Abstraction.Enumerations
{
    public enum RfcDataTypes
    {
        STRING = 0,
        BYTE,
        RAW,
        CHAR,
        NUMERIC,
        NUMERIC_TR,
        DATE,
        DATE_TR,
        DATE_8, //yyyymmdd
        DATETIME,
        DATE_GDTU, //99999999-int(yyyymmdd)
        TIME, //HHmmss
        TIME_FULL, //HH:mm:ss
        FLOAT,
        FLOAT_TR,
        DECIMAL,
        DECIMAL_TR,
        DECIMAL_COMMAND_SIGN,
        DECIMAL_COMMAND_SIGN_TR,
        UNIT,
        INTEGER,
        BOOLEAN_X,
        QUAN_DOUBLE,
        FIXED_CHARACTER,
        QUAN_DOUBLE_US,
        DECIMAL_NO_CULTURE
    }
}
