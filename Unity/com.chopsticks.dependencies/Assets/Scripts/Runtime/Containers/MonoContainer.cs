﻿namespace Chopsticks.Dependencies.Containers
{
    /// <summary>
    /// Designates that all child GameObject Containers/Dependencies 
    /// are contained within this container. This enables the organization 
    /// of dependencies to be defined through the Unity hierarchy and prefabs.
    /// </summary>
    public class MonoContainer : BaseMonoContainer<DependencyContainer, UnityDependencyContainerService>
    {
        protected override DependencyContainer SetUp() => new();
    }
}
