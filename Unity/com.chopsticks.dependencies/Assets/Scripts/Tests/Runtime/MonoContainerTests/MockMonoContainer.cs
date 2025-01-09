using Chopsticks.Dependencies.Containers;
using Chopsticks.Dependencies.Resolutions;
using System;
using System.Collections.Generic;

using static MonoContainerTests.MockMonoContainer;

namespace MonoContainerTests
{
    public class MockMonoContainer : MonoContainer<MockDependencyContainer>
    {
        public abstract class MockDependencyContainer :
            IDependencyContainer, IDependencyResolutionProvider
        {
            public abstract bool InheritParentDependencies { get; set; }
            public abstract IDependencyResolutionProvider Parent { get; set; }

            public abstract bool CanProvide(Type contract);
            public abstract IDependencyContainer Deregister(DependencyRegistration registration);
            public abstract DependencyResolution GetResolution(Type contract);
            public abstract IEnumerable<DependencyResolution> GetResolutions(Type contract);
            public abstract IEnumerable<DependencyResolution> GetResolutionsForDisposal();
            public abstract IDependencyContainer Register(DependencySpecification specification, 
                out DependencyRegistration registration);
            public abstract bool Resolve(Type contract, out object implementation);
            public abstract IEnumerable<object> ResolveAll(Type contract);
        }


        public new MockDependencyContainer InternalContainer { get; set; } = 
            NSubstitute.Substitute.For<MockDependencyContainer>();

        public bool RegisteredNativeDependencies { get; set; }

        protected override MockDependencyContainer SetUp() => 
            InternalContainer;

        protected override void RegisterNativeDependencies() => 
            RegisteredNativeDependencies = true;
    }
}