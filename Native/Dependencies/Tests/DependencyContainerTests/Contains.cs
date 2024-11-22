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
        container.Register(spec);
        container.Deregister(spec);

        // Assert
        Assert.That(container.Contains(spec.Contract), Is.False);
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
        container.Register(spec);
        container.Deregister(spec);

        // Assert
        Assert.That(container.Contains(spec.Implementation), Is.False);
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

        // Assert
        Assert.That(childContainer.Contains(spec.Contract), Is.True);
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

        // Assert
        Assert.That(childContainer.Contains(typeof(Mock.IContractB)), Is.False);
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

        // Assert
        Assert.That(childContainer.Contains(spec.Contract), Is.False);
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

        // Assert
        Assert.That(container.Contains(spec.Implementation), Is.False);
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

        // Assert
        Assert.That(container.Contains(typeof(Mock.IContractB)), Is.False);
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

        // Assert
        Assert.That(container.Contains(spec.Contract), Is.True);
    }

    [Test]
    public void Contains_RegisteredImplementationForSelf_FalseForContract()
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

        // Assert
        Assert.That(container.Contains(spec.Contract), Is.False);
    }

    [Test]
    public void Contains_RegisteredImplementationForSelf_TrueForImplementation()
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

        // Assert
        Assert.That(container.Contains(spec.Implementation), Is.True);
    }
}