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
        });

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
        });

        getLastImplementation = () => lastImplementation;
        return container;
    }

    private static DependencyContainer SetUpMultiRegistrationContainer(DependencyLifetime lifetime,
        out Func<Mock.IContractA?> getLastFirstImplementation,
        out Func<Mock.IContractA?> getLastSecondImplementation,
        out DependencySpecification firstSpecification,
        out DependencySpecification secondSpecification)
    {
        Mock.IContractA? lastFirstImplementation = null;
        Mock.IContractA? lastSecondImplementation = null;

        firstSpecification = new()
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
        };
        secondSpecification = new()
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
        };

        var container = new DependencyContainer();
        container.Register(firstSpecification)
            .Register(secondSpecification);

        getLastFirstImplementation = () => lastFirstImplementation;
        getLastSecondImplementation = () => lastSecondImplementation;
        return container;
    }

    private static DependencyContainer SetUpStandardContainer(DependencyLifetime lifetime,
        out Func<Mock.IContractA?> getLastImplementation, 
        out DependencySpecification specification)
    {
        Mock.IContractA? lastImplementation = null;

        specification = new()
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
        };

        var container = new DependencyContainer();
        container.Register(specification);

        getLastImplementation = () => lastImplementation;
        return container;
    }


    [Test]
    public void Resolve_DeregisteredAllAfterMultiRegistration_OutsNullDependency()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out _, out var firstSpecification, out var secondSpecification);
        container.Deregister(firstSpecification)
            .Deregister(secondSpecification);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
    }

    [Test]
    public void Resolve_DeregisteredAllAfterMultiRegistration_False()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out _, out var firstSpecification, out var secondSpecification);
        container.Deregister(firstSpecification)
            .Deregister(secondSpecification);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_DeregisteredContract_False()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _, 
            out var specification);
        container.Deregister(specification);

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
            out var specification);
        container.Deregister(specification);

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
            out _, out var getLastSecondImplementation, out var firstSpecification, out _);
        container.Deregister(firstSpecification);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(getLastSecondImplementation(), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_DeregisteredFirstAfterMultiRegistration_True()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out _, out var firstSpecification, out _);
        container.Deregister(firstSpecification);

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
            out var getLastFirstImplementation, out _, out _, out var secondSpecification);
        container.Deregister(secondSpecification);

        // Act
        container.Resolve(typeof(Mock.IContractA), out var resolvedImplementation);

        // Assert
        Assert.That(getLastFirstImplementation(), Is.EqualTo(resolvedImplementation));
    }

    [Test]
    public void Resolve_DeregisteredSecondAfterMultiRegistration_True()
    {
        // Set up
        var container = SetUpMultiRegistrationContainer(DependencyLifetime.Singleton,
            out _, out _, out _, out var secondSpecification);
        container.Deregister(secondSpecification);

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
