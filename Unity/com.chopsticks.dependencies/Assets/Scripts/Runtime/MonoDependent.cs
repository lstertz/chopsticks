using Chopsticks.Dependencies.Containers;
using UnityEngine;

namespace Chopsticks.Dependencies
{
    /// <summary>
    /// Defines a consumer of dependencies that is itself not a dependency.
    /// </summary>
    public class MonoDependent : MonoBehaviour
    {
        public IDependencyContainer Container { get; }

        public MonoDependent()  // May need to be Awake.
        {
            // Handle the set up of the dependencies.
        }
    }
}
