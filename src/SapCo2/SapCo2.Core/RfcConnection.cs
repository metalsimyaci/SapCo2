using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using SapCo2.Core.Abstract;
using SapCo2.Core.Extensions;
using SapCo2.Wrapper.Abstract;

namespace SapCo2.Core
{
    [SuppressMessage("ReSharper", "FlagArgument")]
    public class RfcConnection : RfcConnectionBase
    {
        public RfcConnection(IRfcInterop interop, IOptionsSnapshot<RfcConnectionOption> options):base(interop,options.Value)
        {
        }

        public RfcConnection(IRfcInterop interop, RfcConnectionOption options):base(interop,options)
        {
        }

        public RfcConnection(IRfcInterop interop, string connectionString):base(interop, new RfcConnectionOption().Parse(connectionString))
        {
        }

    }
}
