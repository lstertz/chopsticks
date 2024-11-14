namespace Chopsticks.Dependencies
{
    public interface IContractB
    {
        IContractA A { get; }
    }

    // Non-MonoBehaviour dependency, which was registered in the startup.
    public class DependencyB : IContractB
    {
        public IContractA A => _a.Get();
        private readonly Dependency<IContractA> _a = new();
    }
}
