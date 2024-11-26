using System;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    /// <inheritdoc cref="IDependencyContainer"/>
    public class DependencyContainer : IDependencyContainer, IDisposable
    {
        /// <summary>
        /// Resolves, and maintains resolved implementation instances of, the dependency 
        /// specified by the provided dependency specification.
        /// </summary>
        /// <param name="specification">The spec that defines the dependency that is 
        /// resolved and maintained by this resolution.</param>
        private abstract class Resolution(DependencySpecification specification) : IDisposable
        {
            protected DependencySpecification Specification { get; init; } = specification;

            public abstract void Dispose();
        }

        private class ContainedResolution(DependencySpecification specification) :
            Resolution(specification)
        {
            public override void Dispose()
            {
            }
        }

        private class SingletonResolution(DependencySpecification specification) : 
            Resolution(specification)
        {
            public override void Dispose()
            {
            }
        }

        private class TransientResolution(DependencySpecification specification) :
            Resolution(specification)
        {
            public override void Dispose()
            {
            }
        }


        /// <inheritdoc/>
        public bool InheritParentDependencies { get; set; }

        /// <inheritdoc/>
        public IDependencyContainer? Parent { get; set; }


        /// <summary>
        /// A mapping of contract types to all resolutions for each contract.
        /// </summary>
        private readonly Dictionary<Type, List<Resolution>> _resolutions = [];


        /// <inheritdoc/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        public bool Contains(Type dependencyType)
        {
            if (InheritParentDependencies && Parent?.Contains(dependencyType) == true)
                return true;

            return _resolutions.ContainsKey(dependencyType);
        }


        /// <inheritdoc/>
        public IDependencyContainer Deregister(DependencySpecification specification)
        {
            if (!_resolutions.ContainsKey(specification.Contract))
                return this;

            var resolution = _resolutions[specification.Contract][0];
            resolution.Dispose();

            _resolutions[specification.Contract].RemoveAt(0);
            if (_resolutions[specification.Contract].Count == 0)
                _resolutions.Remove(specification.Contract);

            return this;
        }

        /// <inheritdoc/>
        public IDependencyContainer Register(DependencySpecification specification)
        {
            Resolution resolution = specification.Lifetime switch
            {
                DependencyLifetime.Contained => new ContainedResolution(specification),
                DependencyLifetime.Singleton => new SingletonResolution(specification),
                DependencyLifetime.Transient => new TransientResolution(specification),
                _ => throw new NotImplementedException($"The lifetime of " +
                    $"{specification.Lifetime} is not supported.")
            };

            if (_resolutions.TryAdd(specification.Contract, [resolution]))
                return this;

            _resolutions[specification.Contract].Add(resolution);
            return this;
        }


        /// <inheritdoc/>
        public object AssertiveResolve(Type dependencyType,
            string? customErrorMessage = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Resolve(Type dependencyType, out object? implementation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<object> ResolveAll(Type dependencyType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ResolveAllSingletons()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Provides the first resolution for the specified dependency type.
        /// </summary>
        /// <param name="dependencyType">The type of the dependency, either as the 
        /// implementation type or the contract type.</param>
        /// <returns>The first resolution for the specified dependency type, 
        /// or null if there is no such known resolution.</returns>
        private Resolution? GetResolution(Type dependencyType)
        {
            // TODO :: Find the local resolution first, then check parent's resolutions.
            // TODO :: If the resolution is Contained, cache it as a local resolution during resolving.
            return null;
        }

        /// <summary>
        /// Provides all resolutions for the specified dependency type.
        /// </summary>
        /// <param name="dependencyType">The type of the dependency, either as the 
        /// implementation type or the contract type.</param>
        /// <returns>All resolutions for the specified dependency type.</returns>
        private IEnumerable<Resolution> GetResolutions(Type dependencyType)
        {
            // TODO :: Find the local resolution first, then join with unique parent's resolutions.
            // TODO :: If any resolution is Contained, cache it as a local resolution during resolving.
            return [];
        }
    }
}
