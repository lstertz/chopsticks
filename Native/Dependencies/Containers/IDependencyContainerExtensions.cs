using Chopsticks.Dependencies.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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

        // TODO :: Re-evaluate with specifying only contract.

        public static IDependencyContainer Register<TContract>(
            this IDependencyContainer container,
            TContract dependency)
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TContract>(
            this IDependencyContainer container,
            TContract dependency, out DependencyRegistration registration)
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TContract>(
            this IDependencyContainer container,
            Func<IDependencyContainer, TContract> dependencyFactory,
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
        {
            throw new NotImplementedException();
        }

        public static IDependencyContainer Register<TContract>(
            this IDependencyContainer container,
            Func<IDependencyContainer, TContract> dependencyFactory,
            out DependencyRegistration registration,
            DependencyLifetime lifetime = DependencyLifetime.Singleton)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Resolves the dependency of the specified type with the 
        /// first registered implementation.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract is to be resolved.</typeparam>
        /// <param name="container">The container resolving the dependency.</param>
        /// <param name="customErrorMessage">The custom message of the exception 
        /// thrown if the dependency could not be resolved.</param>
        /// <exception cref="MissingDependencyException">Thrown if the specified 
        /// dependency could not be resolved.</exception>
        /// <returns>The resolved dependency.</returns>
        public static TContract AssertiveResolve<TContract>(
            this IDependencyContainer container, string? customErrorMessage = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves the dependency of the specified type with the 
        /// first registered implementation.
        /// </summary>
        /// <param name="container">The container resolving the dependency.</param>
        /// <param name="contract">The type of the contract that is to be resolved.</param>
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
        /// <typeparam name="TContract">The type of the contract is to be resolved.</typeparam>
        /// <param name="container">The container resolving the dependency.</param>
        /// <param name="implementation">The resolving dependency implementation, or null 
        /// if it could not be resolved.</param>
        /// <returns>Whether the dependency was successfully resolved.</returns>
        public static bool Resolve<TContract>(
            this IDependencyContainer container, 
            out TContract? implementation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves the dependency of the specified type with all registered implementations.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract is to be resolved.</typeparam>
        /// <param name="container">The container resolving the dependency.</param>
        /// <returns>The collection of all resolving implementations..</returns>
        public static IEnumerable<TContract> ResolveAll<TContract>(
            this IDependencyContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
