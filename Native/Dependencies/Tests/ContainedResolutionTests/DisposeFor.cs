using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace ContainedResolutionTests;

public class DisposeFor
{
    public static class SetUp
    {
        public static ContainedResolution StandardResolution(
            out IDependencyContainer containerA,
            out IDependencyContainer containerB,
            out Func<IDependencyContainer, object> factory)
        {
            containerA = Substitute.For<IDependencyContainer>();
            containerB = Substitute.For<IDependencyContainer>();

            factory = Substitute.For<Func<IDependencyContainer, object>>();
            factory.Invoke(containerA).Returns(Substitute.For<IDisposable>());
            factory.Invoke(containerB).Returns(Substitute.For<IDisposable>());

            return new ContainedResolution(typeof(IDisposable), factory);
        }
    }


    [Test]
    public void DisposeFor_DisposableContainerInstance_DisposesInstance()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var container, out _, out var factory);
        var implementation = resolution.Get(container) as IDisposable;

        // Act
        resolution.DisposeFor(container);

        // Assert
        implementation!.Received().Dispose();
    }

    [Test]
    public void DisposeFor_DisposableOtherContainerInstance_DoesNotDisposeInstance()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var containerA, out var containerB,
            out var factory);
        var implementationB = resolution.Get(containerB) as IDisposable;

        // Act
        resolution.DisposeFor(containerA);

        // Assert
        implementationB!.DidNotReceive().Dispose();
    }
}
