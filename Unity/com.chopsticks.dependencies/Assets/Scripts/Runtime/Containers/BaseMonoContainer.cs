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
    public abstract class BaseMonoContainer<TNativeContainer, TUnityContainerService> : 
        MonoBehaviour, IDependencyContainer, IUnityContainer<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
        where TUnityContainerService : IUnityContainerService<TNativeContainer>, new()
    {
        public static IDependencyContainer Global => _containerService.GlobalContainer;
        private static readonly TUnityContainerService _containerService = new();


        TNativeContainer IUnityContainer<TNativeContainer>.NativeContainer => InternalContainer;
        protected TNativeContainer InternalContainer { get; private set; }



        [SerializeField]
        private bool _inheritParentDependencies;

        [SerializeField]
        private BaseMonoContainer<TNativeContainer, TUnityContainerService> _overrideParent;

        [SerializeField]
        private ContainerParentSetting _containerParentSetting = 
            ContainerParentSetting.HierarchyWithGlobal;

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
            InternalContainer.InheritParentDependencies = _inheritParentDependencies;
            UpdateParent();

            // TODO :: Check whether children need to update their parent.

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
            UpdateParent();
            

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


        private void UpdateParent()
        {
            if (_containerParentSetting == ContainerParentSetting.None)
            {
                InternalContainer.Parent = null;
                return;
            }

            InternalContainer.Parent = _containerService.GetContainer(
                (ContainerRetrievalSetting)_containerParentSetting, false, this, _overrideParent);
        }
    }
}
