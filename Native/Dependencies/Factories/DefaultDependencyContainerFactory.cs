using Chopsticks.Dependencies.Containers;

namespace Chopsticks.Dependencies.Factories
{
    /// <inheritdoc/>
    public class DefaultDependencyContainerFactory : 
        IDependencyContainerFactory<DependencyContainer>
    {
        /// <inheritdoc/>
        public DependencyContainer BuildContainer() => new();
    }
}
