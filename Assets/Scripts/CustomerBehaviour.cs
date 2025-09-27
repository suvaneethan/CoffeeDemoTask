using UnityEngine;
using System.Collections;

public class CustomerBehaviour : MonoBehaviour
{
    [Tooltip("Money rewarded when customer is served")]
    public int price = 10;

    [Header("Exit movement")]
    public Transform exitPoint;   // assigned by spawner
    public float moveSpeed = 2f;

    bool served = false;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null && player.HasCup() && !served)
        {
            served = true;

            // Drop & destroy cup
            var cup = player.DropCupAt(transform.position + Vector3.up * 0.2f);
            if (cup) Destroy(cup);

            // Reward
            GameManager.Instance?.AddMoney(price, transform.position);

            // Start exit
            if (exitPoint != null)
                StartCoroutine(MoveAndExit());
            else
            {
                // fallback: just despawn + spawn new one
                CustomerSpawner.Instance?.SpawnAfterDelay(0.8f);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator MoveAndExit()
    {
        // Move until reaching exit point
        while (Vector3.Distance(transform.position, exitPoint.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                exitPoint.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Now spawn a new customer (AFTER this one left)
        CustomerSpawner.Instance?.SpawnAfterDelay(0.5f);

        // Destroy old customer
        Destroy(gameObject);
    }
}
