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
    /// <typeparam name="TNativeContainer">The type of the native container that manages 
    /// the dependencies of this mono container.</typeparam>
    public abstract class MonoContainer<TNativeContainer, TGlobalContainerProvider> : 
        MonoBehaviour, IDependencyContainer
        where TGlobalContainerProvider : IGlobalContainerProvider<TNativeContainer>, new()
        where TNativeContainer: IDependencyContainer, IDependencyResolutionProvider, IDisposable
    {
        public static IDependencyContainer Global => _globalContainerProvider.Get;
        private static readonly TGlobalContainerProvider _globalContainerProvider = new();

        protected TNativeContainer InternalContainer { get; private set; }


        [SerializeField]
        private bool _inheritParentDependencies;

        [SerializeField]
        private MonoContainer<TNativeContainer, TGlobalContainerProvider> _overrideParent;

        // TODO :: Option to be child of:
        //          None
        //          Global
        //          Hierarchy (with global)
        //          Hierarchy (without global)
        //          Override


        // TODO :: Inspector display features:
        //          Current parent.
        //          Contained MonoDependencies.
        //          Maybe list native dependencies.
        //          Prevent disablement.



        /// <summary>
        /// Initiates <see cref="SetUp"/>, defines parent settings, and calls 
        /// <see cref="RegisterNativeDependencies"/>.
        /// </summary>
        public void Awake()
        {
            InternalContainer = SetUp();
            InternalContainer.Parent = FindParent();
            InternalContainer.InheritParentDependencies = _inheritParentDependencies;

            RegisterNativeDependencies();
        }

        /// <summary>
        /// Sets up the internal container of this mono container.
        /// </summary>
        /// <returns>The container that will internally manage the dependencies 
        /// of this mono container.</returns>
        protected abstract TNativeContainer SetUp();

        /// <summary>
        /// Registers any native dependencies that should be inherent to this container.
        /// </summary>
        /// <remarks>
        /// This is only performed once after <see cref="SetUp"/>.
        /// </remarks>
        protected virtual void RegisterNativeDependencies() { }


        public void OnDestroy() => 
            InternalContainer.Dispose();


        public void OnTransformParentChanged() => 
            InternalContainer.Parent = FindParent();

        private IDependencyResolutionProvider FindParent()
        {
            // TODO :: Clean-up and account for other options.

            var parent = _overrideParent != null ? _overrideParent : 
                GetComponentInParent<MonoContainer<TNativeContainer, TGlobalContainerProvider>>();
            if (parent == null)
                return _globalContainerProvider.Get;

            return parent.InternalContainer;
        }


        public void OnValidate()
        {
            // TODO :: Prevent override parent from forming a chain.
            // TODO :: Update for inherit parent dependencies or override parent changing.
        }


        /// <inheritdoc/>
        public IDependencyContainer Deregister(DependencyRegistration registration)
        {
            InternalContainer.Deregister(registration);
            return this;
        }

        /// <inheritdoc/>
        public IDependencyContainer Register(DependencySpecification specification,
            out DependencyRegistration registration)
        {
            InternalContainer.Register(specification, out registration);
            return this;
        }

        /// <inheritdoc/>
        public bool Resolve(Type dependencyType, out object implementation) => 
            InternalContainer.Resolve(dependencyType, out implementation);

        /// <inheritdoc/>
        public IEnumerable<object> ResolveAll(Type dependencyType) => 
            InternalContainer.ResolveAll(dependencyType);
    }
}
