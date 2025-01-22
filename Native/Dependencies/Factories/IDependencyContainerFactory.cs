using Chopsticks.Dependencies.Containers;

namespace Chopsticks.Dependencies.Factories
{
    /// <summary>
    /// Manages the construction of dependency containers.
    /// </summary>
    public interface IDependencyContainerFactory<TDependencyContainer>
        where TDependencyContainer : IDependencyContainer
    {
        /// <summary>
        /// Builds a new container.
        /// </summary>
        TDependencyContainer BuildContainer();
    }
}
