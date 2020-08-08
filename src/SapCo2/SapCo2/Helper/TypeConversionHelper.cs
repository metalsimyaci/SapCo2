using System;
using System.Globalization;
using System.Reflection;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Helper
{
    internal static class TypeConversionHelper
    {
        public static string ConvertToRfcType(object value, RfcEntityPropertySapTypes rfcEntityPropertySapType)
        {
            switch (rfcEntityPropertySapType)
            {
            case RfcEntityPropertySapTypes.STRING:
            case RfcEntityPropertySapTypes.BYTE:
            case RfcEntityPropertySapTypes.CHAR:
            case RfcEntityPropertySapTypes.UNIT:
            case RfcEntityPropertySapTypes.NUMERIC_TR:
            case RfcEntityPropertySapTypes.DECIMAL_COMMAND_SIGN:
            case RfcEntityPropertySapTypes.DECIMAL_COMMAND_SIGN_TR:
            case RfcEntityPropertySapTypes.QUAN_DOUBLE:
                return value.ToString();

            case RfcEntityPropertySapTypes.NUMERIC:
                return string.Empty;

            case RfcEntityPropertySapTypes.DATE:
                if (DateTime.TryParse(value.ToString(), out var parsedDate))
                    return ((DateTime)value).ToString("dd.MM.yyyy");

                throw new RfcConversionException("Invalid Date [dd.MM.yyyy]");

            case RfcEntityPropertySapTypes.DATE_8:
                if (DateTime.TryParse(value.ToString(), out var parsedDate8))
                    return ((DateTime)value).ToString("yyyyMMdd");

                throw new RfcConversionException("Invalid Date [yyyyMMdd]");

            case RfcEntityPropertySapTypes.DATETIME:
                return string.Empty;

            case RfcEntityPropertySapTypes.DATE_GDTU:

                if (!DateTime.TryParse(value.ToString(), out var parsedDateGdatu))
                    throw new RfcConversionException("Invalid Date [yyyyMMdd]");

                if (int.TryParse(((DateTime)value).ToString("yyyyMMdd"), out var parsedDateGdatuInt))
                    return (99999999 - parsedDateGdatuInt).ToString();

                throw new RfcConversionException($"Date(yyyyMMdd) not converted integer");

            case RfcEntityPropertySapTypes.TIME:
                if (DateTime.TryParse(value.ToString(), out var parsedTime))
                    return ((DateTime)value).ToString("hhmmss");

                throw new RfcConversionException($"Invalid Time [format:hhmmss, value:{value}]");

            case RfcEntityPropertySapTypes.FLOAT:
                if (float.TryParse(value.ToString(), out var f))
                    return ((float)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Float [culture:en-US, value:{value}]");

            case RfcEntityPropertySapTypes.FLOAT_TR:
                if (float.TryParse(value.ToString(), out var fTr))
                    return ((float)value).ToString(new CultureInfo("tr-TR"));

                throw new RfcConversionException($"Invalid Float [culture:tr-TR, value:{value}]");

            case RfcEntityPropertySapTypes.DECIMAL:
                if (decimal.TryParse(value.ToString(), out var d))
                    return ((decimal)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Decimal [culture:en-US, value:{value}]");

            case RfcEntityPropertySapTypes.DECIMAL_TR:
                if (decimal.TryParse(value.ToString(), out var dTr))
                    return ((decimal)value).ToString(new CultureInfo("tr-TR"));

                throw new RfcConversionException($"Invalid Decimal [culture:tr-TR, value:{value}]");

            case RfcEntityPropertySapTypes.INTEGER:
                if (int.TryParse(value.ToString(), out var i))
                    return ((int)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Integer [culture:en-US, value:{value}]");

            case RfcEntityPropertySapTypes.BOOLEAN_X:
                if (bool.TryParse(value.ToString(), out var b))
                    return b ? "X" : string.Empty;

                throw new RfcConversionException($"Invalid Boolean [value:{value}]");

            default:
                throw new RfcConversionException($"Not Match rfcEntityPropertySapType [{nameof(rfcEntityPropertySapType)}:{rfcEntityPropertySapType}]");

            }
        }

        public static void ConvertFromRfcType(object instance, PropertyInfo property, object value, RfcEntityPropertySapTypes rfcEntityPropertySapType)
        {
            string trimmedValue = value?.ToString().Trim();
            string pattern;

            switch (rfcEntityPropertySapType)
            {
            case RfcEntityPropertySapTypes.STRING:
            case RfcEntityPropertySapTypes.CHAR:
            case RfcEntityPropertySapTypes.BYTE:
            case RfcEntityPropertySapTypes.UNIT:
                property.SetValue(instance, Convert.ChangeType(trimmedValue, property.PropertyType, null));
                break;

            case RfcEntityPropertySapTypes.FIXED_CHARACTER:
                property.SetValue(instance, trimmedValue?.TrimStart('0'));
                break;

            case RfcEntityPropertySapTypes.RAW:
                property.SetValue(instance, value);
                break;

            case RfcEntityPropertySapTypes.DATE:
                pattern = "dd.MM.yyyy";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedDate);
                property.SetValue(instance, parsedDate);
                break;

            case RfcEntityPropertySapTypes.DATE_TR:
                pattern = "yyyy-MM-dd";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedDateTr);
                property.SetValue(instance, parsedDateTr);
                break;

            case RfcEntityPropertySapTypes.DATE_8:
                pattern = "yyyyMMdd";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedDate8);
                property.SetValue(instance, parsedDate8);
                break;

            case RfcEntityPropertySapTypes.DATE_GDTU:
                pattern = "yyyyMMdd";
                int.TryParse(trimmedValue, out var dateGdtu);
                DateTime.TryParseExact((99999999 - dateGdtu).ToString(), pattern, null, DateTimeStyles.None,
                    out var parsedDateGdtu);
                property.SetValue(instance, parsedDateGdtu);
                break;

            case RfcEntityPropertySapTypes.DATETIME:
                property.SetValue(instance, value);
                break;

            case RfcEntityPropertySapTypes.TIME:
                pattern = "HHmmss";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedTime);
                property.SetValue(instance, parsedTime);
                break;

            case RfcEntityPropertySapTypes.TIME_FULL:
                pattern = "HH:mm:ss";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedTimeFull);
                property.SetValue(instance, parsedTimeFull);
                break;

            case RfcEntityPropertySapTypes.FLOAT:
                float.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out var f);
                property.SetValue(instance, f);
                break;

            case RfcEntityPropertySapTypes.FLOAT_TR:
                float.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out var fTr);
                property.SetValue(instance, fTr);
                break;

            case RfcEntityPropertySapTypes.DECIMAL_NO_CULTURE:
                decimal.TryParse(trimmedValue, out var dCurrentCulture);
                property.SetValue(instance, dCurrentCulture);
                break;

            case RfcEntityPropertySapTypes.DECIMAL:
                decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out var d);
                property.SetValue(instance, d);
                break;

            case RfcEntityPropertySapTypes.DECIMAL_TR:
                decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out var dTr);
                property.SetValue(instance, dTr);
                break;

            case RfcEntityPropertySapTypes.DECIMAL_COMMAND_SIGN:
                decimal.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    new CultureInfo("en-US"), out var decimalCommandSing);
                property.SetValue(instance, decimalCommandSing);
                break;

            case RfcEntityPropertySapTypes.DECIMAL_COMMAND_SIGN_TR:
                decimal.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    new CultureInfo("tr-TR"), out var decimalCommandSingTr);
                property.SetValue(instance, decimalCommandSingTr);
                break;

            case RfcEntityPropertySapTypes.NUMERIC:
                if (string.IsNullOrWhiteSpace(trimmedValue) || trimmedValue.Length <= 4)
                {
                    int.TryParse(trimmedValue, out var iNumeric);
                    property.SetValue(instance, iNumeric);
                }
                else if (trimmedValue.Length <= 8)
                {
                    double.TryParse(trimmedValue,
                        NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                        new CultureInfo("en-US"), out var nDbNumericEn);
                    property.SetValue(instance, nDbNumericEn);
                }
                else
                {
                    decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out var ndEn);
                    property.SetValue(instance, ndEn);
                }
                break;

            case RfcEntityPropertySapTypes.NUMERIC_TR:
                if (string.IsNullOrWhiteSpace(trimmedValue) || trimmedValue.Length <= 4)
                {
                    int.TryParse(trimmedValue, out var iNumeric);
                    property.SetValue(instance, iNumeric);
                }
                else if (trimmedValue.Length <= 8)
                {
                    double.TryParse(trimmedValue,
                        NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                        new CultureInfo("tr-TR"), out var nDbNumericTr);
                    property.SetValue(instance, nDbNumericTr);
                }
                else
                {
                    decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out var ndTr);
                    property.SetValue(instance, ndTr);
                }
                break;

            case RfcEntityPropertySapTypes.INTEGER:
                int.TryParse(trimmedValue, out var i);
                property.SetValue(instance, i);
                break;

            case RfcEntityPropertySapTypes.BOOLEAN_X:
                bool x = string.Equals(trimmedValue, "X", StringComparison.InvariantCultureIgnoreCase);
                property.SetValue(instance, x);
                break;

            case RfcEntityPropertySapTypes.QUAN_DOUBLE:
                double.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    CultureInfo.CurrentCulture, out var db);
                property.SetValue(instance, db);
                break;

            case RfcEntityPropertySapTypes.QUAN_DOUBLE_US:
                double.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    new CultureInfo("en-US"), out var dbUs);
                property.SetValue(instance, dbUs);
                break;

            default:
                throw new RfcConversionException($"Not Match rfcEntityPropertySapType [{nameof(rfcEntityPropertySapType)}:{rfcEntityPropertySapType}]");
            }
        }
    }
}
