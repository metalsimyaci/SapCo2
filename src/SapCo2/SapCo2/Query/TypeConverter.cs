using System;
using System.Globalization;
using System.Reflection;
using SapCo2.Abstraction.Enumerations;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Query
{
    internal static class TypeConverter
    {
        public static string ConvertToRfcType(object value, RfcDataTypes rfcTablePropertySapType)
        {
            switch (rfcTablePropertySapType)
            {
            case RfcDataTypes.STRING:
            case RfcDataTypes.BYTE:
            case RfcDataTypes.CHAR:
            case RfcDataTypes.UNIT:
            case RfcDataTypes.NUMERIC_TR:
            case RfcDataTypes.DECIMAL_COMMAND_SIGN:
            case RfcDataTypes.DECIMAL_COMMAND_SIGN_TR:
            case RfcDataTypes.QUAN_DOUBLE:
                return value.ToString();

            case RfcDataTypes.NUMERIC:
                return string.Empty;

            case RfcDataTypes.DATE:
                if (DateTime.TryParse(value.ToString(), out _))
                    return ((DateTime)value).ToString("dd.MM.yyyy");

                throw new RfcConversionException("Invalid Date [dd.MM.yyyy]");

            case RfcDataTypes.DATE_8:
                if (DateTime.TryParse(value.ToString(), out _))
                    return ((DateTime)value).ToString("yyyyMMdd");

                throw new RfcConversionException("Invalid Date [yyyyMMdd]");

            case RfcDataTypes.DATETIME:
                return string.Empty;

            case RfcDataTypes.DATE_GDTU:
                if (!DateTime.TryParse(value.ToString(), out _))
                    throw new RfcConversionException("Invalid Date [yyyyMMdd]");

                if (int.TryParse(((DateTime)value).ToString("yyyyMMdd"), out int parsedDateGdatuInt))
                    return (99999999 - parsedDateGdatuInt).ToString();

                throw new RfcConversionException($"Date(yyyyMMdd) not converted integer");

            case RfcDataTypes.TIME:
                if (DateTime.TryParse(value.ToString(), out _))
                    return ((DateTime)value).ToString("hhmmss");

                throw new RfcConversionException($"Invalid Time [format:hhmmss, value:{value}]");

            case RfcDataTypes.FLOAT:
                if (float.TryParse(value.ToString(), out _))
                    return ((float)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Float [culture:en-US, value:{value}]");

            case RfcDataTypes.FLOAT_TR:
                if (float.TryParse(value.ToString(), out _))
                    return ((float)value).ToString(new CultureInfo("tr-TR"));

                throw new RfcConversionException($"Invalid Float [culture:tr-TR, value:{value}]");

            case RfcDataTypes.DECIMAL:
                if (decimal.TryParse(value.ToString(), out _))
                    return ((decimal)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Decimal [culture:en-US, value:{value}]");

            case RfcDataTypes.DECIMAL_TR:
                if (decimal.TryParse(value.ToString(), out _))
                    return ((decimal)value).ToString(new CultureInfo("tr-TR"));

                throw new RfcConversionException($"Invalid Decimal [culture:tr-TR, value:{value}]");

            case RfcDataTypes.INTEGER:
                if (int.TryParse(value.ToString(), out _))
                    return ((int)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Integer [culture:en-US, value:{value}]");

            case RfcDataTypes.BOOLEAN_X:
                if (bool.TryParse(value.ToString(), out bool b))
                    return b ? "X" : string.Empty;

                throw new RfcConversionException($"Invalid Boolean [value:{value}]");

            default:
                throw new RfcConversionException($"Not Match rfcEntityPropertyRfcType [{nameof(rfcTablePropertySapType)}:{rfcTablePropertySapType}]");

            }
        }

        public static void ConvertFromRfcType(object instance, PropertyInfo property, object value, RfcDataTypes rfcTablePropertySapType)
        {
            string trimmedValue = value?.ToString()?.Trim();
            string pattern;

            switch (rfcTablePropertySapType)
            {
            case RfcDataTypes.STRING:
            case RfcDataTypes.CHAR:
            case RfcDataTypes.BYTE:
            case RfcDataTypes.UNIT:
                property.SetValue(instance, Convert.ChangeType(trimmedValue, property.PropertyType, null));
                break;

            case RfcDataTypes.FIXED_CHARACTER:
                property.SetValue(instance, trimmedValue?.TrimStart('0'));
                break;

            case RfcDataTypes.RAW:
                property.SetValue(instance, value);
                break;

            case RfcDataTypes.DATE:
                pattern = "dd.MM.yyyy";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out DateTime parsedDate);
                property.SetValue(instance, parsedDate);
                break;

            case RfcDataTypes.DATE_TR:
                pattern = "yyyy-MM-dd";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out DateTime parsedDateTr);
                property.SetValue(instance, parsedDateTr);
                break;

            case RfcDataTypes.DATE_8:
                pattern = "yyyyMMdd";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out DateTime parsedDate8);
                property.SetValue(instance, parsedDate8);
                break;

            case RfcDataTypes.DATE_GDTU:
                pattern = "yyyyMMdd";
                int.TryParse(trimmedValue, out int dateGdtu);
                DateTime.TryParseExact((99999999 - dateGdtu).ToString(), pattern, null, DateTimeStyles.None, out DateTime parsedDateGdtu);
                property.SetValue(instance, parsedDateGdtu);
                break;

            case RfcDataTypes.DATETIME:
                property.SetValue(instance, value);
                break;

            case RfcDataTypes.TIME:
                pattern = "HHmmss";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out DateTime parsedTime);
                property.SetValue(instance, parsedTime);
                break;

            case RfcDataTypes.TIME_FULL:
                pattern = "HH:mm:ss";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out DateTime parsedTimeFull);
                property.SetValue(instance, parsedTimeFull);
                break;

            case RfcDataTypes.FLOAT:
                float.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out float f);
                property.SetValue(instance, f);
                break;

            case RfcDataTypes.FLOAT_TR:
                float.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out float fTr);
                property.SetValue(instance, fTr);
                break;

            case RfcDataTypes.DECIMAL_NO_CULTURE:
                decimal.TryParse(trimmedValue, out decimal dCurrentCulture);
                property.SetValue(instance, dCurrentCulture);
                break;

            case RfcDataTypes.DECIMAL:
                decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out decimal d);
                property.SetValue(instance, d);
                break;

            case RfcDataTypes.DECIMAL_TR:
                decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out decimal dTr);
                property.SetValue(instance, dTr);
                break;

            case RfcDataTypes.DECIMAL_COMMAND_SIGN:
                decimal.TryParse(trimmedValue, NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign, new CultureInfo("en-US"),
                    out decimal decimalCommandSing);
                property.SetValue(instance, decimalCommandSing);
                break;

            case RfcDataTypes.DECIMAL_COMMAND_SIGN_TR:
                decimal.TryParse(trimmedValue, NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign, new CultureInfo("tr-TR"),
                    out decimal decimalCommandSingTr);
                property.SetValue(instance, decimalCommandSingTr);
                break;

            case RfcDataTypes.NUMERIC:
                if (string.IsNullOrWhiteSpace(trimmedValue) || trimmedValue.Length <= 4)
                {
                    int.TryParse(trimmedValue, out int iNumeric);
                    property.SetValue(instance, iNumeric);
                }
                else if (trimmedValue.Length <= 8)
                {
                    double.TryParse(trimmedValue, NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign, new CultureInfo("en-US"),
                        out double nDbNumericEn);
                    property.SetValue(instance, nDbNumericEn);
                }
                else
                {
                    decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out decimal ndEn);
                    property.SetValue(instance, ndEn);
                }
                break;

            case RfcDataTypes.NUMERIC_TR:
                if (string.IsNullOrWhiteSpace(trimmedValue) || trimmedValue.Length <= 4)
                {
                    int.TryParse(trimmedValue, out int iNumeric);
                    property.SetValue(instance, iNumeric);
                }
                else if (trimmedValue.Length <= 8)
                {
                    double.TryParse(trimmedValue, NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign, new CultureInfo("tr-TR"),
                        out double nDbNumericTr);
                    property.SetValue(instance, nDbNumericTr);
                }
                else
                {
                    decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out decimal ndTr);
                    property.SetValue(instance, ndTr);
                }
                break;

            case RfcDataTypes.INTEGER:
                int.TryParse(trimmedValue, out int i);
                property.SetValue(instance, i);
                break;

            case RfcDataTypes.BOOLEAN_X:
                bool x = string.Equals(trimmedValue, "X", StringComparison.InvariantCultureIgnoreCase);
                property.SetValue(instance, x);
                break;

            case RfcDataTypes.QUAN_DOUBLE:
                double.TryParse(trimmedValue, NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign, CultureInfo.CurrentCulture, out double db);
                property.SetValue(instance, db);
                break;

            case RfcDataTypes.QUAN_DOUBLE_US:
                double.TryParse(trimmedValue, NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign, new CultureInfo("en-US"), out double dbUs);
                property.SetValue(instance, dbUs);
                break;

            default:
                throw new RfcConversionException($"Not Match RfcDataType [{nameof(rfcTablePropertySapType)}:{rfcTablePropertySapType}]");
            }
        }
    }
}
