using Chopsticks.Dependencies.Containers;
using System;

namespace Chopsticks.Dependencies.Resolutions
{
    /// <summary>
    /// Resolves, and maintains resolved implementation instances of, the dependency 
    /// specified by the provided dependency specification.
    /// </summary>
    /// <param name="contract">The contract that this resolution fulfills.</param>
    /// <param name="factory">The factory that provides implementations for resolution.</param>
    public abstract class DependencyResolution(Type contract,
        Func<IDependencyContainer, object> factory) :
        IDisposable
    {
        /// <summary>
        /// The factory that provides implementations for resolution.
        /// </summary>
        protected Func<IDependencyContainer, object> Factory { get; private init; } = factory;
        
        /// <summary>
        /// The registration that identifies this resolution as a dependency of a 
        /// <see cref="IDependencyContainer"/>.
        /// </summary>
        public DependencyRegistration Registration { get; private init; } = new()
        {
            Contract = contract,
        };


        /// <inheritdoc/>
        public abstract void Dispose();

        /// <summary>
        /// Provides an implementation to resolve the dependency.
        /// </summary>
        /// <remarks>
        /// <param name="container">The container that will provide any constructed 
        /// implementation's own dependencies.</param>
        /// <returns>An implementation that resolves the dependency.</returns>
        public abstract object Get(IDependencyContainer container);
    }
}
