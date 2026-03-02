using System.Collections;
using UnityEngine;

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
        StartCoroutine(LoadSceneAsync(obj));
    }
    
    private IEnumerator LoadSceneAsync(SceneDataSO sceneData)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneData.SceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            loadingProgressRadio.RaiseEvent(asyncLoad.progress);
            yield return null;
        }
        loadingProgressRadio.RaiseEvent(1f);
        asyncLoad.allowSceneActivation = true;
        sceneLoadedRadio.RaiseEvent();
    }
}