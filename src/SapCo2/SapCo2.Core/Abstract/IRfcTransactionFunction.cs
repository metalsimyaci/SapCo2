using System;
using System.Threading.Tasks;

namespace SapCo2.Core.Abstract
{
    public interface IRfcTransactionFunction:IDisposable
    {
        /// <summary>
        /// Invokes the remote function.
        /// </summary>
        void Invoke();

        /// <summary>
        /// Invokes the remote asynchronous function.
        /// </summary>
        Task<bool> InvokeAsync();

        /// <summary>
        /// Invokes the remote function with the given input options.
        /// </summary>
        /// <param name="input">The input options.</param>
        void Invoke(object input);

        /// <summary>
        /// Invokes the remote asynchronous function with the given input options.
        /// </summary>
        /// <param name="input">The input options.</param>
        Task<bool> InvokeAsync(object input);

        /// <summary>
        /// Returns the output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output model.</typeparam>
        /// <returns>The output.</returns>
        TOutput ReadSubmitResult<TOutput>();
    }
}
