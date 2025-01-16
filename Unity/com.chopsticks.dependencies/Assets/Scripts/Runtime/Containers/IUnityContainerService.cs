using Chopsticks.Dependencies.Resolutions;
using System;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    public interface IUnityContainerService<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
    {
        TNativeContainer GlobalContainer { get; }

        TNativeContainer GetContainer<TUnityContainer>(ContainerRetrievalSetting setting, 
            bool includeSelf, TUnityContainer unityContainer, TUnityContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<TNativeContainer>;

        void ResetGlobal();
    }
}
