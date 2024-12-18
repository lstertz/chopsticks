using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace DependencyContainerTests;

public class Dispose
{
    public static class Mock
    {
        public interface IContractA { }

        public interface IContractB { }

        public class ImplementationA : IContractA { }
    }

    // TODO :: Test CanProvide to be false after disposal.
    // TODO :: Test Resolve to be false and out null after disposal.

    public static class SetUp
    {
        public static DependencyContainer ChildContainer(
            out DependencyResolution parentResolution)
        {
            var spec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => new Mock.ImplementationA()
            };

            var parentFactory = Substitute.For<IDependencyResolutionFactory>();
            parentResolution = ConfigureFactoryForSpec(parentFactory, spec);

            var parentContainer = new DependencyContainer(parentFactory);
            parentContainer.Register(spec, out _);

            var childFactory = Substitute.For<IDependencyResolutionFactory>();
            var container = new DependencyContainer(childFactory)
            {
                InheritParentDependencies = true,
                Parent = parentContainer
            };

            return container;
        }

        public static DependencyContainer StandardContainer(
            out DependencyResolution firstResolution,
            out DependencyResolution secondResolution)
        {
            var firstSpec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => Substitute.For<Mock.IContractA>()
            };
            var secondSpec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => Substitute.For<Mock.IContractA>()
            };

            var factory = Substitute.For<IDependencyResolutionFactory>();
            firstResolution = ConfigureFactoryForSpec(factory, firstSpec);
            secondResolution = ConfigureFactoryForSpec(factory, secondSpec);

            var container = new DependencyContainer(factory);
            container
                .Register(firstSpec, out _)
                .Register(secondSpec, out _);

            return container;
        }


        private static DependencyResolution ConfigureFactoryForSpec(
            IDependencyResolutionFactory factory,
            DependencySpecification spec)
        {
            var mockDependency = Substitute.For<Mock.IContractA>();

            var resolution = Substitute.For<DependencyResolution>(
                spec.Contract,
                spec.ImplementationFactory);

            factory.BuildResolutionFor(spec).Returns(_ => resolution);

            return resolution;
        }
    }


    [Test]
    public void Dispose_WithOwnResolutions_DisposesResolutions()
    {
        // Set up
        var container = SetUp.StandardContainer(
            out var firstResolution, out var secondResolution);

        // Act
        container.Dispose();

        // Assert
        Assert.Ignore();
    }

    [Test]
    public void Dispose_WithParentResolutions_DisposesParentResolutionForSelf()
    {
        // Set up
        var container = SetUp.ChildContainer(out var parentResolution);

        // Act
        container.Dispose();

        // Assert
        Assert.Ignore();
    }
}
