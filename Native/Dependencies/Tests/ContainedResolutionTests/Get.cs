using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace ContainedResolutionTests;

public class Get
{
    public static class Mock
    {
        public class Implementation { }
    }

    public static class SetUp
    {
        public static ContainedResolution StandardResolution(
            out IDependencyContainer containerA,
            out IDependencyContainer containerB,
            out Func<IDependencyContainer, object> factory,
            out Mock.Implementation implementationA,
            out Mock.Implementation implementationB)
        {
            implementationA = new Mock.Implementation();
            implementationB = new Mock.Implementation();

            containerA = Substitute.For<IDependencyContainer>();
            containerB = Substitute.For<IDependencyContainer>();

            factory = Substitute.For<Func<IDependencyContainer, object>>();
            factory.Invoke(containerA).Returns(implementationA);
            factory.Invoke(containerB).Returns(implementationB);

            return new ContainedResolution(typeof(Mock.Implementation), factory);
        }
    }

    [Test]
    public void Get_FirstCallWithContainerA_InstantiatesFromFactory()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var containerA, out _, 
            out var factory, out var implementationA, out _);

        // Act
        var resultingImplementation = resolution.Get(containerA);

        // Assert
        factory.Received().Invoke(containerA);
        Assert.That(resultingImplementation, Is.EqualTo(implementationA));
    }

    [Test]
    public void Get_SecondCallWithContainerA_ReturnsFirstImplementation()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var containerA, out _,
            out var factory, out var implementationA, out _);

        var firstImplementation = resolution.Get(containerA);
        factory.ClearReceivedCalls();

        // Act
        var resultingImplementation = resolution.Get(containerA);

        // Assert
        factory.DidNotReceive().Invoke(containerA);
        Assert.That(resultingImplementation, Is.EqualTo(firstImplementation));
        Assert.That(resultingImplementation, Is.EqualTo(implementationA));
    }

    [Test]
    public void Get_SecondCallWithContainerB_InstantiatesFromFactory()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var containerA, out var containerB,
            out var factory, out var implementationA, out var implementationB);

        var firstImplementation = resolution.Get(containerA);
        factory.ClearReceivedCalls();

        // Act
        var resultingImplementation = resolution.Get(containerB);

        // Assert
        factory.Received().Invoke(containerB);
        Assert.That(resultingImplementation, Is.Not.EqualTo(firstImplementation));
        Assert.That(resultingImplementation, Is.EqualTo(implementationB));
    }
}
