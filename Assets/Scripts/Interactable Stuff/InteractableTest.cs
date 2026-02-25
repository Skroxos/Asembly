using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractible
{
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);g
    }
}
