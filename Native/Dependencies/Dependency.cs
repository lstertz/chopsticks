using Chopsticks.Dependencies.Containers;
using System;

namespace Chopsticks.Dependencies
{
    /// <summary>
    /// Wraps a contract type to accommodate dynamic runtime fulfillment 
    /// for its dependents.
    /// </summary>
    /// <typeparam name="TContract">The type of the contract that 
    /// will be fulfilled as a dependency.</typeparam>
    public class Dependency<TContract>
    {
        /// <summary>
        /// Provides the pre-resolved implementation that fulfills this dependency, 
        /// if it has been resolved.
        /// </summary>
        /// <remarks>
        /// This provides a cached instance of the last resolution, regardless of the 
        /// <see cref="DependencyLifetime"/> of the dependency.
        /// </remarks>
        /// <returns>The pre-resolved implementation, 
        /// or null if it has not been resolved.</returns>
        public TContract? Get() =>
            throw new NotImplementedException();

        /// <summary>
        /// Attempts to provide a pre-resolved implementation that fulfills this dependency, 
        /// otherwise it will attempt to resolve the dependency, 
        /// per its <see cref="DependencyLifetime"/>, 
        /// and to provide any such resolving implementation.
        /// </summary>
        /// <remarks>
        /// This may provide a cached instance of the last resolution, regardless of the 
        /// <see cref="DependencyLifetime"/> of the dependency.
        /// </remarks>
        /// <param name="container">The container from which 
        /// the dependency will be resolved.</param>
        /// <returns>Either the pre-resolved implementation or 
        /// a newly resolved implementation.</returns>
        public TContract GetOrResolve(IDependencyContainer container) => 
            throw new NotImplementedException();
        
        /// <summary>
        /// Attempts to resolve the dependency, per its <see cref="DependencyLifetime"/>,
        /// and to provide any such resolving implementation.
        /// </summary>
        /// <param name="container">The container from which 
        /// the dependency will be resolved.</param>
        /// <returns>The newly resolved implementation.</returns>
        public TContract Resolve(IDependencyContainer container) => 
            throw new NotImplementedException();
    }
}
