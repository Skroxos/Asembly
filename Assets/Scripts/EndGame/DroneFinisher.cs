using System;
using System.Collections;
using DG.Tweening;
using DroneAssembly.Radios.GeneralRadios;
using UnityEngine;

namespace DroneAssembly.EndGame
{
   public class DroneFinisher : MonoBehaviour
   {
      [SerializeField] private Transform droneRoot;
      [SerializeField] private float warmUpDuration = 2f;
      [SerializeField] private float flightHeight = 10f;
      [SerializeField] private float flightDuration = 5f;
      [SerializeField] private SimpleEventRadio droneFinisherRadio;
      [SerializeField] private SimpleEventRadio finishRadio;
   
      public static event Action OnFinish;
      private void OnEnable()
      {
         droneFinisherRadio.OnRaised += Finish;
      }
   
      private void OnDisable()
      {
         droneFinisherRadio.OnRaised -= Finish;
      }
   
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
         yield return new WaitForSeconds(flightDuration);
         finishRadio.RaiseEvent();
      }
   }
}