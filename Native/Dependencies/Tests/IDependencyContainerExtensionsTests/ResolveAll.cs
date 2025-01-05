using Chopsticks.Dependencies.Containers;
using NSubstitute;

namespace IDependencyContainerExtensionsTests;

public class ResolveAll
{
    public static class Mock
    {
        public interface IContract { }
    }


    [Test]
    public void ResolveAll_GenericCall_EnforcesGenericTyping()
    {
        // Set up
        var implementationA = Substitute.For<Mock.IContract>();
        var implementationB = Substitute.For<Mock.IContract>();
        IEnumerable<object> expectedImplementations = [
            implementationA,
            implementationB
            ];

        var container = Substitute.For<IDependencyContainer>();
        container.ResolveAll(typeof(Mock.IContract)).Returns(expectedImplementations);

        // Act
        var implementations = container.ResolveAll<Mock.IContract>().ToArray();

        // Assert
        container.Received(1).ResolveAll(typeof(Mock.IContract));
        Assert.That(implementations, Is.EquivalentTo(expectedImplementations));
    }
}
