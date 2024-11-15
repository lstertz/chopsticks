using System;

namespace Chopsticks.Dependencies.Containers
{
    public record struct DependencySpecification
    {
        public Type Contract { get; init; }

        public Type Implementation { get; init; }

        public DependencyLifetime Lifetime { get; init; }

        public Func<IDependencyContainer, object?> ImplementationFactory { get; init; }
    }
}
