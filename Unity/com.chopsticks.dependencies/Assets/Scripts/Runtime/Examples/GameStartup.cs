using Chopsticks.Dependencies.Containers;

namespace Chopsticks.Dependencies.Initialization
{
    public class InitGame : InitDependencies
    {
        protected override void RegisterDependencies(IDependencyContainer container)
        {
            // container.Register<DependencyB, IContractB>(resolveAtStartup: false);
        }
    }
}
