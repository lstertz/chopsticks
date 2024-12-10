using Chopsticks.Dependencies.Containers;
using System;

namespace Chopsticks.Dependencies.Resolutions
{
    /// <inheritdoc cref="IDependencyResolutionFactory"/>
    public class DependencyResolutionFactory : IDependencyResolutionFactory
    {
        /// <inheritdoc/>
        public DependencyResolution BuildResolutionFor(DependencySpecification specification) =>
            specification.Lifetime switch
            {
                DependencyLifetime.Contained => new ContainedResolution(
                    specification.Contract, specification.ImplementationFactory),
                DependencyLifetime.Singleton => new SingletonResolution(
                    specification.Contract, specification.ImplementationFactory),
                DependencyLifetime.Transient => new TransientResolution(
                    specification.Contract, specification.ImplementationFactory),
                _ => throw new NotImplementedException($"The lifetime of " +
                    $"{specification.Lifetime} has no matching resolution for this factory.")
            };
    }
}
