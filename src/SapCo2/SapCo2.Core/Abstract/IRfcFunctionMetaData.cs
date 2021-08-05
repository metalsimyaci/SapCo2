using System;
using System.Collections.Generic;
using SapCo2.Wrapper.Struct;

namespace SapCo2.Core.Abstract
{
    public interface IRfcFunctionMetaData:IDisposable
    {
        List<RfcParameterDescription> GetParameterDescriptions();
        List<RfcFieldDescription> GetFieldDescriptions(IntPtr typeDescriptionHandler);
    }
}
