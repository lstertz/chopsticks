using Chopsticks.Dependencies.Containers;
using MonoContainerTests.Mocks;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;
using UnityEngine;

using ParentSetting = Chopsticks.Dependencies.Containers.ContainerParentSetting;

namespace MonoContainerTests
{
    public class ContainerParentSetting
    {
        [Test]
        public void ContainerParentSetting_GlobalParentSettingOnAwake_SetsToRetrievedContainer()
        {
            // Set up
            var gameObject = new GameObject();
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting", ParentSetting.Global);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.Global, false, container,
                parentContainer);
        }

        [Test]
        public void ContainerParentSetting_HierarchyWithGlobalParentSettingOnAwake_WithParent_SetsToRetrievedContainer()
        {
            // Set up
            var parentGameObject = new GameObject("Parent Object");
            var gameObject = new GameObject("Test Object");
            gameObject.transform.parent = parentGameObject.transform;
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = parentGameObject.AddComponent<MockMonoContainer>();

            container.transform.parent = parentContainer.transform;

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.HierarchyWithGlobal);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.HierarchyWithGlobal, false, container,
                parentContainer);
        }

        [Test]
        public void ContainerParentSetting_HierarchyWithoutGlobalParentSettingOnAwake_SetsToRetrievedContainer()
        {
            // Set up
            var parentGameObject = new GameObject("Parent Object");
            var gameObject = new GameObject("Test Object");
            gameObject.transform.parent = parentGameObject.transform;
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = parentGameObject.AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.HierarchyWithoutGlobal);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.HierarchyWithoutGlobal, false, container,
                parentContainer);
        }

        [Test]
        public void ContainerParentSetting_NoneParentSettingOnAwake_SetsToNull()
        {
            // Set up
            var gameObject = new GameObject();
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.None);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Null);
            container.ContainerService.Sub.DidNotReceiveWithAnyArgs().GetContainer(
                ContainerRetrievalSetting.Global, false, container, parentContainer);
        }

        [Test]
        public void ContainerParentSetting_OverrideParentSettingOnAwake_SetsToRetrievedContainer()
        {
            // Set up
            var gameObject = new GameObject();
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting", 
                ParentSetting.Override);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            container.ContainerService.Sub.ClearReceivedCalls();

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Not.Null);
            container.ContainerService.Sub.Received(1).GetContainer(
                ContainerRetrievalSetting.Override, false, container, parentContainer);
        }





        // TODO :: Test for changes after creation.
    }
}