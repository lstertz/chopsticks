using Chopsticks.Dependencies.Factories;

namespace MonoContainerTests.Mocks
{
    public class MockDependencyContainerFactory : 
        IDependencyContainerFactory<MockDependencyContainer, MockDependencyContainer.Definition>
    {
        public MockDependencyContainer BuildContainer(MockDependencyContainer.Definition def) => 
            NSubstitute.Substitute.For<MockDependencyContainer>();
    }
}