using UnityEngine;

[RequireComponent(typeof(SocketController))]
public class SocketAudioComponent : MonoBehaviour
{
    [SerializeField] private AudioConfig snapSound;
    [SerializeField] private AudioClipRadio audioClipRadio;
    private SocketController _socketController;


    private void Awake()
    {
        _socketController = GetComponent<SocketController>();
    }

    private void OnEnable()
    {
        _socketController.OnPartSnapped += HandlePartSnapped;
    }


    private void OnDisable()
    {
        _socketController.OnPartSnapped -= HandlePartSnapped;
    }

    private void HandlePartSnapped()
    {
        if (snapSound != null && snapSound.audioClips.Length > 0) audioClipRadio.RaiseEvent(snapSound);
    }
}