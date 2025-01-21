using Chopsticks.Dependencies.Containers;
using NUnit.Framework;

namespace UnityDependencyContainerServiceTests
{
    public class ResetGlobal
    {
        // TODO :: Update after the service is made generic.
        [Test]
        public void ResetGlobal_WithExistingInstance_DisposesOfInstance()
        {
            // Set up
            var service = new UnityDependencyContainerService();
            var container = service.GlobalContainer;

            // Register a dependency to be checked for, to confirm disposal.
            container.Register("Test String Dependency");

            // Act
            service.ResetGlobal();

            // Assert
            Assert.That(container.CanProvide(typeof(string)), Is.False);
        }
    }
}