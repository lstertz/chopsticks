using System;
using UnityEngine;

namespace Chopsticks.Dependencies.Containers
{
    public class MonoContainerService : IUnityContainerService<DependencyContainer>
    {
        // TODO :: Add locking for thread safety.

        public DependencyContainer GlobalContainer => _instance ??= new();
        private static DependencyContainer _instance;


        // TODO :: Implement includeSelf functionality.

        public DependencyContainer GetContainer<TUnityContainer>(ContainerRetrievalSetting setting, 
            bool includeSelf, TUnityContainer unityContainer, TUnityContainer overrideContainer)
            where TUnityContainer : MonoBehaviour, IUnityContainer<DependencyContainer> => 
            setting switch
            {
                ContainerRetrievalSetting.HierarchyWithGlobal =>
                    FindParentInHierarchy(unityContainer, true),
                ContainerRetrievalSetting.HierarchyWithoutGlobal =>
                    FindParentInHierarchy(unityContainer, false),
                ContainerRetrievalSetting.Global => 
                    GlobalContainer,
                ContainerRetrievalSetting.Override => 
                    overrideContainer.NativeContainer,
                _ => throw new NotSupportedException($"The container retrieval setting of " +
                                        $"{setting} is not supported."),
            };

        public void ResetGlobal()
        {
            _instance?.Dispose();
            _instance = null;
        }


        private DependencyContainer FindParentInHierarchy<TUnityContainer>(
            TUnityContainer unityContainer, bool defaultToGlobal)
            where TUnityContainer : MonoBehaviour, IUnityContainer<DependencyContainer>
        {
            var parentTransform = unityContainer.transform.parent;
            var parent = parentTransform == null ? null :
                    parentTransform.GetComponentInParent<MonoContainer>();

            if (parent == null)
            {
                if (defaultToGlobal)
                    return GlobalContainer;
                return default;
            }

            return (parent as IUnityContainer<DependencyContainer>).NativeContainer;
        }
    }
}
