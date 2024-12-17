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
        /// that is fulfilled by the <see cref="Implementation"/>.
        /// </summary>
        /// <remarks>
        /// This may be the same as <see cref="Implementation"/>.
        /// </remarks>
        public required Type Contract { get; init; }

        /// <summary>
        /// The type of the implementation that fulfills this dependency.
        /// </summary>
        public required Type Implementation { get; init; }

        /// <summary>
        /// The <see cref="DependencyLifetime"/> of the dependency.
        /// </summary>
        public DependencyLifetime Lifetime { get; init; } = DependencyLifetime.Singleton;

        /// <summary>
        /// The factory method that can produce an instance of the <see cref="Implementation"/> to 
        /// resolve this dependency.
        /// </summary>
        public required Func<IDependencyContainer, object> ImplementationFactory { get; init; }


        /// <summary>
        /// Constructs the default specififcation.
        /// </summary>
        public DependencySpecification() { }
    }
}
