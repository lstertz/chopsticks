using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace TransientResolutionTests;

public class Get
{
    public static class Mock
    {
        public class Implementation { }
    }

    public static class SetUp
    {
        public static TransientResolution StandardResolution(
            out IDependencyContainer container,
            out Func<IDependencyContainer, object> factory,
            out Func<Mock.Implementation?> getLastImplementation)
        {
            Mock.Implementation? implementation = null;
            container = Substitute.For<IDependencyContainer>();

            factory = Substitute.For<Func<IDependencyContainer, object>>();
            factory.Invoke(container).Returns(_ =>
            {
                implementation = new Mock.Implementation();
                return implementation;
            });

            getLastImplementation = () => implementation;
            return new TransientResolution(typeof(Mock.Implementation), factory);
        }
    }

    [Test]
    public void Get_FirstCall_InstantiatesFromFactory()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var container, out var factory, 
            out var getLastImplementation);

        // Act
        var resultingImplementation = resolution.Get(container);

        // Assert
        factory.Received().Invoke(container);
        Assert.That(resultingImplementation, Is.EqualTo(getLastImplementation()));
    }

    [Test]
    public void Get_SecondCall_InstantiatesFromFactory()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var container, out var factory,
            out var getLastImplementation);

        var firstImplementation = resolution.Get(container);
        factory.ClearReceivedCalls();

        // Act
        var resultingImplementation = resolution.Get(container);

        // Assert
        factory.Received().Invoke(container);
        Assert.That(resultingImplementation, Is.Not.EqualTo(firstImplementation));
        Assert.That(resultingImplementation, Is.EqualTo(getLastImplementation()));
    }
}
