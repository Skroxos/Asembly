using Cysharp.Threading.Tasks;
using DG.Tweening;
using DroneAssembly.Radios.GeneralRadios;
using System;
using System.Collections;
using System.Threading;
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
      [SerializeField] private SimpleEventRadio onFinishRadio;
      [SerializeField] private AudioSource audioSource;
      
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
         FinishSequence().Forget();
      }

   
      private async UniTaskVoid FinishSequence()
      {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            audioSource.Play();
            onFinishRadio.RaiseEvent();
            await UniTask.Delay(TimeSpan.FromSeconds(warmUpDuration), cancellationToken: token);

            await droneRoot.DOMoveY(droneRoot.position.y + flightHeight, flightDuration)
            .SetEase(Ease.InQuad)
            .ToUniTask(cancellationToken: token);

            finishRadio.RaiseEvent();
      }
   }
}