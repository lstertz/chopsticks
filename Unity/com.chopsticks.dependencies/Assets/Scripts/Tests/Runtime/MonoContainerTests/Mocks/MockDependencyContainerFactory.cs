using Chopsticks.Dependencies.Factories;

namespace MonoContainerTests.Mocks
{
    public class MockDependencyContainerFactory : 
        IDependencyContainerFactory<MockDependencyContainer>
    {
        public MockDependencyContainer BuildContainer() => 
            NSubstitute.Substitute.For<MockDependencyContainer>();
    }
}