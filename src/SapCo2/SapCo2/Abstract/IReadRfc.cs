using System;
using SapCo2.Core.Abstract;

namespace SapCo2.Abstract
{
    public interface IReadRfc:IDisposable
    {
        void ExecuteRfc(IRfcConnection connection, string name);
        void ExecuteRfc<TIn>(IRfcConnection connection, string name, TIn inputParameter) where TIn : class;
        T GetRfc<T>(IRfcConnection connection, string name) where T : class;
        T GetRfc<T, TIn>(IRfcConnection connection, string name, TIn inputParameter) where T : class where TIn : class;
    }
}
