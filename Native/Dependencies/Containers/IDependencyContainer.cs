namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Manages the registration and containment of dependencies, 
    /// providing localized resolution while controlling dependency scope
    /// and accessibility.
    /// </summary>
    public interface IDependencyContainer
    {
        bool InheritParentDependencies { get; }

        IDependencyContainer Parent { get; set; }
    }
}
