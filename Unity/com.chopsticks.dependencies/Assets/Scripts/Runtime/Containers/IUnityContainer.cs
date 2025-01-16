using Chopsticks.Dependencies.Resolutions;
using System;

namespace Chopsticks.Dependencies.Containers
{
    public interface IUnityContainer<TNativeContainer>
        where TNativeContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
    {
        TNativeContainer NativeContainer { get; }
    }
}
