using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;

namespace DependencyContainerTests;

public class CanProvide
{
    private static class Mock
    {
        public interface IContractA { }

        public interface IContractB { }

        public class ImplementationA : IContractA { }
    }


    [Test]
    public void Contains_Deregistered_False()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec, out var registration)
            .Deregister(registration);
        var contains = (container as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void Contains_DeregisteredFirstAfterMultipleRegistrations_True()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec1 = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };
        DependencySpecification spec2 = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec1, out var registration1)
            .Register(spec2, out _)
            .Deregister(registration1);
        var contains = (container as IDependencyResolutionProvider).CanProvide(spec2.Contract);

        // Assert
        Assert.That(contains, Is.True);
    }

    [Test]
    public void Contains_DeregisteredMultipleAfterMultipleRegistrations_False()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec1 = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };
        DependencySpecification spec2 = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec1, out var registration1)
            .Register(spec2, out var registration2)
            .Deregister(registration1)
            .Deregister(registration2);
        var contains = (container as IDependencyResolutionProvider).CanProvide(spec2.Contract);

        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void Contains_DeregisteredSecondAfterMultipleRegistrations_True()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec1 = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };
        DependencySpecification spec2 = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec1, out _)
            .Register(spec2, out var registration2)
            .Deregister(registration2);
        var contains = (container as IDependencyResolutionProvider).CanProvide(spec1.Contract);

        // Assert
        Assert.That(contains, Is.True);
    }


    [Test]
    public void Contains_InheritanceEnabledAndAvailable_True()
    {
        // Set up
        DependencyContainer parentContainer = new();
        DependencyContainer childContainer = new()
        {
            InheritParentDependencies = true,
            Parent = parentContainer
        };

        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        parentContainer.Register(spec, out _);
        var contains = (childContainer as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(contains, Is.True);
    }

    [Test]
    public void Contains_InheritanceEnabledAndUnavailable_False()
    {
        // Set up
        DependencyContainer parentContainer = new();
        DependencyContainer childContainer = new()
        {
            InheritParentDependencies = true,
            Parent = parentContainer
        };

        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        parentContainer.Register(spec, out _);
        var contains = (childContainer as IDependencyResolutionProvider).CanProvide(
            typeof(Mock.IContractB));

        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void Contains_InheritanceDisabledAndAvailable_False()
    {
        // Set up
        DependencyContainer parentContainer = new();
        DependencyContainer childContainer = new()
        {
            InheritParentDependencies = false,
            Parent = parentContainer
        };

        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        parentContainer.Register(spec, out _);
        var contains = (childContainer as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(contains, Is.False);
    }


    [Test]
    public void Contains_Registered_True()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec, out _);
        var contains = (container as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(contains, Is.True);
    }

    [Test]
    public void Contains_Unregistered_False()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec, out _);
        var contains = (container as IDependencyResolutionProvider).CanProvide(
            typeof(Mock.IContractB));

        // Assert
        Assert.That(contains, Is.False);
    }
}