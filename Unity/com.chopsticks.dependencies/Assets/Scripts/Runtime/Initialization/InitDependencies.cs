using Chopsticks.Dependencies.Containers;
using UnityEngine;

namespace Chopsticks.Dependencies.Initialization
{
    public class InitDependencies : MonoBehaviour
    {
        private void Awake()
        {
            var container = new DependencyContainer();
            RegisterDependencies(container);

            // TODO :: Finalize registration and instantiate singletons.
        }

        protected virtual void RegisterDependencies(IDependencyContainer container)
        {

        }
    }
}
