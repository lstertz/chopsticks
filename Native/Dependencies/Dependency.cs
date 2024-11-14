using Chopsticks.Dependencies.Containers;
using System;

namespace Chopsticks.Dependencies
{
    /// <summary>
    /// Wraps a contract type to accommodate dynamic runtime fulfillment 
    /// for its dependents.
    /// </summary>
    /// <typeparam name="TContract">The type of the contract that 
    /// will be fulfilled as a dependency.</typeparam>
    public class Dependency<TContract>
    {
        public TContract Get() => throw new NotImplementedException();
        
        public TContract Resolve(IDependencyContainer container) => 
            throw new NotImplementedException();
    }
}
