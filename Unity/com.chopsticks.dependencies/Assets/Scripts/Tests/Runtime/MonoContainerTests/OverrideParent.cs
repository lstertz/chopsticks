using MonoContainerTests.Mocks;
using NUnit.Framework;
using UnityEngine;

namespace MonoContainerTests
{
    public class OverrideParent
    {
        [Test]
        public void OverrideParent_ThroughSerializedFieldOnAwake_Sets()
        {
            // Set up
            var gameObject = new GameObject();
            gameObject.SetActive(false);

            var container = gameObject.AddComponent<MockMonoContainer>();
            var parentContainer = new GameObject().AddComponent<MockMonoContainer>();
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