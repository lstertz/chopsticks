using Chopsticks.Dependencies.Resolutions;
using System;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Provides Unity-specific services for working with containers, including 
    /// access to a global instance and strategies to work with other containers.
    /// </summary>
    /// <typeparam name="TNativeContainer">The type of the native container for which 
    /// Unity-specific services are being provided.</typeparam>
    public interface IUnityContainerService<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
    {
        /// <summary>
        /// A static, global instance of a container, for use as a default or 
        /// the highest-level container of a hierarchy of containers.
        /// </summary>
        TNativeContainer GlobalContainer { get; }

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
        TNativeContainer GetContainer<TUnityContainer, TOverrideContainer>(
            ContainerRetrievalSetting setting, bool includeSelf, 
            TUnityContainer unityContainer, TOverrideContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<TNativeContainer>
            where TOverrideContainer : IUnityContainer<TNativeContainer>;

        /// <summary>
        /// Resets the <see cref="GlobalContainer"/>.
        /// </summary>
        void ResetGlobal();
    }
}
