using Chopsticks.Dependencies.Containers;
using NSubstitute;
using System;

namespace MonoContainerTests.Mocks
{
    public class MockGlobalContainerProvider : IGlobalContainerProvider<MockDependencyContainer>
    {
        public MockDependencyContainer Get => Substitute.For<MockDependencyContainer>();

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}