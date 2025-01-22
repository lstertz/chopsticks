using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Factories;

namespace DependencyResolutionFactoryTests;

public class BuildResolutionFor
{
    public static class Mock
    {
        public interface IContract { }
    }


    [Test]
    public void BuildContainer_FirstCall_BuildsNewInstance()
    {
        // Set up
        var factory = new DependencyContainerFactory();

        // Act
        var container = factory.BuildContainer();

        // Assert
        Assert.That(container, Is.Not.Null);
        Assert.That(container is DependencyContainer, Is.True);
    }

    [Test]
    public void BuildContainer_SecondCall_BuildsNewInstance()
    {
        // Set up
        var factory = new DependencyContainerFactory();
        var firstContainer = factory.BuildContainer();

        // Act
        var secondContainer = factory.BuildContainer();

        // Assert
        Assert.That(secondContainer, Is.Not.EqualTo(firstContainer));
    }
}
