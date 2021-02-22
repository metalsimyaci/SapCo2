using System;
using System.Text.RegularExpressions;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Fields.Abstract;
using SapCo2.Wrapper.Interop;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class DateField : Field<DateTime?>
    {
        private static readonly string ZeroRfcDateString = new string('0', 8);
        private static readonly string EmptyRfcDateString = new string(' ', 8);

        public DateField(string name, DateTime? value)
            : base(name, value)
        {
        }

        public override void Apply(IRfcInterop interop, IntPtr dataHandle)
        {
            RfcResultCodes resultCode = interop.SetDate(
                dataHandle: dataHandle,
                name: Name,
                date: (Value?.ToString("yyyyMMdd") ?? ZeroRfcDateString).ToCharArray(),
                errorInfo: out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public static DateField Extract(IRfcInterop interop, IntPtr dataHandle, string name)
        {
            char[] buffer = EmptyRfcDateString.ToCharArray();

            RfcResultCodes resultCode = interop.GetDate(
                dataHandle: dataHandle,
                name: name,
                emptyDate: buffer,
                errorInfo: out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            string dateString = new string(buffer);

            if (dateString == EmptyRfcDateString || dateString == ZeroRfcDateString)
                return new DateField(name, null);

            Match match = Regex.Match(dateString, "^(?<Year>[0-9]{4})(?<Month>[0-9]{2})(?<Day>[0-9]{2})$");
            if (!match.Success)
                return new DateField(name, null);

            int year = int.Parse(match.Groups["Year"].Value);
            int month = int.Parse(match.Groups["Month"].Value);
            int day = int.Parse(match.Groups["Day"].Value);

            return new DateField(name, new DateTime(year, month, day));
        }

        public override string ToString()
            => Value.HasValue ? $"{Name} = {Value:yyyy-MM-dd}" : $"{Name} = No date";
    }
}
