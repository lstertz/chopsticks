using Chopsticks.Dependencies.Containers;
using UnityEditor;

namespace MonoContainerTests
{
    public static class MonoContainerTestExtensions
    {
        public static MonoContainer SetSerializedProperty(this MonoContainer container, 
            string propertyName, bool value)
        {
            var serializedObject = new SerializedObject(container);
            var serializedProperty = serializedObject.FindProperty(propertyName);

            serializedProperty.boolValue = value;
            serializedObject.ApplyModifiedProperties();

            return container;
        }

        public static MonoContainer SetSerializedProperty(this MonoContainer container,
            string propertyName, UnityEngine.Object value)
        {
            var serializedObject = new SerializedObject(container);
            var serializedProperty = serializedObject.FindProperty(propertyName);

            serializedProperty.objectReferenceValue = value;
            serializedObject.ApplyModifiedProperties();

            return container;
        }
    }
}
