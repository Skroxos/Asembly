using Cysharp.Threading.Tasks;
using DroneAssembly.Radios;
using DroneAssembly.Radios.GeneralRadios;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DroneAssembly.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneDataSO sceneData;
        [SerializeField] private SceneTransitionRadio sceneTransitionRadio;
        [SerializeField] private SimpleEventRadio sceneLoadedRadio;
        [SerializeField] private LoadingProgressRadio loadingProgressRadio;

        private void OnEnable()
        {
            sceneTransitionRadio.OnEventRaised += HandleSceneTransition;
        }


        private void OnDisable()
        {
            sceneTransitionRadio.OnEventRaised -= HandleSceneTransition;
        }

        private void HandleSceneTransition(SceneDataSO obj)
        {
            LoadSceneAsync(obj, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid LoadSceneAsync(SceneDataSO sceneData,CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(sceneData.SceneName).WithCancellation(token);
            sceneLoadedRadio.RaiseEvent();
        }
    }
}