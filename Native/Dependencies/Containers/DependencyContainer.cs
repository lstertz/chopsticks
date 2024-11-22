using System;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    /// <inheritdoc cref="IDependencyContainer"/>
    public class DependencyContainer : IDependencyContainer
    {
        /// <inheritdoc/>
        public bool InheritParentDependencies { get; set; }

        /// <inheritdoc/>
        public IDependencyContainer? Parent { get; set; }


        // TODO :: Track specifications and their factories, and/or resulting instances.


        public DependencyContainer()
        {

        }


        /// <inheritdoc/>
        public bool Contains(Type dependencyType)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        public IDependencyContainer Deregister(DependencySpecification specification)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IDependencyContainer Register(DependencySpecification specification)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        public object AssertiveResolve(Type dependencyType, 
            string? customErrorMessage = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Resolve(Type dependencyType, out object? implementation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<object> ResolveAll(Type dependencyType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ResolveAllSingletons()
        {
            throw new NotImplementedException();
        }
    }
}
