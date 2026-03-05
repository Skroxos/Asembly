using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
        StopAllCoroutines();
        StartCoroutine(SpawnPartsCoroutine(requirements));
    }


    // No point in using Object Pooling for this as the amount of parts is very low, and they are only spawned once per step.
    // If we were to spawn a lot of parts or spawn them multiple times, then Object Pooling would be a good idea to optimize performance.
    // Also, parts are never destroyed, they are just snapped into place.
    private IEnumerator SpawnPartsCoroutine(List<StepRequirement> requirements)
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
                partInstance.transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);

                if (spawnSound != null && spawnSound.audioClips.Length > 0) audioClipRadio.RaiseEvent(spawnSound);
                yield return new WaitForSeconds(0.2f);
            }

            initialSpawnPos += spawnPoint.forward * groupSpacing;
            yield return new WaitForSeconds(0.5f);
        }
    }
}