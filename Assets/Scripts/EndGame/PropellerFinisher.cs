using System;
using DG.Tweening;
using UnityEngine;

public class PropellerFinisher : MonoBehaviour
{
    [SerializeField] private Transform propellerRoot;
    [SerializeField] private float spinDuration = 5f;
    [SerializeField] private float spinSpeed = 360f;

    private void OnEnable()
    {
        DroneFinisher.OnFinish += Spin;
    }
   
    private void OnDisable()
    {
        DroneFinisher.OnFinish -= Spin;
    }
    
    private void Spin()
    {
        propellerRoot.DOLocalRotate(new Vector3(0, spinSpeed, 0), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental); 
    }
}