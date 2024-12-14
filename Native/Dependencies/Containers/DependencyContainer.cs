using Chopsticks.Dependencies.Resolutions;
using System;
using System.Collections.Generic;

namespace Chopsticks.Dependencies.Containers
{
    /// <inheritdoc cref="IDependencyContainer"/>
    public class DependencyContainer : IDependencyContainer, IDependencyResolutionProvider, IDisposable
    {
        /// <inheritdoc/>
        public bool InheritParentDependencies { get; set; } = true;

        /// <inheritdoc/>
        public IDependencyResolutionProvider? Parent { get; set; }


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
            // TODO :: Clean-up the Contained resolutions in parents.
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        public bool CanProvide(Type contract)
        {
            if (InheritParentDependencies && (Parent?.CanProvide(contract) == true))
                return true;

            return _resolutions.ContainsKey(contract);
        }


        /// <inheritdoc/>
        public IDependencyContainer Deregister(DependencyRegistration registration)
        {
            if (!_resolutions.ContainsKey(registration.Contract))
                return this;

            int index = 0;
            DependencyResolution? resolution = null;
            var resolutions = _resolutions[registration.Contract];
            int count = resolutions.Count;
            for (; index < count; index++)
            {
                if (ReferenceEquals(resolutions[index].Registration, registration))
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
        public bool Resolve(Type contract, out object? implementation)
        {
            implementation = null;

            var resolution = (this as IDependencyResolutionProvider).GetResolution(contract);
            if (resolution == null)
                return false;

            implementation = resolution.Get(this);
            return true;
        }

        /// <inheritdoc/>
        public IEnumerable<object> ResolveAll(Type contract)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ResolveAllSingletons()
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        DependencyResolution? IDependencyResolutionProvider.GetResolution(Type contract)
        {
            if (!_resolutions.TryGetValue(contract, out var resolutions))
                return Parent?.GetResolution(contract);

            return resolutions[0];
        }

        /// <inheritdoc/>
        IEnumerable<DependencyResolution> IDependencyResolutionProvider.GetResolutions(
            Type contract)
        {
            // TODO :: Implement.
            return [];
        }
    }
}
