using System.Reflection;
using SapCo2.Abstraction.Model;
using SapCo2.Wrapper.Exception;

namespace SapCo2.Extensions
{
    public static class BapiReturnExtensions
    {
        public const string SUCCESS = "S";
        public const string ERROR = "E";
        public const string WARNING = "W";
        public const string INFORMATION = "I";
        public const string ABORT = "A";

        public static void ThrowOnError(this BapiReturnParameter resultCode)
        {
            if (resultCode.MessageType != ERROR && resultCode.MessageType != ABORT)
                return;

            throw new RfcException(resultCode.ToExceptionMessage());
        }

        private static string ToExceptionMessage(this BapiReturnParameter s)
        {
            string message = "";
            PropertyInfo[] properties = s.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                message += $"{propertyInfo.Name}:{propertyInfo.GetValue(s)},";
            }

            message += $"MessageTypeDefinition: {ToMessageTypeString(s.MessageType)}";
            return message;
        }

        private static string ToMessageTypeString(string s)
        {
            switch (s)
            {
            case ERROR:
                return nameof(ERROR);

            case SUCCESS:
                return nameof(SUCCESS);

            case WARNING:
                return nameof(WARNING);

            case INFORMATION:
                return nameof(INFORMATION);

            case ABORT:
                return nameof(ABORT);

            default:
                return string.Empty;
            }
        }
    }
}
