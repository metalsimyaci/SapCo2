using System;
using System.Collections.Generic;
using System.Text;
using SapCo2.Wrapper.Enumeration;

namespace SapCo2.Wrapper.Exception
{
    public class RfcConversionException:RfcException
    {
        public RfcConversionException(string message) : base(RfcResultCodes.RFC_CONVERSION_FAILURE,message)
        {
        }

    }
}
