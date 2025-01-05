using Chopsticks.Dependencies;
using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Initialization;
using UnityEngine;

namespace Examples
{
    public class GameStartup : InitDependencies
    {
        private IDependencyContainer _container;

        protected override void RegisterDependencies(IDependencyContainer container)
        {
            _container = container;
            _container.Register(new()
            {
                Contract = typeof(IContractA),
                ImplementationFactory = _ =>
                {
                    // This is not the proper workflow once MonoDependencies register themselves.
                    // They should be created/exist in Unity or be instantiated by a factory.
                    var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    return go.AddComponent<DependencyA>();
                },
                Lifetime = DependencyLifetime.Singleton
            }, out _);
        }

        public void Update()
        {
            if (_container.Resolve(typeof(IContractA), out var a))
                Debug.Log(((IContractA)a).ConfiguredField);
        }
    }
}
