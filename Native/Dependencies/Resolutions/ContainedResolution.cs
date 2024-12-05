using Chopsticks.Dependencies.Containers;
using System;

namespace Chopsticks.Dependencies.Resolutions
{
    /// <inheritdoc/>
    /// <remarks>
    /// This resolution assumes the dependency has a lifetime 
    /// of <see cref="DependencyLifetime.Contained"/>.
    /// </remarks>
    public class ContainedResolution(Type contract,
        Func<IDependencyContainer, object> factory) :
        DependencyResolution(contract, factory)
    {
        private object? _instance;


        /// <inheritdoc/>
        public override void Dispose()
        {
            if (_instance is IDisposable disposable)
                disposable.Dispose();

            _instance = null;
        }

        /// <inheritdoc/>
        public override object Get(IDependencyContainer container) =>
            _instance ??= Factory(container);
    }
}
