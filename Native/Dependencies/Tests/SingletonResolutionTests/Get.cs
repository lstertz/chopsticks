using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace SingletonResolutionTests;

public class Get
{
    public static class Mock
    {
        public class Implementation { }
    }

    public static class SetUp
    {
        public static SingletonResolution StandardResolution(
            out IDependencyContainer container,
            out Func<IDependencyContainer, object> factory,
            out Mock.Implementation implementation)
        {
            implementation = new Mock.Implementation();
            container = Substitute.For<IDependencyContainer>();

            factory = Substitute.For<Func<IDependencyContainer, object>>();
            factory.Invoke(container).Returns(implementation);

            return new SingletonResolution(typeof(Mock.Implementation), factory);
        }
    }

    [Test]
    public void Get_FirstCall_InstantiatesFromFactory()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var container, out var factory, 
            out var implementation);

        // Act
        var resultingImplementation = resolution.Get(container);

        // Assert
        factory.Received().Invoke(container);
        Assert.That(resultingImplementation, Is.EqualTo(implementation));
    }

    [Test]
    public void Get_SecondCall_ReturnsFirstImplementation()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var container, out var factory,
            out var implementation);

        resolution.Get(container);
        factory.ClearReceivedCalls();

        // Act
        var resultingImplementation = resolution.Get(container);

        // Assert
        factory.DidNotReceiveWithAnyArgs().Invoke(container);
        Assert.That(resultingImplementation, Is.EqualTo(implementation));
    }
}
