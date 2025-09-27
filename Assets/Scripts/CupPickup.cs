using UnityEngine;

public class CupPickup : MonoBehaviour, IInteractable
{
    [Tooltip("Assign the root cup object (with Rigidbody + Collider)")]
    public Transform cupRoot;

    public void Interact(PlayerController player)
    {
        if (player == null) return;
        if (player.HasCup()) return; // only allow one cup

        GameObject cup = (cupRoot != null) ? cupRoot.gameObject : gameObject;
        player.PickupCup(cup);
    }
}
