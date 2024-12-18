using Chopsticks.Dependencies.Containers;
using System;
using System.Collections.Generic;

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
        /// <inheritdoc/>
        public override bool IsContained => true;

        private readonly Dictionary<IDependencyContainer, object> _instances = new(1);


        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();

            foreach (var instance in _instances.Values)
                if (instance is IDisposable disposable)
                    disposable.Dispose();

            _instances.Clear();
        }

        /// <inheritdoc/>
        public override void DisposeFor(IDependencyContainer container)
        {
            if (_instances.TryGetValue(container, out var instance))
                if (instance is IDisposable disposable)
                    disposable.Dispose();

            _instances.Remove(container);
        }


        /// <inheritdoc/>
        public override object? Get(IDependencyContainer container)
        {
            if (!_instances.TryGetValue(container, out var instance))
            {
                instance = Factory?.Invoke(container);
                if (instance is not null)
                    _instances.Add(container, instance);
            }

            return instance;
        }
    }
}
