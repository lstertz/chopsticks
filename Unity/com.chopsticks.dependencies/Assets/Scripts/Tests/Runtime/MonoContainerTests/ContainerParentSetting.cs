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

        public class SetUp
        {
            public static MockMonoContainer StandardContainer(
                ParentSetting parentSetting,
                out GameObject containerGameObject,
                out MockMonoContainer parentContainer,
                out IUnityContainerService<MockDependencyContainer> serviceSub)
            {
                var parentGameObject = new GameObject("Parent Object");
                containerGameObject = new GameObject("Test Object");
                containerGameObject.SetActive(false);

                var container = containerGameObject.AddComponent<MockMonoContainer>();
                parentContainer = parentGameObject.AddComponent<MockMonoContainer>();

                container.transform.parent = parentContainer.transform;

                container.SetSerializedProperty("_containerParentSetting", parentSetting);
                container.SetSerializedProperty("_overrideParent", parentContainer);

                serviceSub = container.ContainerService.Sub;
                serviceSub.ClearReceivedCalls();

                return container;
            }
        }


        [Test]
        [TestCase(ParentSetting.Global)]
        [TestCase(ParentSetting.HierarchyWithGlobal)]
        [TestCase(ParentSetting.HierarchyWithoutGlobal)]
        [TestCase(ParentSetting.Override)]
        public void Awake_VariousParentSettings_SetsToRetrievedContainer(
            ParentSetting parentSetting)
        {
            // Set up
            var container = SetUp.StandardContainer(parentSetting,
                out var containerGameObject, out var parentContainer, out var serviceSub);

            serviceSub.GetContainer(
                (ContainerRetrievalSetting)parentSetting, false, container, parentContainer)
                .Returns(parentContainer.InternalContainer);

            // Act
            containerGameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent,
                Is.EqualTo(parentContainer.InternalContainer));
            container.ContainerService.Sub.Received(1).GetContainer(
                (ContainerRetrievalSetting)parentSetting, false, container, parentContainer);
        }

        [Test]
        public void Awake_NoneParentSetting_SetsToNull()
        {
            // Set up
            var container = SetUp.StandardContainer(ParentSetting.None,
                out var containerGameObject, out var parentContainer, out _);

            // Act
            containerGameObject.SetActive(true);

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Null);
            container.ContainerService.Sub.DidNotReceiveWithAnyArgs().GetContainer(
                ContainerRetrievalSetting.Global, false, container, parentContainer);
        }


        [Test]
        [TestCase(ParentSetting.Global)]
        [TestCase(ParentSetting.HierarchyWithGlobal)]
        [TestCase(ParentSetting.HierarchyWithoutGlobal)]
        [TestCase(ParentSetting.Override)]
        public void OnTransformParentChanged_VariousParentSettings_SetsToRetrievedContainer(
            ParentSetting parentSetting)
        {
            // Set up
            var container = SetUp.StandardContainer(parentSetting,
                out _, out var parentContainer, out var serviceSub);

            serviceSub.GetContainer(
                (ContainerRetrievalSetting)parentSetting, false, container, parentContainer)
                .Returns(parentContainer.InternalContainer);

            // Act
            container.OnTransformParentChanged();

            // Assert
            Assert.That(container.InternalContainer.Parent,
                Is.EqualTo(parentContainer.InternalContainer));
            container.ContainerService.Sub.Received(1).GetContainer(
                (ContainerRetrievalSetting)parentSetting, false, container, parentContainer);
        }

        [Test]
        public void OnTransformParentChanged_NoneParentSetting_SetsToNull()
        {
            // Set up
            var container = SetUp.StandardContainer(ParentSetting.None,
                out _, out var parentContainer, out _);

            // Act
            container.OnTransformParentChanged();

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Null);
            container.ContainerService.Sub.DidNotReceiveWithAnyArgs().GetContainer(
                ContainerRetrievalSetting.Global, false, container, parentContainer);
        }


        [Test]
        [TestCase(ParentSetting.Global)]
        [TestCase(ParentSetting.HierarchyWithGlobal)]
        [TestCase(ParentSetting.HierarchyWithoutGlobal)]
        [TestCase(ParentSetting.Override)]
        public void OnValidate_VariousParentSettings_SetsToRetrievedContainer(
            ParentSetting parentSetting)
        {
            // Set up
            var container = SetUp.StandardContainer(parentSetting,
                out _, out var parentContainer, out var serviceSub);

            serviceSub.GetContainer(
                (ContainerRetrievalSetting)parentSetting, false, container, parentContainer)
                .Returns(parentContainer.InternalContainer);

            // Act
            container.OnValidate();

            // Assert
            Assert.That(container.InternalContainer.Parent,
                Is.EqualTo(parentContainer.InternalContainer));
            container.ContainerService.Sub.Received(1).GetContainer(
                (ContainerRetrievalSetting)parentSetting, false, container, parentContainer);
        }

        [Test]
        public void OnValidate_NoneParentSetting_SetsToNull()
        {
            // Set up
            var container = SetUp.StandardContainer(ParentSetting.None,
                out _, out var parentContainer, out _);

            // Act
            container.OnValidate();

            // Assert
            Assert.That(container.InternalContainer.Parent, Is.Null);
            container.ContainerService.Sub.DidNotReceiveWithAnyArgs().GetContainer(
                ContainerRetrievalSetting.Global, false, container, parentContainer);
        }
    }
}