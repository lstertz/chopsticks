using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace ContainedResolutionTests;

public class Dispose
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
    public void Dispose_DisposableInstances_DisposesAllInstances()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var containerA, out var containerB,
            out var factory);
        var implementationA = resolution.Get(containerA) as IDisposable;
        var implementationB = resolution.Get(containerB) as IDisposable;

        // Act
        resolution.Dispose();

        // Assert
        implementationA!.Received().Dispose();
        implementationB!.Received().Dispose();
    }
}
