using UnityEngine;
using UnityEngine.InputSystem;

namespace DroneAssembly.VR_Port.Menu
{
    public class MenuToggle : MonoBehaviour
    {
        [SerializeField] private InputActionReference menuButtonReference;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject rightHandUIRay;

        private void OnEnable()
        {
            menuButtonReference.action.performed += ToggleMenu;
        }

        private void OnDisable()
        {
            menuButtonReference.action.performed -= ToggleMenu;
        }

        private void ToggleMenu(InputAction.CallbackContext context)
        {
            if (menu == null || rightHandUIRay == null) return;
            menu.SetActive(!menu.activeSelf);
            rightHandUIRay.SetActive(!rightHandUIRay.activeSelf);
        }
    }
}
