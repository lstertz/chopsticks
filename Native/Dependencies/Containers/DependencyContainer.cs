using System;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    public class DependencyContainer : IDependencyContainer
    {
        /// <inheritdoc/>
        public bool InheritParentDependencies { get; init; }

        /// <inheritdoc/>
        public IDependencyContainer Parent { get; set; }


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

        }
    }
}
