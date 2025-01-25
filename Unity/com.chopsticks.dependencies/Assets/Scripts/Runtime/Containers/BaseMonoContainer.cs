using Chopsticks.Dependencies.Factories;
using Chopsticks.Dependencies.Resolutions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Manages the organization of MonoBehaviour containers based on the hierarchy of GameObjects.
    /// </summary>
    /// <typeparam name="TNativeContainer">The type of the internal, non-Unity 
    /// dependency container.</typeparam>
    /// <typeparam name="TNativeContainerFactory">The factory to produce the internal, non-Unity 
    /// dependency container.</typeparam>
    /// <typeparam name="TNativeContainerDefinition">The type of definition to define any custom 
    /// properties of the internal, non-Unity dependency container.</typeparam>
    /// <typeparam name="TUnityContainerService">The type of the Unity container service that 
    /// provides Unity-specific services.</typeparam>
    public abstract class BaseMonoContainer<TNativeContainer, TNativeContainerFactory, 
        TNativeContainerDefinition, TUnityContainerService> : 
        BaseUnityContainer<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
        where TNativeContainerFactory : IDependencyContainerFactory<TNativeContainer, 
            TNativeContainerDefinition>, new()
        where TUnityContainerService : IUnityContainerService<TNativeContainer>, new()
    {
        /// <summary>
        /// The global (highest application scope) container for all of the same type 
        /// of MonoContainers, as defined by this container's Unity Container Service.
        /// </summary>
        public static IDependencyContainer Global => _containerService.GlobalContainer;

        protected static readonly TUnityContainerService _containerService = new();
        protected static readonly TNativeContainerFactory _containerFactory = new();


        /// <inheritdoc/>
        protected override TNativeContainer InternalContainer => 
            _internalContainer ??= _containerFactory.BuildContainer(InternalContainerDefinition);
        private TNativeContainer _internalContainer;

        /// <summary>
        /// The definition used, by default, to define the <see cref="InternalContainer"/>.
        /// </summary>
        protected virtual TNativeContainerDefinition InternalContainerDefinition { get; }


        [SerializeField]
        private bool _inheritParentDependencies;

        [SerializeField]
        private BaseUnityContainer<TNativeContainer> _overrideParent;

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
            InternalContainer.InheritParentDependencies = _inheritParentDependencies;
            UpdateParent();

            // TODO :: Check whether children need to update their parent.

            RegisterNativeDependencies();
        }

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
        public override IDependencyContainer Deregister(DependencyRegistration registration)
        {
            InternalContainer.Deregister(registration);
            return this;
        }

        /// <inheritdoc/>
        public override IDependencyContainer Register(DependencySpecification specification,
            out DependencyRegistration registration)
        {
            InternalContainer.Register(specification, out registration);
            return this;
        }

        /// <inheritdoc/>
        public override bool Resolve(Type dependencyType, out object implementation) =>
            InternalContainer.Resolve(dependencyType, out implementation);

        /// <inheritdoc/>
        public override IEnumerable<object> ResolveAll(Type dependencyType) =>
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
