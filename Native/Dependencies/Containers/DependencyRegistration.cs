using System;

namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Represents the registration of a dependency for a specific contract.
    /// </summary>
    public class DependencyRegistration
    {
        /// <summary>
        /// The contract fulfilled by the registered dependency.
        /// </summary>
        public required Type Contract { get; init; }
    }
}
