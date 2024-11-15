using System;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    public static class IDependencyContainerExtensions
    {
        public static bool Contains<TDependency>(this IDependencyContainer container)
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation>(
            this IDependencyContainer container, 
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation>(
            this IDependencyContainer container,
            TImplementation dependency)
        {
            throw new NotImplementedException();
        }
        public static IDependencyContainer Register<TImplementation>(
            this IDependencyContainer container,
            Func<IDependencyContainer, TImplementation> dependencyFactory,
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation, TContract>(
            this IDependencyContainer container,
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
             where TImplementation : TContract
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation, TContract>(
            this IDependencyContainer container,
            TContract dependency) 
            where TImplementation : TContract
        {
            throw new NotImplementedException();
        }
        public static IDependencyContainer Register<TImplementation, TContract>(
            this IDependencyContainer container,
            Func<IDependencyContainer, TContract> dependencyFactory,
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
             where TImplementation : TContract
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves the dependency of the specified type with the 
        /// first registered implementation.
        /// </summary>
        /// <typeparam name="TDependency">The type of the dependency, either as the 
        /// implementation type or the contract type that would be resolved.</typeparam>
        /// <param name="container">The container resolving the dependency.</param>
        /// <param name="implementation">The resolving dependency implementation, or null 
        /// if it could not be resolved.</param>
        /// <returns>Whether the dependency was successfully resolved.</returns>
        public static bool Resolve<TDependency>(
            this IDependencyContainer container, 
            out object? implementation)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Resolves the dependency of the specified type with all registered implementations.
        /// </summary>
        /// <typeparam name="TDependency">The type of the dependency, either as the 
        /// implementation type or the contract type that would be resolved.</typeparam>
        /// <param name="container">The container resolving the dependency.</param>
        /// <returns>The collection of all resolving implementations..</returns>
        public static IEnumerable<TDependency> ResolveAll<TDependency>(
            this IDependencyContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
