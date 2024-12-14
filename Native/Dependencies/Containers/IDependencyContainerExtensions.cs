using Chopsticks.Dependencies.Exceptions;
using System;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    public static class IDependencyContainerExtensions
    {


        public static IDependencyContainer Register(
            this IDependencyContainer container,
            DependencySpecification specification)
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
            TImplementation dependency, out DependencyRegistration registration)
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

        public static IDependencyContainer Register<TImplementation>(
            this IDependencyContainer container,
            Func<IDependencyContainer, TImplementation> dependencyFactory,
            out DependencyRegistration registration,
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation, TContract>(
            this IDependencyContainer container,
            TContract singletonDependency) 
            where TImplementation : TContract
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation, TContract>(
            this IDependencyContainer container,
            TContract singletonDependency, 
            out DependencyRegistration registration)
            where TImplementation : TContract
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation, TContract>(
            this IDependencyContainer container,
            Func<IDependencyContainer, TImplementation> dependencyFactory,
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
             where TImplementation : TContract
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TImplementation, TContract>(
            this IDependencyContainer container,
            Func<IDependencyContainer, TImplementation> dependencyFactory,
            out DependencyRegistration registration,
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
        /// <param name="customErrorMessage">The custom message of the exception 
        /// thrown if the dependency could not be resolved.</param>
        /// <exception cref="MissingDependencyException">Thrown if the specified 
        /// dependency could not be resolved.</exception>
        /// <returns>The resolved dependency.</returns>
        public static TDependency AssertiveResolve<TDependency>(
            this IDependencyContainer container, string? customErrorMessage = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves the dependency of the specified type with the 
        /// first registered implementation.
        /// </summary>
        /// <param name="container">The container resolving the dependency.</param>
        /// <param name="contract">The type of the contract that the implementation resolve, 
        /// as a dependency.</param>
        /// <param name="customErrorMessage">The custom message of the exception 
        /// thrown if the dependency could not be resolved.</param>
        /// <exception cref="MissingDependencyException">Thrown if the specified 
        /// dependency could not be resolved.</exception>
        /// <returns>The resolving dependency implementation.</returns>
        public static object AssertiveResolve(this IDependencyContainer container, Type contract,
            string? customErrorMessage = null)
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
            out TDependency? implementation)
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
