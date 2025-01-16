﻿using UnityEditor;

namespace MonoContainerTests.Mocks
{
    public static class MockMonoContainerTestExtensions
    {
        public static MockMonoContainer SetSerializedProperty(
            this MockMonoContainer container, 
            string propertyName, bool value)
        {
            var serializedObject = new SerializedObject(container);
            var serializedProperty = serializedObject.FindProperty(propertyName);

            serializedProperty.boolValue = value;
            serializedObject.ApplyModifiedProperties();

            return container;
        }
        public static MockMonoContainer SetSerializedProperty(
            this MockMonoContainer container,
            string propertyName, Chopsticks.Dependencies.Containers.ContainerParentSetting value)
        {
            var serializedObject = new SerializedObject(container);
            var serializedProperty = serializedObject.FindProperty(propertyName);

            serializedProperty.enumValueFlag = (int)value;
            serializedObject.ApplyModifiedProperties();

            return container;
        }

        public static MockMonoContainer SetSerializedProperty(
            this MockMonoContainer container,
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
