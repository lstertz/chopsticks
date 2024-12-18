using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace SingletonResolutionTests;

public class Dispose
{
    public static class SetUp
    {
        public static SingletonResolution StandardResolution(
            out IDependencyContainer container,
            out Func<IDependencyContainer, object> factory)
        {
            container = Substitute.For<IDependencyContainer>();

            factory = Substitute.For<Func<IDependencyContainer, object>>();
            factory.Invoke(container).Returns(Substitute.For<IDisposable>());

            return new SingletonResolution(typeof(IDisposable), factory);
        }
    }


    [Test]
    public void Dispose_DisposableInstance_DisposesInstance()
    {
        // Set up
        var resolution = SetUp.StandardResolution(out var container, out var factory);
        var implementation = resolution.Get(container) as IDisposable;

        // Act
        resolution.Dispose();

        // Assert
        implementation!.Received().Dispose();
    }
}
