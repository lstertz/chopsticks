using Chopsticks.Dependencies;
using Chopsticks.Dependencies.Containers;

namespace DependencyContainerTests;

public class Resolve
{
    private static class Mock
    {
        public interface IContractA { }

        public interface IContractB { }

        public class ImplementationA : IContractA { }
    }


    private static DependencyContainer SetUpChildContainer(DependencyLifetime lifetime,
        out DependencyContainer parentContainer,
        out Func<Mock.IContractA?> getLastImplementation)
    {
        Mock.IContractA? lastImplementation = null;

        parentContainer = new DependencyContainer();
        parentContainer.Register(new()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ =>
            {
                var implementation = new Mock.ImplementationA();
                lastImplementation = implementation;
                return implementation;
            },
            Lifetime = lifetime
        }, out _);

        var container = new DependencyContainer()
        {
            InheritParentDependencies = true,
            Parent = parentContainer
        };

        getLastImplementation = () => lastImplementation;
        return container;
    }

    private static DependencyContainer SetUpImplementationAsContractContainer(
        DependencyLifetime lifetime, out Func<Mock.IContractA?> getLastImplementation)
    {
        Mock.IContractA? lastImplementation = null;

        var container = new DependencyContainer();
        container.Register(new()
        {
            Contract = typeof(Mock.ImplementationA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ =>
            {
                var implementation = new Mock.ImplementationA();
                lastImplementation = implementation;
                return implementation;
            },
            Lifetime = lifetime
        }, out _);

        getLastImplementation = () => lastImplementation;
        return container;
    }

    private static DependencyContainer SetUpMultiRegistrationContainer(DependencyLifetime lifetime,
        out Func<Mock.IContractA?> getLastFirstImplementation,
        out Func<Mock.IContractA?> getLastSecondImplementation,
        out DependencyRegistration firstRegistration,
        out DependencyRegistration  secondRegistration)
    {
        Mock.IContractA? lastFirstImplementation = null;
        Mock.IContractA? lastSecondImplementation = null;

        var container = new DependencyContainer();
        container.Register(
            new()
            {
                Contract = typeof(Mock.IContractA),
                Implementation = typeof(Mock.ImplementationA),
                ImplementationFactory = _ =>
                {
                    var implementation = new Mock.ImplementationA();
                    lastFirstImplementation = implementation;
                    return implementation;
                },
                Lifetime = lifetime
            }, out firstRegistration)
            .Register(new()
            {
                Contract = typeof(Mock.IContractA),
                Implementation = typeof(Mock.ImplementationA),
                ImplementationFactory = _ =>
                {
                    var implementation = new Mock.ImplementationA();
                    lastSecondImplementation = implementation;
                    return implementation;
                },
                Lifetime = lifetime
            }, out secondRegistration);

        getLastFirstImplementation = () => lastFirstImplementation;
        getLastSecondImplementation = () => lastSecondImplementation;
        return container;
    }

    private static DependencyContainer SetUpStandardContainer(DependencyLifetime lifetime,
        out Func<Mock.IContractA?> getLastImplementation, 
        out DependencyRegistration registration)
    {
        Mock.IContractA? lastImplementation = null;

        var container = new DependencyContainer();
        container.Register(new()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ =>
            {
                var implementation = new Mock.ImplementationA();
                lastImplementation = implementation;
                return implementation;
            },
            Lifetime = lifetime
        }, out registration);

        getLastImplementation = () => lastImplementation;
        return container;
    }


    [Test]
    public void Resolve_DeregisteredAllAfterMultiRegistration_False()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out _, out var firstRegistration, out var secondRegistration);
        container.Deregister(firstRegistration)
            .Deregister(secondRegistration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_DeregisteredAllAfterMultiRegistration_OutsNullDependency()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
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
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _, 
            out var registration);
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
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _,
            out var registration);
        container.Deregister(registration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
    }

    [Test]
    public void Resolve_DeregisteredFirstAfterMultiRegistration_OutsSecondRegistrationInstance()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out var getLastSecondImplementation, out var firstRegistration, out _);
        container.Deregister(firstRegistration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(getLastSecondImplementation(), Is.Not.Null);
        Assert.That(getLastSecondImplementation(), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_DeregisteredFirstAfterMultiRegistration_True()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
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
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out var getLastFirstImplementation, out _, out _, out var secondRegistration);
        container.Deregister(secondRegistration);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(getLastFirstImplementation(), Is.Not.Null);
        Assert.That(getLastFirstImplementation(), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_DeregisteredSecondAfterMultiRegistration_True()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out _, out _, out var secondRegistration);
        container.Deregister(secondRegistration);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_InheritedContained_OutsOwnInstance()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Contained,
            out var parentContainer, out var getLastImplementation);

        // Act
        parentContainer.Resolve(typeof(Mock.IContractA), out var resolvedChildImplementation);
        container.Resolve(typeof(Mock.IContractA), out var resolvedParentImplementation);

        // Assert
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedChildImplementation));
        Assert.That(resolvedChildImplementation, Is.Not.EqualTo(resolvedParentImplementation));
    }

    [Test]
    public void Resolve_InheritedContained_True()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Contained, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }

    [Test]
    public void Resolve_InheritedSingleton_OutsParentInstance()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Singleton, 
            out var parentContainer, out var getLastImplementation);

        // Act
        parentContainer.Resolve(typeof(Mock.IContractA), out var resolvedChildImplementation);
        container.Resolve(typeof(Mock.IContractA), out var resolvedParentImplementation);

        // Assert
        Assert.That(getLastImplementation(), Is.Not.Null);
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedChildImplementation));
        Assert.That(resolvedChildImplementation, Is.EqualTo(resolvedParentImplementation));
    }

    [Test]
    public void Resolve_InheritedSingleton_True()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Singleton, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }

    [Test]
    public void Resolve_InheritedTransient_OutsNewInstance()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Singleton,
            out var parentContainer, out var getLastImplementation);

        // Act
        parentContainer.Resolve(typeof(Mock.IContractA), out var resolvedChildImplementation);
        container.Resolve(typeof(Mock.IContractA), out var resolvedParentImplementation);

        // Assert
        Assert.That(getLastImplementation(), Is.Not.Null);
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedChildImplementation));
        Assert.That(resolvedChildImplementation, Is.Not.EqualTo(resolvedParentImplementation));
    }

    [Test]
    public void Resolve_InheritedTransient_True()
    {
        // Set up
        var container = SetUpChildContainer(DependencyLifetime.Transient, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredContained_OutsSameInstanceEveryCall()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Contained,
            out var getLastImplementation, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementationA);
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementationB);

        // Assert
        Assert.That(getLastImplementation(), Is.Not.Null);
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedImplementationA));
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedImplementationB));
    }

    [Test]
    public void Resolve_RegisteredContained_True()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Contained, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredMultiple_OutsFirstRegistrationInstance()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton, 
            out var getLastFirstImplementation, out _, out _, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(getLastFirstImplementation(), Is.Not.Null);
        Assert.That(getLastFirstImplementation(), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_RegisteredMultiple_True()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out _, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredImplementationAsContract_OutsImplementationDependency()
    {
        // Set up
        var container = SetUpImplementationAsContractContainer(DependencyLifetime.Singleton,
            out var getLastImplementation);

        // Act
        container.Resolve(typeof(Mock.ImplementationA), out var resolvedImplementation);

        // Assert
        Assert.That(getLastImplementation(), Is.Not.Null);
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_RegisteredImplementationAsContract_True()
    {
        // Set up
        var container = SetUpImplementationAsContractContainer(DependencyLifetime.Singleton, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.ImplementationA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredSingleton_OutsSameInstanceEveryCall()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton,
            out var getLastImplementation, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementationA);
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementationB);

        // Assert
        Assert.That(getLastImplementation(), Is.Not.Null);
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedImplementationA));
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedImplementationB));
    }

    [Test]
    public void Resolve_RegisteredSingleton_True()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }


    [Test]
    public void Resolve_RegisteredTransient_OutsNewInstanceEveryCall()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Transient,
            out var getLastImplementation, out _);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementationA);
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementationB);

        // Assert
        Assert.That(getLastImplementation(), Is.Not.Null);
        Assert.That(getLastImplementation(), Is.Not.EqualTo(resolvedImplementationA));
        Assert.That(getLastImplementation(), Is.EqualTo(resolvedImplementationB));
    }

    [Test]
    public void Resolve_RegisteredTransient_True()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Transient, out _, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

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

    // TODO :: Add tests for a first implementation resolving after a second registration.
}
