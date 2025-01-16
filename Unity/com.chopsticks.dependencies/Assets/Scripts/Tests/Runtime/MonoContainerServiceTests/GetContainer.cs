using Chopsticks.Dependencies.Containers;
using MonoContainerTests.Mocks;
using NUnit.Framework;
using UnityEngine;

namespace MonoContainerServiceTests
{
    public class GetContainer
    {
        public class SetUp
        {
            public static MonoContainerService ParentedContainers(
                out MonoContainer childContainer, out MonoContainer parentContainer)
            {
                var service = new MonoContainerService();

                var parentGameObject = new GameObject("Parent Object");
                var gameObject = new GameObject("Test Object");
                gameObject.transform.parent = parentGameObject.transform;
                gameObject.SetActive(false);

                childContainer = gameObject.AddComponent<MonoContainer>();
                parentContainer = parentGameObject.AddComponent<MonoContainer>();

                return service;
            }

            public static MonoContainerService StandardContainer(out MonoContainer container)
            {
                var service = new MonoContainerService();

                var gameObject = new GameObject("Test Object");
                gameObject.SetActive(false);

                container = gameObject.AddComponent<MonoContainer>();

                return service;
            }
        }


        [Test]
        public void GetContainer_GlobalRetrievalSetting_Global()
        {
            // Set up
            var service = new MonoContainerService();

            // Act
            var serviceContainer = service.GetContainer(ContainerRetrievalSetting.Global,
                false, (MonoContainer)null, null);

            // Assert
            Assert.That(serviceContainer, Is.EqualTo(service.GlobalContainer));
        }

        [Test]
        public void GetContainer_HierarchyWithGlobalRetrievalSetting_IncludeSelf_Self()
        {
            // Set up
            var service = SetUp.ParentedContainers(out var child, out var parent);

            // Act
            var serviceContainer = service.GetContainer(
                ContainerRetrievalSetting.HierarchyWithGlobal, true, child, null);

            // Assert
            Assert.That(serviceContainer, Is.EqualTo(
                (child as IUnityContainer<DependencyContainer>).NativeContainer));
        }

        [Test]
        public void GetContainer_HierarchyWithGlobalRetrievalSetting_WithParent_Parent()
        {
            // Set up
            var service = SetUp.ParentedContainers(out var child, out var parent);

            // Act
            var serviceContainer = service.GetContainer(
                ContainerRetrievalSetting.HierarchyWithGlobal, false, child, null);

            // Assert
            Assert.That(serviceContainer, Is.EqualTo(
                (parent as IUnityContainer<DependencyContainer>).NativeContainer));
        }

        [Test]
        public void GetContainer_HierarchyWithGlobalRetrievalSetting_WithNoParent_Global()
        {
            // Set up
            var service = SetUp.StandardContainer(out var container);

            // Act
            var serviceContainer = service.GetContainer(
                ContainerRetrievalSetting.HierarchyWithGlobal, false, container, null);

            // Assert
            Assert.That(serviceContainer, Is.EqualTo(MonoContainer.Global));
        }

        [Test]
        public void GetContainer_HierarchyWithoutGlobalRetrievalSetting_IncludeSelf_Self()
        {
            // Set up
            var service = SetUp.ParentedContainers(out var child, out var parent);

            // Act
            var serviceContainer = service.GetContainer(
                ContainerRetrievalSetting.HierarchyWithoutGlobal, true, child, null);

            // Assert
            Assert.That(serviceContainer, Is.EqualTo(
                (child as IUnityContainer<DependencyContainer>).NativeContainer));
        }

        [Test]
        public void GetContainer_HierarchyWithoutGlobalRetrievalSetting_WithParent_Parent()
        {
            // Set up
            var service = SetUp.ParentedContainers(out var child, out var parent);

            // Act
            var serviceContainer = service.GetContainer(
                ContainerRetrievalSetting.HierarchyWithoutGlobal, false, child, null);

            // Assert
            Assert.That(serviceContainer, Is.EqualTo(
                (parent as IUnityContainer<DependencyContainer>).NativeContainer));
        }

        [Test]
        public void GetContainer_HierarchyWithoutGlobalRetrievalSetting_WithNoParent_Null()
        {
            // Set up
            var service = SetUp.StandardContainer(out var container);

            // Act
            var serviceContainer = service.GetContainer(
                ContainerRetrievalSetting.HierarchyWithoutGlobal, false, container, null);

            // Assert
            Assert.That(serviceContainer, Is.Null);
        }

        [Test]
        public void GetContainer_OverrideRetrievalSetting_OverrideNativeContainer()
        {
            // Set up
            var service = SetUp.StandardContainer(out var overrideContainer);

            // Act
            var serviceContainer = service.GetContainer(ContainerRetrievalSetting.Override,
                false, null, overrideContainer);

            // Assert
            Assert.That(serviceContainer, Is.EqualTo(
                (overrideContainer as IUnityContainer<DependencyContainer>).NativeContainer));
        }
    }
}