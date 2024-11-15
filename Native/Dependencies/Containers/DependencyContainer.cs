using System;

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

        public bool Contains(Type dependencyType)
        {
            throw new NotImplementedException();
        }

        public IDependencyContainer Deregister(DependencySpecification specification)
        {
            throw new NotImplementedException();
        }

        public IDependencyContainer Register(DependencySpecification specification)
        {
            throw new NotImplementedException();
        }

        public bool Resolve<TContract>(out TContract dependency)
        {
            throw new NotImplementedException();
        }
    }
}
