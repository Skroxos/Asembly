using DroneAssembly.Radios;
using UnityEngine;

namespace DroneAssembly.Scene
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private SceneDataSO gameplaySceneData;
        [SerializeField] private SceneTransitionRadio sceneTransitionRadio;

        public void OnPlayButtonClicked()
        {
            sceneTransitionRadio.RaiseEvent(gameplaySceneData);
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}