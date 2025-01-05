using Chopsticks.Dependencies;
using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Exceptions;
using NSubstitute;

namespace IDependencyContainerExtensionsTests;

public class AssertiveResolve
{
    public static class Mock
    {
        public static string RandomString => Guid.NewGuid().ToString();

        public interface IContract { }
    }


    [Test]
    public void AssertiveResolve_GenericNullResolvable_ThrowsWithErrorMessage()
    {
        // Set up
        Mock.IContract? expectedImplementation = null;
        var exceptionMessage = Mock.RandomString;
        var isResolved = true;

        var container = Substitute.For<IDependencyContainer>();
        container.Resolve(typeof(Mock.IContract), out _).Returns(x =>
        {
            x[1] = expectedImplementation;
            return isResolved;
        });

        // Act & Assert
        var exception = Assert.Throws<MissingDependencyException>(
            () => container.AssertiveResolve<Mock.IContract>(exceptionMessage));

        container.Received(1).Resolve(typeof(Mock.IContract), out _);
        Assert.That(exception!.Message, Is.EqualTo(exceptionMessage));
    }

    [Test]
    public void AssertiveResolve_GenericResolvable_EnforcesGenericTyping()
    {
        // Set up
        var expectedImplementation = Substitute.For<Mock.IContract>();
        var isResolved = true;

        var container = Substitute.For<IDependencyContainer>();
        container.Resolve(typeof(Mock.IContract), out _).Returns(x =>
        {
            x[1] = expectedImplementation;
            return isResolved;
        });

        // Act
        var implementation = container.AssertiveResolve<Mock.IContract>();

        // Assert
        container.Received(1).Resolve(typeof(Mock.IContract), out _);
        Assert.That(implementation, Is.EqualTo(expectedImplementation));
    }

    [Test]
    public void AssertiveResolve_GenericUnresolvable_ThrowsWithErrorMessage()
    {
        // Set up
        var expectedImplementation = Substitute.For<Mock.IContract>();
        var exceptionMessage = Mock.RandomString;
        var isResolved = false;

        var container = Substitute.For<IDependencyContainer>();
        container.Resolve(typeof(Mock.IContract), out _).Returns(x =>
        {
            x[1] = expectedImplementation;
            return isResolved;
        });

        // Act & Assert
        var exception = Assert.Throws<MissingDependencyException>(
            () => container.AssertiveResolve<Mock.IContract>(exceptionMessage));

        container.Received(1).Resolve(typeof(Mock.IContract), out _);
        Assert.That(exception!.Message, Is.EqualTo(exceptionMessage));
    }


    [Test]
    public void AssertiveResolve_NonGenericNullResolvable_ThrowsWithErrorMessage()
    {
        // Set up
        Mock.IContract? expectedImplementation = null;
        var exceptionMessage = Mock.RandomString;
        var isResolved = true;

        var container = Substitute.For<IDependencyContainer>();
        container.Resolve(typeof(Mock.IContract), out _).Returns(x =>
        {
            x[1] = expectedImplementation;
            return isResolved;
        });

        // Act & Assert
        var exception = Assert.Throws<MissingDependencyException>(
            () => container.AssertiveResolve(typeof(Mock.IContract), exceptionMessage));

        container.Received(1).Resolve(typeof(Mock.IContract), out _);
        Assert.That(exception!.Message, Is.EqualTo(exceptionMessage));
    }

    [Test]
    public void AssertiveResolve_NonGenericResolvable_EnforcesGenericTyping()
    {
        // Set up
        var expectedImplementation = Substitute.For<Mock.IContract>();
        var isResolved = true;

        var container = Substitute.For<IDependencyContainer>();
        container.Resolve(typeof(Mock.IContract), out _).Returns(x =>
        {
            x[1] = expectedImplementation;
            return isResolved;
        });

        // Act
        var implementation = container.AssertiveResolve(typeof(Mock.IContract));

        // Assert
        container.Received(1).Resolve(typeof(Mock.IContract), out _);
        Assert.That(implementation, Is.EqualTo(expectedImplementation));
    }

    [Test]
    public void AssertiveResolve_NonGenericUnresolvable_ThrowsWithErrorMessage()
    {
        // Set up
        var expectedImplementation = Substitute.For<Mock.IContract>();
        var exceptionMessage = Mock.RandomString;
        var isResolved = false;

        var container = Substitute.For<IDependencyContainer>();
        container.Resolve(typeof(Mock.IContract), out _).Returns(x =>
        {
            x[1] = expectedImplementation;
            return isResolved;
        });

        // Act & Assert
        var exception = Assert.Throws<MissingDependencyException>(
            () => container.AssertiveResolve(typeof(Mock.IContract), exceptionMessage));

        container.Received(1).Resolve(typeof(Mock.IContract), out _);
        Assert.That(exception!.Message, Is.EqualTo(exceptionMessage));
    }
}
