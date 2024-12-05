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
        /// <param name="contract">The contract that this resolution fulfills.</param>
        /// <param name="factory">The factory that provides implementations for resolution.</param>
        private abstract class Resolution(Type contract, 
            Func<IDependencyContainer, object> factory) : 
            IDisposable
        {
            public Func<IDependencyContainer, object> Factory { get; private init; } = factory;

            // TODO :: Use the registration in place of the specification (reduce memory footprint).

            public DependencyRegistration Registration { get; private init; } = new()
            {
                Contract = contract,
            };


            public abstract void Dispose();

            public abstract object Get(IDependencyContainer container);
        }

        private class ContainedResolution(Type contract, 
            Func<IDependencyContainer, object> factory) :
            Resolution(contract, factory)
        {
            private object? _instance;


            public override void Dispose()
            {
                if (_instance is IDisposable disposable)
                    disposable.Dispose();

                _instance = null;
            }

            public override object Get(IDependencyContainer container) =>
                _instance ??= Factory(container);
        }

        private class SingletonResolution(Type contract, 
            Func<IDependencyContainer, object> factory) :
            Resolution(contract, factory)
        {
            private object? _instance;

            public override void Dispose()
            {
                if (_instance is IDisposable disposable)
                    disposable.Dispose();

                _instance = null;

            }

            public override object Get(IDependencyContainer container) => 
                _instance ??= Factory(container);
        }

        private class TransientResolution(Type contract, 
            Func<IDependencyContainer, object> factory) :
            Resolution(contract, factory)
        {
            public override void Dispose()
            {
            }

            public override object Get(IDependencyContainer container) => Factory(container);
        }


        /// <inheritdoc/>
        public bool InheritParentDependencies { get; set; } = true;

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
        public IDependencyContainer Deregister(DependencyRegistration registration)
        {
            if (!_resolutions.ContainsKey(registration.Contract))
                return this;

            // TODO :: May need to handle Contained (inherited resolutions) differently.

            int index = 0;
            Resolution? resolution = null;
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
            Resolution resolution = specification.Lifetime switch
            {
                DependencyLifetime.Contained => new ContainedResolution(
                    specification.Contract, specification.ImplementationFactory),
                DependencyLifetime.Singleton => new SingletonResolution(
                    specification.Contract, specification.ImplementationFactory),
                DependencyLifetime.Transient => new TransientResolution(
                    specification.Contract, specification.ImplementationFactory),
                _ => throw new NotImplementedException($"The lifetime of " +
                    $"{specification.Lifetime} is not supported.")
            };
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
                return false;


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
