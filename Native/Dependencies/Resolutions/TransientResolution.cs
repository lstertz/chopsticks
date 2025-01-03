using Chopsticks.Dependencies.Containers;
using System;

namespace Chopsticks.Dependencies.Resolutions
{
    /// <inheritdoc/>
    /// <remarks>
    /// This resolution assumes the dependency has a lifetime 
    /// of <see cref="DependencyLifetime.Transient"/>.
    /// </remarks>
    public class TransientResolution(Type contract,
        Func<IDependencyContainer, object?> factory) :
        DependencyResolution(contract, factory)
    {
        /// <inheritdoc/>
        public override object? Get(IDependencyContainer container) => 
            Factory?.Invoke(container);
    }
}
