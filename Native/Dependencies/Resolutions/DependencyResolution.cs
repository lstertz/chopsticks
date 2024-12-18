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
        /// Specifies whether this resolution should be contained within its container, 
        /// which should require it to be copied to any inheriting containers.
        /// </summary>
        public virtual bool IsContained => false;

        /// <summary>
        /// The registration that identifies this resolution as a dependency of a 
        /// <see cref="IDependencyContainer"/>.
        /// </summary>
        public DependencyRegistration Registration { get; protected init; } = new()
        {
            Contract = contract,
        };


        /// <summary>
        /// The factory that provides implementations for resolution.
        /// </summary>
        protected Func<IDependencyContainer, object>? Factory { get; set; } = factory;


        /// <summary>
        /// Performs disposal of all instances managed, either directly 
        /// or indirectly through a factory, by this resolution.
        /// </summary>
        public virtual void Dispose() 
        {
            Factory = null;
        }

        /// <summary>
        /// Performs disposal of any instances that are exclusive to the specified container 
        /// and that are directly managed by this resolution.
        /// </summary>
        /// <param name="container">The container whose exclusive instances are directly managed by 
        /// this resolution will be disposed.</param>
        public virtual void DisposeFor(IDependencyContainer container) { }


        /// <summary>
        /// Provides an implementation to resolve the dependency.
        /// </summary>
        /// <param name="container">The container that will provide any constructed 
        /// implementation's own dependencies.</param>
        /// <returns>An implementation that resolves the dependency, or null 
        /// if this resolution has been disposed of.</returns>
        public abstract object? Get(IDependencyContainer container);
    }
}
