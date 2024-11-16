using System;
using System.Collections;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Manages the registration and containment of dependencies, 
    /// providing localized resolution while controlling dependency scope
    /// and accessibility.
    /// </summary>
    public interface IDependencyContainer
    {
        /// <summary>
        /// Specifies whether this container should inherit all of its parent container's 
        /// registered dependencies.
        /// </summary>
        bool InheritParentDependencies { get; }

        /// <summary>
        /// The parent container of this container.
        /// </summary>
        /// <remarks>
        /// This is null if this container has no parent container.
        /// </remarks>
        IDependencyContainer? Parent { get; set; }


        /// <summary>
        /// Specifies whether this container contains a resolution for the identified dependency.
        /// </summary>
        /// <param name="dependencyType">The type of the dependency, either as the 
        /// implementation or contract type that would be resolved, to be checked for.</param>
        /// <returns>Whether this container contains a resolution 
        /// for the identified dependency.</returns>
        bool Contains(Type dependencyType);

        /// <summary>
        /// Deregisters the dependency defined by the given specification.
        /// </summary>
        /// <param name="specification">The specification that defines the 
        /// dependency to be deregistered.</param>
        /// <returns>This container, to chain additional manipulations.</returns>
        IDependencyContainer Deregister(DependencySpecification specification);

        /// <summary>
        /// Registers the dependency defined by the given specification.
        /// </summary>
        /// <param name="specification">The specification that defines the 
        /// dependency to be registered.</param>
        /// <returns>This container, to chain additional manipulations.</returns>
        IDependencyContainer Register(DependencySpecification specification);

        /// <summary>
        /// Resolves the dependency of the specified type with the 
        /// first registered implementation.
        /// </summary>
        /// <param name="dependencyType">The type of the dependency, either as the 
        /// implementation type or the contract type that would be resolved.</param>
        /// <param name="implementation">The resolving dependency implementation, or null 
        /// if it could not be resolved.</param>
        /// <returns>Whether the dependency was successfully resolved.</returns>
        bool Resolve(Type dependencyType, out object? implementation);


        /// <summary>
        /// Resolves the dependency of the specified type with all registered implementations.
        /// </summary>
        /// <param name="dependencyType">The type of the dependency, either as the 
        /// implementation type or the contract type that would be resolved.</param>
        /// <returns>The collection of all resolving implementations..</returns>
        IEnumerable<object> ResolveAll(Type dependencyType);

        /// <summary>
        /// Resolves all dependencies that will be singletons within the scope of this container, 
        /// which will include those registered with a lifetime of either
        /// <see cref="DependencyLifetime.Singleton"/> or 
        /// <see cref="DependencyLifetime.Contained"/>.
        /// </summary>
        void ResolveAllSingletons();
    }
}
