using UnityEngine;

public class ShardSpawner : MonoBehaviour
{
    public GameObject[] shardPrefabs; // Array for shard prefabs
    public Transform player;          // Player transform, auto-assigned if null
    public Transform cameraTransform; // Camera transform, auto-assigned if null

    private float shardTimer = 0f;
    private bool spawningActive = true;

    void Start()
    {
        // Auto-assign player if not set in Inspector
        if (player == null)
        {
            MagicSphereController playerController = FindObjectOfType<MagicSphereController>();
            if (playerController != null)
            {
                player = playerController.transform;
                Debug.Log("Player auto-assigned in ShardSpawner to: " + player.gameObject.name);
            }
            else
            {
                Debug.LogError("No MagicSphereController found in scene! Shards won't spawn.");
                spawningActive = false;
                return;
            }
        }

        // Auto-assign cameraTransform if not set in Inspector
        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
                Debug.Log("CameraTransform auto-assigned to Main Camera in ShardSpawner.");
            }
            else
            {
                Debug.LogError("No main camera found in scene! Shards won't spawn.");
                spawningActive = false;
                return;
            }
        }

        Debug.Log("ShardSpawner initialized.");
    }

    void Update()
    {
        if (!spawningActive)
        {
            return;
        }

        shardTimer += Time.deltaTime;
        if (shardTimer >= 5f)
        {
            if (Random.value <= 0.25f) // 25% probability as per exam
            {
                SpawnShard();
                Debug.Log("Shard spawned due to 25% probability check.");
            }
            shardTimer = 0f;
        }
    }

    void SpawnShard()
    {
        if (shardPrefabs == null || shardPrefabs.Length == 0)
        {
            Debug.LogWarning("No shard prefabs assigned in ShardSpawner, skipping spawn.");
            return;
        }

        GameObject shardPrefab = shardPrefabs[Random.Range(0, shardPrefabs.Length)];
        if (shardPrefab == null)
        {
            Debug.LogWarning("A null prefab was found in shardPrefabs, skipping spawn.");
            return;
        }

        Vector3 spawnPosition = CalculateSpawnPosition();
        GameObject shard = Instantiate(shardPrefab, spawnPosition, Quaternion.identity);
        shard.layer = LayerMask.NameToLayer("Shards"); // Set layer as per exam
        shard.transform.localScale = new Vector3(10, 10, 10); // Scale as per exam
        shard.tag = "Shards"; // Tag for identification

        // Add ShardBehavior if not already on the prefab
        if (!shard.GetComponent<ShardBehavior>())
        {
            shard.AddComponent<ShardBehavior>();
        }

        Rigidbody rb = shard.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddTorque(Random.Range(-50f, 50f), Random.Range(-50f, 50f), Random.Range(-50f, 50f));
        }
        Debug.Log("Shard spawned at position: " + spawnPosition);
    }

    Vector3 CalculateSpawnPosition()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        float randomX = Random.Range(-10f, 10f); // X-axis: -10 to 10 as per exam
        float randomZ = Random.Range(30f, 40f);  // Z-axis: 30 to 40 as per exam
        float initialHeight = 40f;               // Y-axis: 40 (fixed) as per exam

        Vector3 spawnOffset = forward * randomZ + cameraTransform.right * randomX;
        Vector3 spawnPosition = player.position + spawnOffset;
        spawnPosition.y = initialHeight;
        Debug.Log("Calculated spawn position: " + spawnPosition);
        return spawnPosition;
    }

    public void StopSpawning()
    {
        spawningActive = false;
        shardTimer = 0f;
        Debug.Log("ShardSpawner stopped: No more shards will fall.");
    }

    public void StartSpawning()
    {
        spawningActive = true;
        shardTimer = 0f;
        Debug.Log("ShardSpawner restarted.");
    }

    // Nested ShardBehavior class to handle collection
}