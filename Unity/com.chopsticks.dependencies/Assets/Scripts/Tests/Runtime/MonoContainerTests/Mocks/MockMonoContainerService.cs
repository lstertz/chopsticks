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

        public MockDependencyContainer FindParentContainer<TUnityContainer, TOverrideContainer>(
            ContainerRetrievalSetting setting, TUnityContainer unityContainer, 
            TOverrideContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<MockDependencyContainer>
            where TOverrideContainer : IUnityContainer<MockDependencyContainer> =>
            Sub.FindParentContainer(setting, unityContainer, overrideContainer);

        public MockDependencyContainer GetContainer<TUnityContainer, TOverrideContainer>(
            ContainerRetrievalSetting setting, bool includeSelf, TUnityContainer unityContainer,
            TOverrideContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<MockDependencyContainer>
            where TOverrideContainer : IUnityContainer<MockDependencyContainer> => 
            Sub.GetContainer(setting, includeSelf, unityContainer, overrideContainer);

        public void ResetGlobal() => 
            Sub.ResetGlobal();
    }
}