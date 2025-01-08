using Chopsticks.Dependencies.Resolutions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    public abstract class BaseMonoContainer : MonoBehaviour, 
        IDependencyContainer, IDependencyResolutionProvider
    {
        public abstract bool InheritParentDependencies { get; set; }
        public abstract IDependencyResolutionProvider Parent { get; set; }

        public abstract IDependencyContainer Deregister(DependencyRegistration registration);
        public abstract IDependencyContainer Register(DependencySpecification specification, 
            out DependencyRegistration registration);
        public abstract bool Resolve(Type contract, out object implementation);
        public abstract IEnumerable<object> ResolveAll(Type contract);

        public abstract bool CanProvide(Type contract);
        public abstract DependencyResolution GetResolution(Type contract);
        public abstract IEnumerable<DependencyResolution> GetResolutions(Type contract);
        public abstract IEnumerable<DependencyResolution> GetResolutionsForDisposal();
    }
}
