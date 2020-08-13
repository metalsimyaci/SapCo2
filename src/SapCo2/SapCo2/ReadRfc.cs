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
            CreateFunction(connection, name);
            Invoke();
        }
        public void ExecuteRfc<TIn>(IRfcConnection connection, string name, TIn inputParameter) where TIn : class
        {
            CreateFunction(connection, name);
            Invoke(inputParameter);
        }
        public T GetRfc<T>(IRfcConnection connection, string name) where T : class
        {
            CreateFunction(connection, name);
            return Invoke<T>();
        }
        public T GetRfc<T, TIn>(IRfcConnection connection, string name, TIn inputParameter) where T : class where TIn : class
        {
            CreateFunction(connection, name);
            return Invoke<T>(inputParameter);
        }
    }
}
