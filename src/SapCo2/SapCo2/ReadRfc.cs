using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Wrapper.Abstract;

namespace SapCo2
{
    public sealed class ReadRfc : RfcFunctionBase, IReadRfc
    {
        public ReadRfc(IRfcInterop interop) : base(interop)
        {
        }

        public void ExecuteRfc(IRfcConnection connection, string name)
        {
            using IRfcFunction function = CreateFunction(connection, name);
            function.Invoke();
        }
        public void ExecuteRfc<TIn>(IRfcConnection connection, string name, TIn inputParameter) where TIn : class
        {
            using IRfcFunction function = CreateFunction(connection, name);
            function.Invoke(inputParameter);
        }
        public T GetRfc<T>(IRfcConnection connection, string name) where T : class
        {
            using IRfcFunction function = CreateFunction(connection, name);
            return function.Invoke<T>();
        }
        public T GetRfc<T, TIn>(IRfcConnection connection, string name, TIn inputParameter) where T : class where TIn : class
        {
            using IRfcFunction function = CreateFunction(connection, name);
            return function.Invoke<T>(inputParameter);
        }
    }
}
