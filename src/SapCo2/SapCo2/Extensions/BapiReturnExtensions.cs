using System.Collections.Generic;
using System.Linq;
using SapCo2.Parameters;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Extensions
{
    public static class BapiReturnExtensions
    {
        public const string Success = "S";
        public const string Error = "E";
        public const string Warning = "W";
        public const string Information = "I";
        public const string Abort = "A";

        public static void ThrowOnError(this RfcBapiOutputParameter resultCode)
        {
            if (resultCode.MessageType != Error && resultCode.MessageType != Abort)
                return;
            throw new RfcException(resultCode.ToExceptionMessage());

        }

        private static string ToExceptionMessage(this RfcBapiOutputParameter s)
        {
            IEnumerable<string> propertyInfoStrings = Enumerable.Select(s.GetType().GetProperties(),
                propertyInfo => $"{propertyInfo.Name}:{propertyInfo.GetValue(propertyInfo, null)}");
            var message = string.Join(", ", propertyInfoStrings);
            message += $"MessageTypeDefinition: {ToMessageTypeString(s.MessageType)}";

            return message;
        }

        private static string ToMessageTypeString(string s)
        {
            switch (s)
            {
            case Error:
                return nameof(Error);
            case Success:
                return nameof(Success);
            case Warning:
                return nameof(Warning);
            case Information:
                return nameof(Information);
            case Abort:
                return nameof(Abort);
            default:
                return string.Empty;
            }
        }
    }
}
