namespace DependencyContainerTests;

// TODO :: Write tests for Resolve methods.
// Make sure to account for changes through deregistration and re-registration, and multi-registration.

// TODO :: Write tests for Disposal.

public class Resolve
{
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
    public void Resolve_RegisteredContained_OutsSameInstanceEveryCall()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredContained_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredImplementationWithContract_OutsImplementationDependency()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredImplementationWithContract_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredImplementationAsContract_OutsImplementationDependency()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredImplementationAsContract_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredSingleton_OutsSameInstanceEveryCall()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredSingleton_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredTransient_OutsNewInstanceEveryCall()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_RegisteredTransient_True()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_UnregisteredContract_False()
    {
        Assert.Ignore();
    }

    [Test]
    public void Resolve_UnregisteredContract_OutsNullDependency()
    {
        Assert.Ignore();
    }
}
