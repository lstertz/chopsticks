using Chopsticks.Dependencies;
using Chopsticks.Dependencies.Containers;

namespace DependencyContainerTests;

// TODO :: Write tests for Resolve methods.
// Make sure to account for changes through deregistration and re-registration, and multi-registration.

// TODO :: Write tests for Disposal.

public class Resolve
{
    private static class Mock
    {
        public interface IContractA { }

        public interface IContractB { }

        public class ImplementationA : IContractA { }
    }


    private static DependencyContainer SetUpChildContainer(DependencyLifetime lifetime,
        out Func<Mock.IContractA?> getLastImplementation)
    {
        // TODO :: Update as a parent container of a child container to be returned.

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
        });

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

    private static DependencyContainer SetUpStandardContainer(DependencyLifetime lifetime,
        out Func<Mock.IContractA?> getLastImplementation)
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
        });

        getLastImplementation = () => lastImplementation;
        return container;
    }


    // TODO :: Hold off on deregistration tests until dergistration is updated.

    [Test]
    public void Resolve_DeregisteredContract_False()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_DeregisteredContract_OutsNullDependency()
    {
        Assert.Ignore();
    }

    // TODO :: Add tests for a new implementation resolving after a deregistration.

    [Test]
    public void Resolve_InheritedContained_OutsOwnInstance()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_InheritedContained_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_InheritedSingleton_OutsParentInstance()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_InheritedSingleton_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_InheritedTransient_OutsNewInstance()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_InheritedTransient_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredContained_OutsSameInstanceEveryCall()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Contained,
            out var getLastImplementation);

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
        var container = SetUpStandardContainer(DependencyLifetime.Contained, out _);

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
            out var getLastImplementation);

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
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _);

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
            out var getLastImplementation);

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
        var container = SetUpStandardContainer(DependencyLifetime.Transient, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractA), out _);

        // Assert
        Assert.That(resolved, Is.True);
    }

    [Test]
    public void Resolve_UnregisteredContract_False()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _);

        // Act
        var resolved = container.Resolve(typeof(Mock.IContractB), out _);

        // Assert
        Assert.That(resolved, Is.False);
    }

    [Test]
    public void Resolve_UnregisteredContract_OutsNullDependency()
    {
        // Set up
        var container = SetUpStandardContainer(DependencyLifetime.Singleton, out _);

        // Act
        container.Resolve(typeof(Mock.IContractB), out var resolvedImplementation);

        // Assert
        Assert.That(resolvedImplementation, Is.Null);
    }

    // TODO :: Add tests for a first implementation resolving after a second registration.
}
