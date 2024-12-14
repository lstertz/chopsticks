namespace Chopsticks.Dependencies
{
    /// <summary>
    /// Specifies how a dependency is resolved across dependents.
    /// </summary>
    public enum DependencyLifetime
    {
        /// <summary>
        /// A new dependency will be instantiated for each container in which 
        /// it is resolved.
        /// </summary>
        /// <remarks>
        /// The dependency, within its container, 
        /// is maintained the same as a dependency with a <see cref="Singleton"/> lifetime.
        /// </remarks>
        Contained,
        /// <summary>
        /// A single dependency will be instantiated and used for resolution 
        /// across all of its dependents, within the scope of its container and 
        /// its child containers (if they permit inheritance from their parent).
        /// </summary>
        /// <remarks>
        /// Singletons are maintained by the container until they are either deregistered 
        /// or the container is disposed, at which time the singleton will also be disposed 
        /// if it is disposable.
        /// </remarks>
        Singleton,
        /// <summary>
        /// A new dependency will be instantiated for each resolution.
        /// </summary>
        /// <remarks>
        /// Transient dependencies are traditionally very temporal with no expectations of 
        /// persistent data or disposability. Any such exceptional expectations should be 
        /// defined on the dependency's contract and handled by the dependent.
        /// </remarks>
        Transient
    }
}
