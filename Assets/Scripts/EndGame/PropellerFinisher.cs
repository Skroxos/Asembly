using DG.Tweening;
using DroneAssembly.Radios.GeneralRadios;
using UnityEngine;

namespace DroneAssembly.EndGame
{
    public class PropellerFinisher : MonoBehaviour
    {
        [SerializeField] private Transform propellerRoot;
        [SerializeField] private float spinDuration = 5f;
        [SerializeField] private float spinSpeed = 360f;
        [SerializeField] private SimpleEventRadio onFinishRadio;

        private void OnEnable()
        {
            onFinishRadio.OnRaised += Spin;
        }
   
        private void OnDisable()
        {
            onFinishRadio.OnRaised -= Spin;
        }
    
        private void Spin()
        {
            propellerRoot.DOLocalRotate(new Vector3(0, spinSpeed, 0), spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental); 
        }
    }
}