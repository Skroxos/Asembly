using UnityEngine;

#if  UNITY_EDITOR
using UnityEditor;
#endif

namespace DroneAssembly.Scene
{
    [CreateAssetMenu(menuName = "Scene/SceneData")]
    public class SceneDataSO : ScriptableObject
    {
        [SerializeField] private string sceneName;
        public string SceneName => sceneName;

#if UNITY_EDITOR
        [SerializeField] private SceneAsset sceneAsset;

        private void OnValidate()
        {
            if (sceneAsset != null) 
            {
                sceneName = sceneAsset.name;
            }
            else 
            {
                sceneName = "";
            }
        }
#endif
    }
}