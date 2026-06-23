using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DroneAssembly.Audio;
using DroneAssembly.Radios;
using DroneAssembly.StepManager;
using UnityEngine;

namespace DroneAssembly.Spawner
{
    public class DeskSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float spacing = 0.2f;
        [SerializeField] private float groupSpacing = 0.5f;
        [SerializeField] private AudioClipRadio audioClipRadio;
        [SerializeField] private AudioConfig spawnSound;
        [SerializeField] private SpawnPartRadioSO spawnPartRadio;

        private void OnEnable()
        {
            spawnPartRadio.OnEventRaised += SpawnParts;
        }

        private void OnDisable()
        {
            spawnPartRadio.OnEventRaised -= SpawnParts;
        }

        public void SpawnParts(List<StepRequirement> requirements)
        {
            SpawnPartsAsync(requirements, this.GetCancellationTokenOnDestroy()).Forget();
        }


        // No point in using Object Pooling for this as the amount of parts is very low, and they are only spawned once per step.
        // If we were to spawn a lot of parts or spawn them multiple times, then Object Pooling would be a good idea to optimize performance.
        // Also, parts are never destroyed, they are just snapped into place.
        private async UniTaskVoid SpawnPartsAsync(List<StepRequirement> requirements, CancellationToken cancellationToken)
        {
            var initialSpawnPos = spawnPoint.position;
            foreach (var req in requirements)
            {
                var partPrefab = req.requiredPartID.partPrefab;
                if (partPrefab == null) 
                {
                    continue; 
                }
                for (var i = 0; i < req.amountRequired; i++)
                {
                    var spawnPos = initialSpawnPos + spawnPoint.right * (i * spacing);
                    var partInstance = Instantiate(partPrefab, spawnPos, Quaternion.identity);
                    var originalScale = partInstance.transform.localScale;
                    partInstance.transform.localScale = Vector3.zero;
                  await partInstance.transform.DOScale(originalScale, 0.5f)
                        .SetEase(Ease.OutBack)
                        .SetLink(partInstance);

                    if (spawnSound != null && spawnSound.audioClips.Length > 0)
                    {
                        audioClipRadio.RaiseEvent(spawnSound);
                    }
                    await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: cancellationToken);
                }

                initialSpawnPos += spawnPoint.forward * groupSpacing;
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellationToken);
            }
        }
    }
}