using System;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Provides services for <see cref="IUnityContainerService{DependencyContainer}"/>, including 
    /// access to a global instance and strategies to work with other containers.
    /// </summary>
    public class UnityDependencyContainerService : IUnityContainerService<DependencyContainer>
    {
        // TODO :: Add locking for thread safety.

        /// <summary>
        /// A static, global instance of a container, for use as a default or 
        /// the highest-level container of a hierarchy of containers.
        /// </summary>
        public DependencyContainer GlobalContainer => _instance ??= new();
        private static DependencyContainer _instance;


        /// <summary>
        /// Finds a dependency container per the specified <see cref="ContainerRetrievalSetting"/>,
        /// starting from the given Unity container.
        /// </summary>
        /// <typeparam name="TUnityContainer">The type of the Unity container from 
        /// which retrieval will start, per some settings.</typeparam>
        /// <typeparam name="TOverrideContainer">The type of the container that may 
        /// be used as an override, per some settings.</typeparam>
        /// <param name="setting">The setting that defines the strategy applied 
        /// to retrieve a container.</param>
        /// <param name="includeSelf">Whether the provided Unity container considers 
        /// itself for some retrieval strategies, particularly when retrieval 
        /// involves a hierarchy.</param>
        /// <param name="unityContainer">The Unity container from which 
        /// retrieval will start, per some settings.</param>
        /// <param name="overrideContainer">The wrapping Unity container of a container 
        /// that may be retrieved per some settings.</param>
        /// <returns>A container retrieved per the specified setting, 
        /// or null if no such container could be found.</returns>
        /// <exception cref="NotSupportedException">Thrown if a  
        /// <see cref="ContainerRetrievalSetting"/>is not supported.</exception>
        public DependencyContainer GetContainer<TUnityContainer, TOverrideContainer>(
            ContainerRetrievalSetting setting, bool includeSelf, 
            TUnityContainer unityContainer, TOverrideContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<DependencyContainer>
            where TOverrideContainer : IUnityContainer<DependencyContainer> =>
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

        /// <summary>
        /// Resets the <see cref="GlobalContainer"/>.
        /// </summary>
        public void ResetGlobal()
        {
            _instance?.Dispose();
            _instance = null;
        }


        private DependencyContainer FindContainerInHierarchy(
            Transform transform, bool defaultToGlobal)
        {
            var container = transform == null ? null :
                    transform.GetComponentInParent<IUnityContainer<DependencyContainer>>();

            if (container == null)
            {
                if (defaultToGlobal)
                    return GlobalContainer;
                return null;
            }

            return container.NativeContainer;
        }
    }
}
