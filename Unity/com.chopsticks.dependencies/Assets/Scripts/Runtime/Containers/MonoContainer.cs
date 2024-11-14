using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Designates that all child GameObject Containers/Dependencies 
    /// are contained within this container. This enables the organization 
    /// of dependencies to be defined through the Unity hierarchy and prefabs.
    /// </summary>
    public class MonoContainer : MonoBehaviour, IDependencyContainer
    {
        public bool InheritParentDependencies => _container.InheritParentDependencies;
        [SerializeField]  // TODO :: Prevent editing during runtime.
        private bool _inheritParentDependencies;

        public IDependencyContainer Parent
        {
            get => _container.Parent;
            set => _container.Parent = value;
        }
        // TODO ?? Permit an override parent, to work detached from the hierarchy.

        private DependencyContainer _container;


        private void Awake()
        {
            _container = new()
            {
                InheritParentDependencies = _inheritParentDependencies,
                Parent = null  // TODO :: Build with parent from up the hierarchy, or global.
            };
        }

        protected void Register()
        {
            // Use the container to register for a non-MonoBehaviour
            //   dependency.
        }

        protected virtual void SetUp() { }

        private void OnDestroy()
        {
            // Dispose of the container.
        }

        private void OnTransformParentChanged()
        {
            _container.Parent = null;  // TODO :: Climb the tree to find the new parent.
        }
    }
}
