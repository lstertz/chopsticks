using Chopsticks.Dependencies.Containers;
using NSubstitute;

namespace MonoContainerTests.Mocks
{
    public class MockMonoContainer : 
        BaseMonoContainer<MockDependencyContainer, MockMonoContainerService>
    {
        public new MockDependencyContainer InternalContainer { get; set; } = 
            Substitute.For<MockDependencyContainer>();

        public bool RegisteredNativeDependencies { get; set; }

        protected override MockDependencyContainer SetUp() => 
            InternalContainer;

        protected override void RegisterNativeDependencies() => 
            RegisteredNativeDependencies = true;
    }
}