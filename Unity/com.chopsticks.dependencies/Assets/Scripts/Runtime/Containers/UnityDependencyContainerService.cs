using Chopsticks.Dependencies.Factories;
using Chopsticks.Dependencies.Resolutions;
using System;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    public class UnityContainerService<TNativeContainer, TNativeContainerFactory> : 
        IUnityContainerService<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
        where TNativeContainerFactory : IDependencyContainerFactory<TNativeContainer>, new()
    {
        // TODO :: Add locking for thread safety.

        /// <inheritdoc/>
        public TNativeContainer GlobalContainer => _instance ??= _instanceFactory.BuildContainer();
        private static TNativeContainer _instance;

        private static TNativeContainerFactory _instanceFactory = new();


        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Thrown if a  
        /// <see cref="ContainerRetrievalSetting"/>is not supported.</exception>
        public TNativeContainer GetContainer<TUnityContainer, TOverrideContainer>(
            ContainerRetrievalSetting setting, bool includeSelf, 
            TUnityContainer unityContainer, TOverrideContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<TNativeContainer>
            where TOverrideContainer : IUnityContainer<TNativeContainer> =>
            setting switch
            {
                ContainerRetrievalSetting.HierarchyWithGlobal =>
                    FindContainerInHierarchy(includeSelf ? unityContainer.transform : 
                        unityContainer.transform.parent, true),
                ContainerRetrievalSetting.HierarchyWithoutGlobal =>
                    FindContainerInHierarchy(includeSelf ? unityContainer.transform :
                        unityContainer.transform.parent, false),
                ContainerRetrievalSetting.Global => 
                    GlobalContainer,
                ContainerRetrievalSetting.Override => 
                    overrideContainer.NativeContainer,
                _ => throw new NotSupportedException($"The container retrieval setting of " +
                                        $"{setting} is not supported."),
            };

        /// <inheritdoc/>
        public void ResetGlobal()
        {
            _instance?.Dispose();
            _instance = default;
        }


        private TNativeContainer FindContainerInHierarchy(
            Transform transform, bool defaultToGlobal)
        {
            var container = transform == null ? null :
                    transform.GetComponentInParent<IUnityContainer<TNativeContainer>>();

            if (container == null)
            {
                if (defaultToGlobal)
                    return GlobalContainer;
                return default;
            }

            return container.NativeContainer;
        }
    }
}
