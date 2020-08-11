using System;
using SapCo2.Core.Abstract;

namespace SapCo2.Abstract
{
    public interface IReadBapi<T>:IDisposable where T:IRfcBapiOutput
    {
        T GetBapi(IRfcConnection connection, string name);
        T GetBapi<TIn>(IRfcConnection connection, string name, TIn inputParameter) where TIn : class;
    }
}
