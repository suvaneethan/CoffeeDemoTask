using UnityEngine;
using System;
using System.Collections;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Transform counterPoint;
    public GameObject cupPrefab;

    [Header("Processing")]
    public float processTime = 2f;
    public bool parentCupToCounter = true;

    private bool isProcessing = false;

    // Events (optional for UI)
    public Action<float> onProgress;
    public Action<bool> onProcessingChanged;

    public void Interact(PlayerController player)
    {
        //  Busy check
        if (isProcessing) return;
        if (player == null || player.stackManager == null) return;
        if (player.stackManager.BagCount <= 0) return;

        //  Consume exactly 1 bag
        player.stackManager.RemoveAndDestroyTopBag();

        //  Start processing
        StartCoroutine(Process(player));
    }

    private IEnumerator Process(PlayerController player)
    {
        isProcessing = true;
        onProcessingChanged?.Invoke(true);

        float elapsed = 0f;
        while (elapsed < processTime)
        {
            elapsed += Time.deltaTime;
            onProgress?.Invoke(Mathf.Clamp01(elapsed / processTime));
            yield return null;
        }

        onProgress?.Invoke(1f);

        // ✅ Spawn cup
        if (cupPrefab != null && counterPoint != null)
        {
            Vector3 spawnPos = counterPoint.position + counterPoint.forward * 0.2f + Vector3.up * 0.05f;
            GameObject cup = Instantiate(cupPrefab, spawnPos, counterPoint.rotation);

            // Refresh colliders
            Physics.SyncTransforms();

            // Make stable
            var rb = cup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            if (parentCupToCounter)
            {
                cup.transform.SetParent(counterPoint, worldPositionStays: true);
            }

            //  Manually assign cup as interactable if player is nearby
            var pickup = cup.GetComponentInChildren<CupPickup>();
            if (pickup != null)
            {
                PlayerController pc = FindObjectOfType<PlayerController>();
                if (pc != null)
                {
                    float dist = Vector3.Distance(pc.transform.position, cup.transform.position);
                    if (dist <= pc.interactDistance)
                    {
                        // Force set interactable
                        var field = typeof(PlayerController).GetField("currentInteractable",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (field != null) field.SetValue(pc, pickup);
                    }
                }
            }
        }


        isProcessing = false;
        onProcessingChanged?.Invoke(false);
    }

    public bool IsProcessing() => isProcessing;
}
