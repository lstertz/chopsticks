using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace DependencyContainerTests;

public class ResolveAll
{
    public static class Mock
    {
        public interface IContractA { }

        public interface IContractB { }

        public interface IContractC { }
    }

    public static class SetUp
    {
        public static DependencyContainer ChildContainer(
            out DependencyResolution parentResolution1,
            out DependencyResolution parentResolution2,
            out DependencyResolution childResolution)
        {
            var parentSpec1 = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => Substitute.For<Mock.IContractA>()
            };
            var parentSpec2 = new DependencySpecification()  // For verifying exclusion.
            {
                Contract = typeof(Mock.IContractB),
                ImplementationFactory = _ => Substitute.For<Mock.IContractB>()
            };
            var childSpec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => Substitute.For<Mock.IContractA>()
            };

            var parentFactory = Substitute.For<IDependencyResolutionFactory>();
            var parentContainer = new DependencyContainer(parentFactory);
            parentResolution1 = ConfigureFactoryForSpec(parentFactory, 
                parentContainer, parentSpec1);
            parentResolution2 = ConfigureFactoryForSpec(parentFactory, 
                parentContainer, parentSpec2);
            parentContainer.Register(parentSpec1, out _);

            var childFactory = Substitute.For<IDependencyResolutionFactory>();
            var childContainer = new DependencyContainer(childFactory)
            {
                InheritParentDependencies = true,
                Parent = parentContainer
            };
            parentResolution1.Get(childContainer)
                .Returns(parentSpec1.ImplementationFactory(childContainer));
            parentResolution2.Get(childContainer)
                .Returns(parentSpec2.ImplementationFactory(childContainer));
            childResolution = ConfigureFactoryForSpec(childFactory, childContainer, childSpec);

            return childContainer;
        }

        public static DependencyContainer StandardContainer(
            out DependencyResolution firstResolution,
            out DependencyResolution secondResolution,
            out DependencyRegistration firstRegistration,
            out DependencyRegistration secondRegistration)
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
            var thirdSpec = new DependencySpecification()  // For verifying exclusion.
            {
                Contract = typeof(Mock.IContractB),
                ImplementationFactory = _ => Substitute.For<Mock.IContractB>()
            };

            var factory = Substitute.For<IDependencyResolutionFactory>();
            var container = new DependencyContainer(factory);
            firstResolution = ConfigureFactoryForSpec(factory, container, firstSpec);
            secondResolution = ConfigureFactoryForSpec(factory, container, secondSpec);
            ConfigureFactoryForSpec(factory, container, thirdSpec);
            container
                .Register(firstSpec, out firstRegistration)
                .Register(secondSpec, out secondRegistration)
                .Register(thirdSpec, out _);

            return container;
        }


        private static DependencyResolution ConfigureFactoryForSpec(
            IDependencyResolutionFactory factory,
            IDependencyContainer container,
            DependencySpecification spec)
        {
            var mockDependency = Substitute.For<Mock.IContractA>();

            var resolution = Substitute.For<DependencyResolution>(
                spec.Contract,
                spec.ImplementationFactory);
            resolution.Get(container).Returns(mockDependency);

            factory.BuildResolutionFor(spec).Returns(_ => resolution);

            return resolution;
        }
    }


    [Test]
    public void ResolveAll_AfterDispose_ReturnsEmpty()
    {
        // Set up
        var container = SetUp.StandardContainer(out _, out _, out _, out _);
        container.Dispose();

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractA));

        // Assert
        Assert.That(instances, Is.Empty);
    }


    [Test]
    public void ResolveAll_DeregisteredAll_ReturnsEmpty()
    {
        // Set up
        var container = SetUp.StandardContainer(
            out _, out _, out var firstRegistration, out var secondRegistration);
        container.Deregister(firstRegistration)
            .Deregister(secondRegistration);

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractA));

        // Assert
        Assert.That(instances, Is.Empty);
    }

    [Test]
    public void ResolveAll_DeregisteredFirst_ReturnsOnlySecondResolutionInstance()
    {
        // Set up
        var container = SetUp.StandardContainer(
            out _, out var secondResolution, out var firstRegistration, out _);
        container.Deregister(firstRegistration);

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractA)).ToArray();

        // Assert
        Assert.That(instances.Length, Is.EqualTo(1));
        Assert.That(secondResolution.Get(container), Is.EqualTo(instances[0]));
    }

    [Test]
    public void ResolveAll_DeregisteredSecond_ReturnsOnlyFirstResolutionInstance()
    {
        // Set up
        var container = SetUp.StandardContainer(
            out var firstResolution, out _, out _, out var secondRegistration);
        container.Deregister(secondRegistration);

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractA)).ToArray();

        // Assert
        Assert.That(instances.Length, Is.EqualTo(1));
        Assert.That(firstResolution.Get(container), Is.EqualTo(instances[0]));
    }


    [Test]
    public void ResolveAll_Inherited_ReturnsOwnAndParentInstances()
    {
        // Set up
        var container = SetUp.ChildContainer(
            out var parentResolution1, out var parentResolution2, out var childResolution);

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractA)).ToArray();

        // Assert
        Assert.That(instances.Length, Is.EqualTo(3));
        Assert.That(childResolution.Get(container), Is.EqualTo(instances[0]));
        Assert.That(parentResolution1.Get(container), Is.EqualTo(instances[1]));
        Assert.That(parentResolution2.Get(container), Is.EqualTo(instances[2]));
    }

    [Test]
    public void ResolveAll_InheritedAfterDispose_ReturnsOnlyParentInstances()
    {
        // Set up
        var container = SetUp.ChildContainer(
            out var parentResolution1, out var parentResolution2, out var childResolution);
        container.Dispose();

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractA)).ToArray();

        // Assert
        Assert.That(instances.Length, Is.EqualTo(2));
        Assert.That(parentResolution1.Get(container), Is.EqualTo(instances[0]));
        Assert.That(parentResolution2.Get(container), Is.EqualTo(instances[1]));
    }


    [Test]
    public void ResolveAll_Registered_ReturnsInstances()
    {
        // Set up
        var container = SetUp.StandardContainer(out var firstResolution, out var secondResolution,
            out _, out _);

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractA)).ToArray();

        // Assert
        Assert.That(instances.Length, Is.EqualTo(2));
        Assert.That(firstResolution.Get(container), Is.EqualTo(instances[0]));
        Assert.That(secondResolution.Get(container), Is.EqualTo(instances[1]));
    }


    [Test]
    public void ResolveAll_Unregistered_ReturnsEmpty()
    {
        // Set up
        var container = new DependencyContainer();

        // Act
        var instances = container.ResolveAll(typeof(Mock.IContractB));

        // Assert
        Assert.That(instances, Is.Empty);
    }
}
