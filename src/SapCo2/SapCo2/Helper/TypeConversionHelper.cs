using System;
using System.Globalization;
using System.Reflection;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Helper
{
    internal static class TypeConversionHelper
    {
        public static string ConvertToRfcType(object value, RfcDataTypes rfcDataType)
        {
            switch (rfcDataType)
            {
            case RfcDataTypes.String:
            case RfcDataTypes.Byte:
            case RfcDataTypes.Char:
            case RfcDataTypes.Unit:
            case RfcDataTypes.NumericTr:
            case RfcDataTypes.DecimalCommandSign:
            case RfcDataTypes.DecimalCommandSignTr:
            case RfcDataTypes.QuanDouble:
                return value.ToString();

            case RfcDataTypes.Numeric:
                return string.Empty;

            case RfcDataTypes.Date:
                if (DateTime.TryParse(value.ToString(), out var parsedDate))
                    return ((DateTime)value).ToString("dd.MM.yyyy");

                throw new RfcConversionException("Invalid Date [dd.MM.yyyy]");

            case RfcDataTypes.Date8:
                if (DateTime.TryParse(value.ToString(), out var parsedDate8))
                    return ((DateTime)value).ToString("yyyyMMdd");

                throw new RfcConversionException("Invalid Date [yyyyMMdd]");

            case RfcDataTypes.Datetime:
                return string.Empty;

            case RfcDataTypes.DateGdtu:

                if (!DateTime.TryParse(value.ToString(), out var parsedDateGdatu))
                    throw new RfcConversionException("Invalid Date [yyyyMMdd]");

                if (int.TryParse(((DateTime)value).ToString("yyyyMMdd"), out var parsedDateGdatuInt))
                    return (99999999 - parsedDateGdatuInt).ToString();

                throw new RfcConversionException($"Date(yyyyMMdd) not converted integer");

            case RfcDataTypes.Time:
                if (DateTime.TryParse(value.ToString(), out var parsedTime))
                    return ((DateTime)value).ToString("hhmmss");

                throw new RfcConversionException($"Invalid Time [format:hhmmss, value:{value}]");

            case RfcDataTypes.Float:
                if (float.TryParse(value.ToString(), out var f))
                    return ((float)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Float [culture:en-US, value:{value}]");

            case RfcDataTypes.FloatTr:
                if (float.TryParse(value.ToString(), out var fTr))
                    return ((float)value).ToString(new CultureInfo("tr-TR"));

                throw new RfcConversionException($"Invalid Float [culture:tr-TR, value:{value}]");

            case RfcDataTypes.Decimal:
                if (decimal.TryParse(value.ToString(), out var d))
                    return ((decimal)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Decimal [culture:en-US, value:{value}]");

            case RfcDataTypes.DecimalTr:
                if (decimal.TryParse(value.ToString(), out var dTr))
                    return ((decimal)value).ToString(new CultureInfo("tr-TR"));

                throw new RfcConversionException($"Invalid Decimal [culture:tr-TR, value:{value}]");

            case RfcDataTypes.Integer:
                if (int.TryParse(value.ToString(), out var i))
                    return ((int)value).ToString(new CultureInfo("en-US"));

                throw new RfcConversionException($"Invalid Integer [culture:en-US, value:{value}]");

            case RfcDataTypes.BooleanX:
                if (bool.TryParse(value.ToString(), out var b))
                    return b ? "X" : string.Empty;

                throw new RfcConversionException($"Invalid Boolean [value:{value}]");

            default:
                throw new RfcConversionException($"Not Match rfcDataType [{nameof(rfcDataType)}:{rfcDataType}]");

            }
        }

        public static void ConvertFromRfcType(object instance, PropertyInfo property, object value, RfcDataTypes rfcDataType)
        {
            string trimmedValue = value?.ToString().Trim();
            string pattern;

            switch (rfcDataType)
            {
            case RfcDataTypes.String:
            case RfcDataTypes.Char:
            case RfcDataTypes.Byte:
            case RfcDataTypes.Unit:
                property.SetValue(instance, Convert.ChangeType(trimmedValue, property.PropertyType, null));
                break;

            case RfcDataTypes.FixedCharacter:
                property.SetValue(instance, trimmedValue?.TrimStart('0'));
                break;

            case RfcDataTypes.Raw:
                property.SetValue(instance, value);
                break;

            case RfcDataTypes.Date:
                pattern = "dd.MM.yyyy";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedDate);
                property.SetValue(instance, parsedDate);
                break;

            case RfcDataTypes.DateTr:
                pattern = "yyyy-MM-dd";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedDateTr);
                property.SetValue(instance, parsedDateTr);
                break;

            case RfcDataTypes.Date8:
                pattern = "yyyyMMdd";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedDate8);
                property.SetValue(instance, parsedDate8);
                break;

            case RfcDataTypes.DateGdtu:
                pattern = "yyyyMMdd";
                int.TryParse(trimmedValue, out var dateGdtu);
                DateTime.TryParseExact((99999999 - dateGdtu).ToString(), pattern, null, DateTimeStyles.None,
                    out var parsedDateGdtu);
                property.SetValue(instance, parsedDateGdtu);
                break;

            case RfcDataTypes.Datetime:
                property.SetValue(instance, value);
                break;

            case RfcDataTypes.Time:
                pattern = "HHmmss";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedTime);
                property.SetValue(instance, parsedTime);
                break;

            case RfcDataTypes.TimeFull:
                pattern = "HH:mm:ss";
                DateTime.TryParseExact(trimmedValue, pattern, null, DateTimeStyles.None, out var parsedTimeFull);
                property.SetValue(instance, parsedTimeFull);
                break;

            case RfcDataTypes.Float:
                float.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out var f);
                property.SetValue(instance, f);
                break;

            case RfcDataTypes.FloatTr:
                float.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out var fTr);
                property.SetValue(instance, fTr);
                break;

            case RfcDataTypes.DecimalNoCulter:
                decimal.TryParse(trimmedValue, out var dCurrentCulture);
                property.SetValue(instance, dCurrentCulture);
                break;

            case RfcDataTypes.Decimal:
                decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("en-US"), out var d);
                property.SetValue(instance, d);
                break;

            case RfcDataTypes.DecimalTr:
                decimal.TryParse(trimmedValue, NumberStyles.Float, new CultureInfo("tr-TR"), out var dTr);
                property.SetValue(instance, dTr);
                break;

            case RfcDataTypes.DecimalCommandSign:
                decimal.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    new CultureInfo("en-US"), out var decimalCommandSing);
                property.SetValue(instance, decimalCommandSing);
                break;

            case RfcDataTypes.DecimalCommandSignTr:
                decimal.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    new CultureInfo("tr-TR"), out var decimalCommandSingTr);
                property.SetValue(instance, decimalCommandSingTr);
                break;

            case RfcDataTypes.Numeric:
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

            case RfcDataTypes.NumericTr:
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

            case RfcDataTypes.Integer:
                int.TryParse(trimmedValue, out var i);
                property.SetValue(instance, i);
                break;

            case RfcDataTypes.BooleanX:
                bool x = string.Equals(trimmedValue, "X", StringComparison.InvariantCultureIgnoreCase);
                property.SetValue(instance, x);
                break;

            case RfcDataTypes.QuanDouble:
                double.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    CultureInfo.CurrentCulture, out var db);
                property.SetValue(instance, db);
                break;

            case RfcDataTypes.QuanDoubleUs:
                double.TryParse(trimmedValue,
                    NumberStyles.Float | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign,
                    new CultureInfo("en-US"), out var dbUs);
                property.SetValue(instance, dbUs);
                break;

            default:
                throw new RfcConversionException($"Not Match rfcDataType [{nameof(rfcDataType)}:{rfcDataType}]");
            }
        }
    }
}
