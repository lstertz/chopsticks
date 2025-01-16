using Chopsticks.Dependencies.Containers;
using NSubstitute;
using UnityEngine;

namespace MonoContainerTests.Mocks
{
    public class MockMonoContainerService : IUnityContainerService<MockDependencyContainer>
    {
        public IUnityContainerService<MockDependencyContainer> Sub { get; } =
            Substitute.For<IUnityContainerService<MockDependencyContainer>>();

        public MockDependencyContainer GlobalContainer { get; } = 
            Substitute.For<MockDependencyContainer>();

        public MockDependencyContainer GetContainer<TUnityContainer>(
            ContainerRetrievalSetting setting, bool includeSelf, TUnityContainer unityContainer, 
            TUnityContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<MockDependencyContainer> => 
            Sub.GetContainer(setting, includeSelf, unityContainer, overrideContainer);

        public void ResetGlobal() => 
            Sub.ResetGlobal();
    }
}