using System;

namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Specifies the concept of a dependency to be managed and resolved by 
    /// an <see cref="IDependencyContainer"/>.
    /// </summary>
    public readonly record struct DependencySpecification
    {
        /// <summary>
        /// The type of the contract, representing the dependency, 
        /// that is fulfilled by an implementation.
        /// </summary>
        public required Type Contract { get; init; }

        /// <summary>
        /// The <see cref="DependencyLifetime"/> of the dependency.
        /// </summary>
        public DependencyLifetime Lifetime { get; init; } = DependencyLifetime.Singleton;

        /// <summary>
        /// The factory method that can produce an instance of an implementation to 
        /// resolve this dependency.
        /// </summary>
        public required Func<IDependencyContainer, object> ImplementationFactory { get; init; }


        /// <summary>
        /// Constructs the default specififcation.
        /// </summary>
        public DependencySpecification() { }
    }
}
