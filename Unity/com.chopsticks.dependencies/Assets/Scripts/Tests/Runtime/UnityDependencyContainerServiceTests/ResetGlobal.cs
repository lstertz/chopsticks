using Chopsticks.Dependencies.Containers;
using MonoContainerTests.Mocks;
using NSubstitute;
using NUnit.Framework;

namespace UnityDependencyContainerServiceTests
{
    public class ResetGlobal
    {
        [Test]
        public void ResetGlobal_WithExistingInstance_DisposesOfInstance()
        {
            // Set up
            var service = new UnityContainerService<MockDependencyContainer, 
                MockDependencyContainerFactory>();
            var container = service.GlobalContainer;

            // Act
            service.ResetGlobal();

            // Assert
            container.Received(1).Dispose();
        }
    }
}