using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private InputReader inputReader;

    private void OnEnable()
    {
        inputReader.MenuToggleEvent += ToggleUI;
    }


    private void OnDisable()
    {
        inputReader.MenuToggleEvent -= ToggleUI;
    }
    private void ToggleUI()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
    }
}