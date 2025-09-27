using UnityEngine;

public class ResourceArea : MonoBehaviour, IInteractable
{
    [Tooltip("If true then entering trigger auto picks a bag")]
    public bool autoPickup = false;

    public void Interact(PlayerController player)
    {
        if (player == null || player.stackManager == null) return;
        player.stackManager.TryAddBag();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!autoPickup) return;
        var p = other.GetComponent<PlayerController>();
        if (p != null && p.stackManager != null) p.stackManager.TryAddBag();
    }
}
