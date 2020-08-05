using System;
using System.Text.RegularExpressions;
using SapCo2.Wrapper.Abstract;
using SapCo2.Wrapper.Enumeration;
using SapCo2.Wrapper.Extension;
using SapCo2.Wrapper.Fields.Abstract;
using SapCo2.Wrapper.Interop;

namespace SapCo2.Wrapper.Fields
{
    internal sealed class TimeField : Field<TimeSpan?>
    {
        private static readonly string ZeroRfcTimeString = new string('0', 6);
        private static readonly string EmptyRfcTimeString = new string(' ', 6);

        public TimeField(string name, TimeSpan? value)
            : base(name, value)
        {
        }

        public override void Apply(IRfcInterop interop, IntPtr dataHandle)
        {
            string stringValue = Value?.ToString("hhmmss") ?? ZeroRfcTimeString;

            RfcResultCodes resultCode = interop.SetTime(
                dataHandle: dataHandle,
                name: Name,
                time: stringValue.ToCharArray(),
                out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);
        }

        public static TimeField Extract(IRfcInterop interop, IntPtr dataHandle, string name)
        {
            char[] buffer = EmptyRfcTimeString.ToCharArray();

            RfcResultCodes resultCode = interop.GetTime(
                dataHandle: dataHandle,
                name: name,
                emptyTime: buffer,
                out RfcErrorInfo errorInfo);

            resultCode.ThrowOnError(errorInfo);

            var timeString = new string(buffer);

            if (timeString == EmptyRfcTimeString || timeString == ZeroRfcTimeString)
                return new TimeField(name, null);

            Match match = Regex.Match(timeString, "^(?<Hours>[0-9]{2})(?<Minutes>[0-9]{2})(?<Seconds>[0-9]{2})$");
            if (!match.Success)
                return new TimeField(name, null);

            int hours = int.Parse(match.Groups["Hours"].Value);
            int minutes = int.Parse(match.Groups["Minutes"].Value);
            int seconds = int.Parse(match.Groups["Seconds"].Value);

            return new TimeField(name, new TimeSpan(hours, minutes, seconds));
        }

        public override string ToString()
            => $"{Name} = {Value:hh:mm:ss}";
    }
}
