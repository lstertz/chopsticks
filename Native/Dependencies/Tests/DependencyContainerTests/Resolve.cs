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


    private static DependencyResolution ConfigureFactoryForSpec(
        IDependencyResolutionFactory factory,
        DependencySpecification spec)
    {
        var mockDependency = Substitute.For<Mock.IContractA>();

        var resolution = Substitute.For<DependencyResolution>(
            spec.Contract,
            spec.ImplementationFactory);
        resolution.IsContained.Returns(spec.Lifetime == DependencyLifetime.Contained);

        factory.BuildResolutionFor(spec).ReturnsForAnyArgs(_ => resolution);

        return resolution;
    }

    private static DependencyContainer SetUpChildContainer(DependencyLifetime lifetime,
        out DependencyContainer parentContainer,
        out DependencyResolution parentResolution, 
        out DependencyResolution childResolution,
        out DependencySpecification spec)
    {
        spec = new DependencySpecification()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
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

    private static DependencyContainer SetUpImplementationAsContractContainer(
        out DependencyResolution resolution)
    {
        var spec = new DependencySpecification()
        {
            Contract = typeof(Mock.ImplementationA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new Mock.ImplementationA(),
            Lifetime = DependencyLifetime.Singleton
        };

        var factory = Substitute.For<IDependencyResolutionFactory>();
        resolution = ConfigureFactoryForSpec(factory, spec);

        var container = new DependencyContainer(factory);
        container.Register(spec, out _);

        return container;
    }

    private static DependencyContainer SetUpMultiRegistrationContainer(
        out DependencyResolution firstResolution,
        out DependencyResolution secondResolution,
        out DependencyRegistration firstRegistration,
        out DependencyRegistration  secondRegistration)
    {
        var firstSpec = new DependencySpecification()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => Substitute.For<Mock.IContractA>(),
            Lifetime = DependencyLifetime.Singleton
        };
        var secondSpec = new DependencySpecification()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
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

    private static DependencyContainer SetUpStandardContainer(DependencyLifetime lifetime,
        out DependencyRegistration registration,
        out DependencyResolution resolution)
    {
        var spec = new DependencySpecification()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new Mock.ImplementationA(),
            Lifetime = lifetime
        };

        var factory = Substitute.For<IDependencyResolutionFactory>();
        resolution = ConfigureFactoryForSpec(factory, spec);

        var container = new DependencyContainer(factory);
        container.Register(spec, out registration);

        return container;
    }


    [Test]
    public void Resolve_DeregisteredAllAfterMultiRegistration_False()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(
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
        var container = SetUpMultiRegistrationContainer(
            out _, out _, out var firstRegistration, out var secondRegistration);
        container.Deregister(firstRegistration)
            .Deregister(secondRegistration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
    }

    [Test]
    public void Resolve_DeregisteredContract_False()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, 
            out var registration, out _);
        container.Deregister(registration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_DeregisteredContract_OutsNullDependency()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, 
            out var registration, out var resolution);
        container.Deregister(registration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
        resolution.DidNotReceive();
    }

    [Test]
    public void Resolve_DeregisteredFirstAfterMultiRegistration_OutsSecondRegistrationInstance()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(
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
        var container = SetUpMultiRegistrationContainer(
            out _, out _, out var firstRegistration, out _);
        container.Deregister(firstRegistration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }

    [Test]
    public void Resolve_DeregisteredSecondAfterMultiRegistration_OutsFirstRegistrationInstance()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(
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
        var container = SetUpMultiRegistrationContainer(
            out _, out _, out _, out var secondRegistration);
        container.Deregister(secondRegistration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_InheritedContained_OutsFromOwnResolution()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Contained,
            out var parentContainer, out var parentResolution, out var childResolution, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(childResolution.Get(container), Is.EqualTo(resolvedImplementation));
        parentResolution.DidNotReceive().Get(parentContainer);
    }

    [Test]
    public void Resolve_InheritedContained_True()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Contained, 
            out _, out _, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }

    [Test]
    public void Resolve_InheritedNonContained_OutsFromParentResolution()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Singleton,
            out var parentContainer, out var parentResolution, out _, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(parentResolution.Get(parentContainer), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_InheritedNonContained_True()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Singleton,
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
        var container = SetUpChildContainer(DependencyLifetime.Singleton,
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
        var container = SetUpChildContainer(DependencyLifetime.Singleton,
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
        var container = SetUpStandardContainer(DependencyLifetime.Contained,
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
        var container = SetUpStandardContainer(DependencyLifetime.Contained, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredMultiple_OutsFromFirstResolution()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(
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
        var container = SetUpMultiRegistrationContainer(
            out _, out _, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredImplementationAsContract_OutsFromResolution()
    {
        // Set up
        var container = SetUpImplementationAsContractContainer(out var resolution);

        // Act
        container.Resolve(typeof(Mock.ImplementationA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Not.Null);
        Assert.That(resolution.Get(container), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_RegisteredImplementationAsContract_True()
    {
        // Set up
        var container = SetUpImplementationAsContractContainer(out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.ImplementationA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_UnregisteredContract_False()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractB), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_UnregisteredContract_OutsNullDependency()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _, out _);

        // Act
        container.Resolve(typeof(Mock.IContractB), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
    }


    // TODO :: Add tests for a second implementation resolving after a first registration 
    // is deregistered in a multi-registration scenario.
}
