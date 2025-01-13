namespace Chopsticks.Dependencies.Containers
{
    public class GlobalDependencyContainerProvider : IGlobalContainerProvider<DependencyContainer>
    {
        // TODO :: Add locking for thread safety.

        public DependencyContainer Get => _instance ??= new();
        private static DependencyContainer _instance;

        public void Reset()
        {
            _instance?.Dispose();
            _instance = null;
        }
    }
}
