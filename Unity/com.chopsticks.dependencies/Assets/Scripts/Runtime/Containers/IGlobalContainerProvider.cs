using Chopsticks.Dependencies.Resolutions;
using System;

namespace Chopsticks.Dependencies.Containers
{
    public interface IGlobalContainerProvider<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
    {
        TNativeContainer Get { get; }

        void Reset();
    }
}
