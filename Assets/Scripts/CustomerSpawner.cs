using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;  

    public float spawnDelay = 1f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SpawnImmediate();
    }

    public GameObject SpawnImmediate()
    {
        if (customerPrefab == null || spawnPoint == null) return null;

        var customer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        // assign exit point
        var behaviour = customer.GetComponent<CustomerBehaviour>();
        if (behaviour != null && exitPoint != null)
            behaviour.exitPoint = exitPoint;

        return customer;
    }

    public void SpawnAfterDelay(float delay = -1f)
    {
        if (delay < 0) delay = spawnDelay;
        StartCoroutine(DoSpawn(delay));
    }

    IEnumerator DoSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnImmediate();
    }
}
