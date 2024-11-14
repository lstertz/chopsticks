namespace Chopsticks.Dependencies.Containers
{
    public class DependencyContainer : IDependencyContainer
    {
        public bool InheritParentDependencies { get; init; }

        public IDependencyContainer Parent { get; set; }


        public DependencyContainer()
        {

        }

        public DependencyContainer(IDependencyContainer parentContainer)
        {

        }
    }
}
