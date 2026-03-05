using UnityEditor;
using UnityEngine;

namespace DroneAssembly.Scene
{
    [CreateAssetMenu(menuName = "Scene/SceneData")]
    public class SceneDataSO : ScriptableObject
    {
        public string SceneName { get; private set; }
        public int BuildIndex { get; private set; }

#if UNITY_EDITOR
        [SerializeField] private SceneAsset sceneAsset;

        private void OnValidate()
        {
            if (sceneAsset != null) SceneName = sceneAsset.name;
        }
#endif
    }
}