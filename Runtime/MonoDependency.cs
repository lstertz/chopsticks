using Chopsticks.Dependencies.Containers;
using UnityEngine;

namespace Chopsticks.Dependencies
{
    /// <summary>
    /// Defines a dependency, which may also have its own injected dependencies.
    /// </summary>
    public abstract class MonoDependency : MonoBehaviour
    {
        public IDependencyContainer Container { get; protected set; }

        public MonoDependency()  // May need to be Awake.
        {
            // Handle the set up of the dependencies.
        }

        protected virtual void OnEnable()
        {
            // Register with its container (or global container).
        }

        protected virtual void OnDisable()
        {
            // Deregister with its container (or global container).
        }

        protected virtual void OnTransformParentChanged()
        {
            // Switch containers, if necessary.

            OnContainerChanged();
        }

        protected virtual void OnContainerChanged()
        {

        }
    }
}
