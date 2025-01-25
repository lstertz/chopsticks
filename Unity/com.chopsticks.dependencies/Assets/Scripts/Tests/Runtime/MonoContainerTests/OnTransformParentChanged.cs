using Chopsticks.Dependencies.Containers;
using MonoContainerTests.Mocks;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;
using UnityEngine;

using ParentSetting = Chopsticks.Dependencies.Containers.ContainerParentSetting;

namespace MonoContainerTests
{
    public class OnTransformParentChanged
    {
        [Test]
        public void OnTransformParentChanged_GlobalParentSetting_SetsToRetrievedContainer()
        {
            // Set up
            var gameObject = new GameObject();

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting", ParentSetting.Global);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            // Act
            container.OnTransformParentChanged();

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.Global, false, container,
                parentContainer);
        }

        [Test]
        public void OnTransformParentChanged_HierarchyWithGlobalParentSetting_SetsToRetrievedContainer()
        {
            // Set up
            var parentGameObject = new GameObject("Parent Object");
            var gameObject = new GameObject("Test Object");

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = parentGameObject.AddComponent<MockMonoContainer>();

            container.transform.parent = parentContainer.transform;

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.HierarchyWithGlobal);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            container.OnTransformParentChanged();

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.HierarchyWithGlobal, false, container,
                parentContainer);
        }

        [Test]
        public void OnTransformParentChanged_HierarchyWithoutGlobalParentSetting_SetsToRetrievedContainer()
        {
            // Set up
            var parentGameObject = new GameObject("Parent Object");
            var gameObject = new GameObject("Test Object");

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = parentGameObject.AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.HierarchyWithoutGlobal);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            container.OnTransformParentChanged();

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.HierarchyWithoutGlobal, false, container,
                parentContainer);
        }

        [Test]
        public void OnTransformParentChanged_NoneParentSetting_SetsToNull()
        {
            // Set up
            var gameObject = new GameObject();

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.None);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            container.OnTransformParentChanged();

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Null);
            container.ContainerService.Sub.DidNotReceiveWithAnyArgs().GetContainer(
                ContainerRetrievalSetting.Global, false, container, parentContainer);
        }

        [Test]
        public void OnTransformParentChanged_OverrideParentSetting_SetsToRetrievedContainer()
        {
            // Set up
            var gameObject = new GameObject();

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.Override);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            container.OnTransformParentChanged();

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.Override, false, container, parentContainer);
        }
    }
}