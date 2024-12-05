namespace Chopsticks.Dependencies
{
    /// <summary>
    /// Specifies how a dependency is resolved across dependents.
    /// </summary>
    public enum DependencyLifetime
    {
        /// <summary>
        /// A new singleton dependency will be instantiated for each container in which 
        /// it is resolved.
        /// </summary>
        Contained,
        /// <summary>
        /// A single dependency will be instantiated and used for resolution 
        /// across all of its dependents, within the scope of its container.
        /// </summary>
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
