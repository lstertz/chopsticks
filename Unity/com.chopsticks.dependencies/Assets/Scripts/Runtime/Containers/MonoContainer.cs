using Chopsticks.Dependencies.Resolutions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{

    /// <summary>
    /// Designates that all child GameObject Containers/Dependencies 
    /// are contained within this container. This enables the organization 
    /// of dependencies to be defined through the Unity hierarchy and prefabs.
    /// </summary>
    public class MonoContainer : BaseMonoContainer
    {
        public override bool InheritParentDependencies
        {
            get => InnerContainer.InheritParentDependencies;
            set => InnerContainer.InheritParentDependencies = value;
        }
        [SerializeField]
        private bool _inheritParentDependencies;

        public override IDependencyResolutionProvider Parent
        {
            get => InnerContainer.Parent;
            set => InnerContainer.Parent = value;
        }
        [SerializeField]
        private BaseMonoContainer _overrideParent;


        protected IDependencyContainer InnerContainer { get; private set; }
        protected IDependencyResolutionProvider InnerProvider { get; private set; }


        public void Awake()
        {
            // TODO :: Abstract creation or delegate only to implementations, with 
            //          properties defined in the BaseMonoContainer.

            var innerContainer = new DependencyContainer()
            {
                InheritParentDependencies = _inheritParentDependencies,
                Parent = FindParent()
            };

            InnerContainer = innerContainer;
            InnerProvider = innerContainer;
        }

        protected virtual void SetUp() { }


        public void OnDestroy()
        {
            // Dispose of the container.
        }


        public void OnTransformParentChanged() => 
            InnerContainer.Parent = FindParent();

        private BaseMonoContainer FindParent()
        {
            var parent = _overrideParent ?? GetComponentInParent<BaseMonoContainer>();
            if (parent == null)
            {
                // TODO :: Default to global container.
            }

            return parent;
        }


        public void OnValidate()
        {
            // TODO :: Update for inherit parent dependencies or override parent changing.
        }


        public override IDependencyContainer Deregister(DependencyRegistration registration) => 
            InnerContainer.Deregister(registration);

        public override IDependencyContainer Register(DependencySpecification specification, 
            out DependencyRegistration registration) => 
            InnerContainer.Register(specification, out registration);

        public override bool Resolve(Type dependencyType, out object implementation) => 
            InnerContainer.Resolve(dependencyType, out implementation);

        public override IEnumerable<object> ResolveAll(Type dependencyType) => 
            InnerContainer.ResolveAll(dependencyType);


        public override bool CanProvide(Type contract) => 
            InnerProvider.CanProvide(contract);

        public override DependencyResolution GetResolution(Type contract) => 
            InnerProvider.GetResolution(contract);

        public override IEnumerable<DependencyResolution> GetResolutions(Type contract) => 
            InnerProvider.GetResolutions(contract);

        public override IEnumerable<DependencyResolution> GetResolutionsForDisposal() => 
            InnerProvider.GetResolutionsForDisposal();
    }
}
