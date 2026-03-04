using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _menuUI;

    private void Start()
    {
        CloseMenu();
    }

    private void OnEnable()
    {
        _inputReader.MenuToggleEvent += ToggleMenuUI;
    }


    private void OnDisable()
    {
        _inputReader.MenuToggleEvent -= ToggleMenuUI;
    }

    private void ToggleMenuUI()
    {
        _menuUI.SetActive(!_menuUI.activeSelf);
        if (_menuUI.activeSelf)
            OpenMenu();
        else
            CloseMenu();
    }

    private void OpenMenu()
    {
        _menuUI.SetActive(true);
        _inputReader.DisableGameplayInput();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseMenu()
    {
        _menuUI.SetActive(false);
        _inputReader.EnableGameplayInput();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}