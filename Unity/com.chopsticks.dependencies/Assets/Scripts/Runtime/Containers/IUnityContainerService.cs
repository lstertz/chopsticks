using Chopsticks.Dependencies.Resolutions;
using System;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    public interface IUnityContainerService<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
    {
        TNativeContainer GlobalContainer { get; }

        TNativeContainer GetContainer<TUnityContainer, TOverrideContainer>(
            ContainerRetrievalSetting setting, bool includeSelf, 
            TUnityContainer unityContainer, TOverrideContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<TNativeContainer>
            where TOverrideContainer : IUnityContainer<TNativeContainer>;

        void ResetGlobal();
    }
}
