using System;

namespace SapCo2.Core.Abstract
{
    public interface IRfcFunction:IDisposable
    {
        /// <summary>
        /// Invokes the remote function.
        /// </summary>
        void Invoke();

        /// <summary>
        /// Invokes the remote function with the given input options.
        /// </summary>
        /// <param name="input">The input options.</param>
        void Invoke(object input);

        /// <summary>
        /// Invokes the remote function and returns the output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output model.</typeparam>
        /// <returns>The output.</returns>
        TOutput Invoke<TOutput>();

        /// <summary>
        /// Invokes the remote function with the given input options and returns the output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output model.</typeparam>
        /// <param name="input">The input options.</param>
        /// <returns>The output.</returns>
        TOutput Invoke<TOutput>(object input);

        /// <summary>
        /// IRfc function
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IRfcFunction CreateFunction(IRfcConnection connection, string name);
    }
}
