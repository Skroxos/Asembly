using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DroneFinisher : MonoBehaviour
{
   [SerializeField] private Transform droneRoot;
   [SerializeField] private float warmUpDuration = 2f;
   [SerializeField] private float flightHeight = 10f;
   [SerializeField] private float flightDuration = 5f;
   [SerializeField] private SimpleEventRadio droneFinisherRadio;
   
   private void OnEnable()
   {
      droneFinisherRadio.OnRaised += Finish;
   }
   
   private void OnDisable()
   {
      droneFinisherRadio.OnRaised -= Finish;
   }
   public static event Action OnFinish;
   
   private void Finish()
   {
      StartCoroutine(FinishSequence());
   }

   
   private IEnumerator FinishSequence()
   {
      OnFinish?.Invoke();
      yield return new WaitForSeconds(warmUpDuration);
      
      droneRoot.DOMoveY(droneRoot.position.y + flightHeight, flightDuration)
         .SetEase(Ease.InQuad); 
    
      droneRoot.DORotate(new Vector3(15, 0, 0), flightDuration * 0.5f);
   }
}