using UnityEngine;

[RequireComponent(typeof(SocketController))]
public class SocketAudioComponent : MonoBehaviour
{
    private SocketController socketController;
    [SerializeField] private AudioConfig snapSound;
    [SerializeField] private AudioClipRadio audioClipRadio;
    
    
    private void Awake()
    {
        socketController = GetComponent<SocketController>();
    }
    
    private void OnEnable()
    {
        socketController.OnPartSnapped += HandlePartSnapped;
    }


    private void OnDisable()
    {
        socketController.OnPartSnapped -= HandlePartSnapped;
    }
    private void HandlePartSnapped()
    {
        if (snapSound != null && snapSound.audioClips.Length > 0)
        {
            audioClipRadio.RaiseAudioClipUpdate(snapSound);
        }
    }
   
}