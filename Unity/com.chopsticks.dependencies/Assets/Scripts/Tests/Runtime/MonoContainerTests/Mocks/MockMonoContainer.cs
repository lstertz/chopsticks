using Chopsticks.Dependencies.Containers;
using NSubstitute;

namespace MonoContainerTests.Mocks
{
    public class MockMonoContainer : 
        BaseMonoContainer<MockDependencyContainer, MockDependencyContainerFactory, 
            MockDependencyContainer.Definition, MockMonoContainerService>
    {
        public new MockDependencyContainer InternalContainer => base.InternalContainer;
        public MockMonoContainerService ContainerService => _containerService;

        public bool HasRegisteredNativeDependencies { get; set; }

        protected override void RegisterNativeDependencies() => 
            HasRegisteredNativeDependencies = true;
    }
}