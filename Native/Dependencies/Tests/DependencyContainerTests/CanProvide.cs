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
    public void CanProvide_AfterDispose_False()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        container.Register(spec, out var registration);
        container.Dispose();

        // Act
        var canProvide = (container as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(canProvide, Is.False);
    }


    [Test]
    public void CanProvide_Deregistered_False()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        container.Register(spec, out var registration)
            .Deregister(registration);

        // Act
        var canProvide = (container as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(canProvide, Is.False);
    }

    [Test]
    public void CanProvide_DeregisteredFirstAfterMultipleRegistrations_True()
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

        container.Register(spec1, out var registration1)
            .Register(spec2, out _)
            .Deregister(registration1);

        // Act
        var canProvide = (container as IDependencyResolutionProvider).CanProvide(spec2.Contract);

        // Assert
        Assert.That(canProvide, Is.True);
    }

    [Test]
    public void CanProvide_DeregisteredMultipleAfterMultipleRegistrations_False()
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

        container.Register(spec1, out var registration1)
            .Register(spec2, out var registration2)
            .Deregister(registration1)
            .Deregister(registration2);

        // Act
        var canProvide = (container as IDependencyResolutionProvider).CanProvide(spec2.Contract);

        // Assert
        Assert.That(canProvide, Is.False);
    }

    [Test]
    public void CanProvide_DeregisteredSecondAfterMultipleRegistrations_True()
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

        container.Register(spec1, out _)
            .Register(spec2, out var registration2)
            .Deregister(registration2);

        // Act
        var canProvide = (container as IDependencyResolutionProvider).CanProvide(spec1.Contract);

        // Assert
        Assert.That(canProvide, Is.True);
    }


    [Test]
    public void CanProvide_InheritanceEnabledAndAvailable_True()
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

        parentContainer.Register(spec, out _);

        // Act
        var canProvide = (childContainer as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(canProvide, Is.True);
    }

    [Test]
    public void CanProvide_InheritanceEnabledAndUnavailable_False()
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

        parentContainer.Register(spec, out _);

        // Act
        var canProvide = (childContainer as IDependencyResolutionProvider).CanProvide(
            typeof(Mock.IContractB));

        // Assert
        Assert.That(canProvide, Is.False);
    }

    [Test]
    public void CanProvide_InheritanceDisabledAndAvailable_False()
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

        parentContainer.Register(spec, out _);

        // Act
        var canProvide = (childContainer as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(canProvide, Is.False);
    }


    [Test]
    public void CanProvide_Registered_True()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        container.Register(spec, out _);

        // Act
        var canProvide = (container as IDependencyResolutionProvider).CanProvide(spec.Contract);

        // Assert
        Assert.That(canProvide, Is.True);
    }

    [Test]
    public void CanProvide_Unregistered_False()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            ImplementationFactory = _ => new()
        };

        container.Register(spec, out _);

        // Act
        var canProvide = (container as IDependencyResolutionProvider).CanProvide(
            typeof(Mock.IContractB));

        // Assert
        Assert.That(canProvide, Is.False);
    }
}