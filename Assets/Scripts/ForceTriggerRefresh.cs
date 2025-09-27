using UnityEngine;

public class ForceTriggerRefresh : MonoBehaviour
{
    void Start()
    {
        // Small delay ensures physics is ready
        Invoke(nameof(RefreshTriggers), 0.05f);
    }

    void RefreshTriggers()
    {
        // Re-enable collider briefly so OnTriggerEnter fires if player is inside
        var col = GetComponentInChildren<Collider>();
        if (col != null)
        {
            col.enabled = false;
            col.enabled = true;
        }
    }
}
