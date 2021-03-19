using System;
using System.Threading.Tasks;

namespace SapCo2.Core.Abstract
{
    public interface IRfcFunction:IDisposable
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
        /// Invokes the remote function and returns the output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output model.</typeparam>
        /// <returns>The output.</returns>
        TOutput Invoke<TOutput>();

        /// <summary>
        /// Invokes the remote asynchronous function and returns the output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output model.</typeparam>
        /// <returns>The output.</returns>
        Task<TOutput> InvokeAsync<TOutput>();


        /// <summary>
        /// Invokes the remote function with the given input options and returns the output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output model.</typeparam>
        /// <param name="input">The input options.</param>
        /// <returns>The output.</returns>
        TOutput Invoke<TOutput>(object input);

        /// <summary>
        /// Invokes the remote asynchronous function with the given input options and returns the output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output model.</typeparam>
        /// <param name="input">The input options.</param>
        /// <returns>The output.</returns>
        Task<TOutput> InvokeAsync<TOutput>(object input);

    }
}
