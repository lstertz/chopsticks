using Chopsticks.Dependencies;
using UnityEngine;

namespace Examples
{
    public interface IContractA
    {
        string ConfiguredField { get; }
    }

    public class DependencyA : MonoDependency, IContractA
    {
        public string ConfiguredField => _configuredField;
        [SerializeField]
        private string _configuredField = "test";


        // Optional if OnEnable/OnDisable are used.
        protected override void OnEnable() { base.OnEnable(); }
        protected override void OnDisable() { base.OnDisable(); }

        protected override void Register()
        {
            // Example global registration: MonoDependencyContainer.Global.Register(this);

            throw new System.NotImplementedException();
        }
    }
}
