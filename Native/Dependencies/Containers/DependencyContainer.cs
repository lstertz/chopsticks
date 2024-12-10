using Chopsticks.Dependencies.Resolutions;
using System;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    /// <inheritdoc cref="IDependencyContainer"/>
    public class DependencyContainer : IDependencyContainer, IDisposable
    {
        /// <inheritdoc/>
        public bool InheritParentDependencies { get; set; } = true;

        /// <inheritdoc/>
        public IDependencyContainer? Parent { get; set; }


        /// <summary>
        /// A mapping of contract types to all resolutions for each contract.
        /// </summary>
        private readonly Dictionary<Type, List<DependencyResolution>> _resolutions = [];
        private readonly IDependencyResolutionFactory _resolutionFactory;


        /// <summary>
        /// Constructs a new dependnecy container with the default resolution factory.
        /// </summary>
        public DependencyContainer() =>
            _resolutionFactory = new DependencyResolutionFactory();

        /// <summary>
        /// Constructs a new dependency container with the given resolution factory.
        /// </summary>
        public DependencyContainer(IDependencyResolutionFactory resolutionFactory) => 
            _resolutionFactory = resolutionFactory;


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
        public IDependencyContainer Deregister(DependencyRegistration registration)
        {
            if (!_resolutions.ContainsKey(registration.Contract))
                return this;

            // TODO :: May need to handle Contained (inherited resolutions) differently.

            int index = 0;
            DependencyResolution? resolution = null;
            var resolutions = _resolutions[registration.Contract];
            int count = resolutions.Count;
            for (; index < count; index++)
            {
                if (resolutions[index].Registration.Equals(registration))
                {
                    resolution = resolutions[index];
                    break;
                }
            }

            if (resolution == null)
                return this;

            resolution.Dispose();

            resolutions.RemoveAt(index);
            if (resolutions.Count == 0)
                _resolutions.Remove(registration.Contract);

            return this;
        }

        /// <inheritdoc/>
        public IDependencyContainer Register(DependencySpecification specification, 
            out DependencyRegistration registration)
        {
            var resolution = _resolutionFactory.BuildResolutionFor(specification);
            registration = resolution.Registration;

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
            implementation = null;

            var resolution = GetResolution(dependencyType);
            if (resolution == null)
                return Parent?.Resolve(dependencyType, out implementation) is true;

            implementation = resolution.Get(this);
            return true;
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
        private DependencyResolution? GetResolution(Type dependencyType)
        {
            if (!_resolutions.TryGetValue(dependencyType, out var resolution))
                return null;

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
        private IEnumerable<DependencyResolution> GetResolutions(Type dependencyType)
        {
            // TODO :: Find the local resolution first, then join with unique parent's resolutions.
            // TODO :: If any resolution is Contained, cache it as a local resolution during resolving.
            return [];
        }
    }
}
