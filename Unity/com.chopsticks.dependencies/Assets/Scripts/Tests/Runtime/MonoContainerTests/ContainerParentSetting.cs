using MonoContainerTests.Mocks;
using NUnit.Framework;
using UnityEngine;

using ParentSetting = Chopsticks.Dependencies.Containers.ContainerParentSetting;

namespace MonoContainerTests
{
    // TODO :: Update as behavior tests using the mock service.

    public class ContainerParentSetting
    {
        [Test]
        public void ContainerParentSetting_GlobalParentSettingOnAwake_SetsToGlobal()
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
            Assert.That(container.InternalContainer.Parent, Is.EqualTo(MockMonoContainer.Global));
        }

        [Test]
        public void ContainerParentSetting_HierarchyWithGlobalParentSettingOnAwake_WithParent_SetsToParent()
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

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, 
                Is.EqualTo(parentContainer.InternalContainer));
        }

        [Test]
        public void ContainerParentSetting_HierarchyWithGlobalParentSettingOnAwake_WithNoParent_SetsToGlobal()
        {
            // Set up
            var gameObject = new GameObject();
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.HierarchyWithGlobal);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.EqualTo(MockMonoContainer.Global));
        }

        [Test]
        public void ContainerParentSetting_HierarchyWithoutGlobalParentSettingOnAwake_WithParent_SetsToParent()
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

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent,
                Is.EqualTo(parentContainer.InternalContainer));
        }

        [Test]
        public void ContainerParentSetting_HierarchyWithoutGlobalParentSettingOnAwake_WithNoParent_SetsToNull()
        {
            // Set up
            var gameObject = new GameObject();
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting",
                ParentSetting.HierarchyWithoutGlobal);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Null);
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

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Null);
        }

        [Test]
        public void ContainerParentSetting_OverrideParentSettingOnAwake_SetsToOverrideParent()
        {
            // Set up
            var gameObject = new GameObject();
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();

            container.SetSerializedProperty("_containerParentSetting", 
                ParentSetting.Override);
            container.SetSerializedProperty("_overrideParent", parentContainer);

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, 
                Is.EqualTo(parentContainer.InternalContainer));
        }





        // TODO :: Test for changes after creation.
    }
}