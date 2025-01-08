using Chopsticks.Dependencies.Containers;

namespace MonoContainerTests
{
    public class MockMonoContainer : MonoContainer<DependencyContainer>
    {
        public new DependencyContainer InternalContainer { get; set; } = new();

        public bool RegisteredNativeDependencies { get; set; }

        protected override DependencyContainer SetUp() => 
            InternalContainer;

        protected override void RegisterNativeDependencies() => 
            RegisteredNativeDependencies = true;
    }
}