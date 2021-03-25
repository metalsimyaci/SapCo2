using System.Collections.Generic;
using SapCo2.Models;

namespace SapCo2.Samples.Core.Abstracts
{
    public interface IFunctionMetaDataManager:IPrintable<List<ParameterMetaData>>,IPrintable<List<FieldMetaData>>
    {
        List<ParameterMetaData> GetFunctionMetaData(string functionName);
    }
}
