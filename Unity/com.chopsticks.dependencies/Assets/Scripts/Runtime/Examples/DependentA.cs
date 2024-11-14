using UnityEngine;

namespace Chopsticks.Dependencies
{

    public class DependentA : MonoDependent
    {
        public IContractB B => _b.Get()!;
        [SerializeField]
        private Dependency<IContractB> _b;

        //private Dependencies<IContractB> Bs;

        private void Awake()
        {
            // Optionally resolve for this specific container.
            _b.Resolve(Container);

            Debug.Log(B.A.ConfiguredField);
        }
    }
}
