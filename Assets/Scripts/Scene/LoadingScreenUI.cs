using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private LoadingProgressRadio loadingProgressRadio;
    [SerializeField] private Slider progressSlider;
    
    private void OnEnable()
    {
        loadingProgressRadio.OnEventRaised += UpdateProgress;
    }
    
    private void OnDisable()
    {
        loadingProgressRadio.OnEventRaised -= UpdateProgress;
    }

    private void UpdateProgress(float obj)
    {
        progressSlider.gameObject.SetActive(true);
        progressSlider.value = obj;
    }
}