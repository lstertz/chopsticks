using Chopsticks.Dependencies.Containers;
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

            var container = gameObject.AddComponent<MonoContainer>();
            var parentContainer = new GameObject().AddComponent<MonoContainer>();
            container.SetSerializedProperty("_overrideParent", parentContainer);

            // Act
            gameObject.SetActive(true);

            // Assert
            Assert.That(parentContainer, Is.EqualTo(container.Parent));
        }

        // TODO :: Test for changes after creation.
    }
}