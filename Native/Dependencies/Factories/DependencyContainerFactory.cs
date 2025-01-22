using Chopsticks.Dependencies.Containers;

namespace Chopsticks.Dependencies.Factories
{
    /// <inheritdoc/>
    public class DependencyContainerFactory : IDependencyContainerFactory<DependencyContainer>
    {
        /// <inheritdoc/>
        public DependencyContainer BuildContainer() => new();
    }
}
