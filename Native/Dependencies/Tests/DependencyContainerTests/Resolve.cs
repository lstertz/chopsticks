using Chopsticks.Dependencies;
using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using NSubstitute;

namespace DependencyContainerTests;

public class Resolve
{
    public static class Mock
    {
        public interface IContractA { }

        public interface IContractB { }

        public class ImplementationA : IContractA { }
    }

    public static class SetUp
    {
        public static DependencyContainer ChildContainer(DependencyLifetime lifetime,
            out DependencyContainer parentContainer,
            out DependencyResolution parentResolution,
            out DependencyResolution childResolution,
            out DependencySpecification spec)
        {
            spec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => new Mock.ImplementationA(),
                Lifetime = lifetime
            };

            var parentFactory = Substitute.For<IDependencyResolutionFactory>();
            parentResolution = ConfigureFactoryForSpec(parentFactory, spec);

            parentContainer = new DependencyContainer(parentFactory);
            parentContainer.Register(spec, out _);

            var childFactory = Substitute.For<IDependencyResolutionFactory>();
            childResolution = ConfigureFactoryForSpec(childFactory, spec);
            var container = new DependencyContainer(childFactory)
            {
                InheritParentDependencies = true,
                Parent = parentContainer
            };

            return container;
        }

        public static DependencyContainer MultiRegistrationContainer(
            out DependencyResolution firstResolution,
            out DependencyResolution secondResolution,
            out DependencyRegistration firstRegistration,
            out DependencyRegistration secondRegistration)
        {
            var firstSpec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => Substitute.For<Mock.IContractA>(),
                Lifetime = DependencyLifetime.Singleton
            };
            var secondSpec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => Substitute.For<Mock.IContractA>(),
                Lifetime = DependencyLifetime.Singleton
            };

            var factory = Substitute.For<IDependencyResolutionFactory>();
            firstResolution = ConfigureFactoryForSpec(factory, firstSpec);
            secondResolution = ConfigureFactoryForSpec(factory, secondSpec);

            var container = new DependencyContainer(factory);
            container
                .Register(firstSpec, out firstRegistration)
                .Register(secondSpec, out secondRegistration);

            return container;
        }

        public static DependencyContainer StandardContainer(DependencyLifetime lifetime,
            out DependencyRegistration registration,
            out DependencyResolution resolution)
        {
            var spec = new DependencySpecification()
            {
                Contract = typeof(Mock.IContractA),
                ImplementationFactory = _ => new Mock.ImplementationA(),
                Lifetime = lifetime
            };

            var factory = Substitute.For<IDependencyResolutionFactory>();
            resolution = ConfigureFactoryForSpec(factory, spec);

            var container = new DependencyContainer(factory);
            container.Register(spec, out registration);

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
            resolution.IsContained.Returns(spec.Lifetime == DependencyLifetime.Contained);
            resolution.Get(Substitute.For<IDependencyContainer>()).ReturnsForAnyArgs(mockDependency);

            factory.BuildResolutionFor(spec).Returns(_ => resolution);

            return resolution;
        }
    }


    [Test]
    public void Resolve_DeregisteredAllAfterMultiRegistration_False()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out _, out _, out var firstRegistration, out var secondRegistration);
        container.Deregister(firstRegistration)
            .Deregister(secondRegistration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_DeregisteredAllAfterMultiRegistration_OutsNull()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out _, out _, out var firstRegistration, out var secondRegistration);
        container.Deregister(firstRegistration)
            .Deregister(secondRegistration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
    }

    [Test]
    public void Resolve_Deregistered_False()
    {
        // Set up
        var container = SetUp.StandardContainer(DependencyLifetime.Singleton, 
            out var registration, out _);
        container.Deregister(registration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_Deregistered_OutsNullDependency()
    {
        // Set up
        var container = SetUp.StandardContainer(DependencyLifetime.Singleton, 
            out var registration, out var resolution);
        container.Deregister(registration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
        resolution.DidNotReceive();
    }

    [Test]
    public void Resolve_DeregisteredFirstAfterMultiRegistration_OutsSecondResolutionInstance()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out _, out var secondResolution, out var firstRegistration, out _);
        container.Deregister(firstRegistration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(secondResolution.Get(container), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_DeregisteredFirstAfterMultiRegistration_True()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out _, out _, out var firstRegistration, out _);
        container.Deregister(firstRegistration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }

    [Test]
    public void Resolve_DeregisteredSecondAfterMultiRegistration_OutsFirstResolutionInstance()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out var firstResolution, out _, out _, out var secondRegistration);
        container.Deregister(secondRegistration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(firstResolution.Get(container), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_DeregisteredSecondAfterMultiRegistration_True()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out _, out _, out _, out var secondRegistration);
        container.Deregister(secondRegistration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_Inherited_OutsFromParentResolution()
    {
        // Set up
        var container = SetUp.ChildContainer(DependencyLifetime.Singleton,
            out var parentContainer, out var parentResolution, out _, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(parentResolution.Get(parentContainer), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_Inherited_True()
    {
        // Set up
        var container = SetUp.ChildContainer(DependencyLifetime.Singleton,
            out var parentContainer, out var parentResolution, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }

    [Test]
    public void Resolve_OverriddenInheritedRegistration_OutsFromOwnResolution()
    {
        // Set up
        var container = SetUp.ChildContainer(DependencyLifetime.Singleton,
            out var parentContainer, out var parentResolution, out var childResolution, 
            out var spec);
        container.Register(spec, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(childResolution.Get(container), Is.EqualTo(resolvedImplementation));
        parentResolution.DidNotReceive().Get(parentContainer);
    }

    [Test]
    public void Resolve_OverriddenInheritedRegistration_True()
    {
        // Set up
        var container = SetUp.ChildContainer(DependencyLifetime.Singleton,
            out _, out _, out _, out var spec);
        container.Register(spec, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_Registered_OutsFromResolution()
    {
        // Set up
        var container = SetUp.StandardContainer(DependencyLifetime.Contained,
            out _, out var resolution);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(resolution.Get(container), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_Registered_True()
    {
        // Set up
        var container = SetUp.StandardContainer(DependencyLifetime.Contained, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredMultiple_OutsFromFirstResolution()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out var firstResolution, out _, out _, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(firstResolution.Get(container), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_RegisteredMultiple_True()
    {
        // Set up
        var container = SetUp.MultiRegistrationContainer(
            out _, out _, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_Unregistered_False()
    {
        // Set up
        var container = SetUp.StandardContainer(DependencyLifetime.Singleton, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractB), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_Unregistered_OutsNullDependency()
    {
        // Set up
        var container = SetUp.StandardContainer(DependencyLifetime.Singleton, out _, out _);

        // Act
        container.Resolve(typeof(Mock.IContractB), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
    }
}
