using Chopsticks.Dependencies.Containers;

namespace DependencyContainerTests;

public class Contains
{
    private static class Mock
    {
        public interface IContractA { }

        public interface IContractB { }

        public class ImplementationA : IContractA { }
    }


    [Test]
    public void Contains_DeregisteredContractedImplementation_FalseForContract()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec)
            .Deregister(spec);
        var contains = container.Contains(spec.Contract);

        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void Contains_DeregisteredImplementationForSelf_FalseForImplementation()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.ImplementationA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec)
            .Deregister(spec);
        var contains = container.Contains(spec.Implementation);

        // Assert
        Assert.That(contains, Is.False);
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
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        parentContainer.Register(spec);
        var contains = childContainer.Contains(spec.Contract);

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
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        parentContainer.Register(spec);
        var contains = childContainer.Contains(typeof(Mock.IContractB));

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
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        parentContainer.Register(spec);
        var contains = childContainer.Contains(spec.Contract);

        // Assert
        Assert.That(contains, Is.False);
    }


    [Test]
    public void Contains_RegisteredContractedImplementation_FalseForImplementation()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec);
        var contains = container.Contains(spec.Implementation);

        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void Contains_RegisteredContractedImplementation_FalseForUnregisteredContract()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec);
        var contains = container.Contains(typeof(Mock.IContractB));

        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void Contains_RegisteredContractedImplementation_TrueForContract()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.IContractA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec);
        var contains = container.Contains(spec.Contract);

        // Assert
        Assert.That(contains, Is.True);
    }

    [Test]
    public void Contains_RegisteredImplementationForSelf_FalseForContract()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.ImplementationA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec);
        var contains = container.Contains(typeof(Mock.IContractA));

        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void Contains_RegisteredImplementationForSelf_TrueForImplementation()
    {
        // Set up
        DependencyContainer container = new();
        DependencySpecification spec = new()
        {
            Contract = typeof(Mock.ImplementationA),
            Implementation = typeof(Mock.ImplementationA),
            ImplementationFactory = _ => new()
        };

        // Act
        container.Register(spec);
        var contains = container.Contains(spec.Implementation);

        // Assert
        Assert.That(contains, Is.True);
    }
}