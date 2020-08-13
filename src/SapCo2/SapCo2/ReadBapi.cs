using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Extensions;
using SapCo2.Wrapper.Abstract;

namespace SapCo2
{
    public sealed class ReadBapi<T> : RfcFunctionBase, IReadBapi<T> where T:IRfcBapiOutput
    {
        public ReadBapi(IRfcInterop interop) : base(interop)
        {
        }

        public T GetBapi(IRfcConnection connection, string name) 
        {
            CreateFunction(connection, name);
            T result= Invoke<T>();
            result.BapiReturn.ThrowOnError();
            return result;
        }
        public T GetBapi<TIn>(IRfcConnection connection, string name, TIn inputParameter) where TIn : class
        {
            CreateFunction(connection, name);
            T result= Invoke<T>(inputParameter);
            result.BapiReturn.ThrowOnError();
            return result;
        }
    }
}
