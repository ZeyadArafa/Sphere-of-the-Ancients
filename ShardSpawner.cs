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
        shard.layer = LayerMask.NameToLayer("Shards"); // Set layer
        shard.transform.localScale = new Vector3(10, 10, 10); // Scale as per exam
        shard.tag = "Shards"; // Tag for debugging

        // Ensure ShardBehavior is present
        if (!shard.GetComponent<ShardBehavior>())
        {
            shard.AddComponent<ShardBehavior>();
            Debug.Log("ShardBehavior added to spawned shard.");
        }

        // Ensure Rigidbody is present and configured
        Rigidbody rb = shard.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = shard.AddComponent<Rigidbody>();
            Debug.Log("Rigidbody added to shard.");
        }
        rb.useGravity = true; // Ensure gravity works
        rb.isKinematic = false; // Non-kinematic for physics
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevent tunneling

        // Ensure Collider is present and NOT a trigger for terrain collision
        Collider col = shard.GetComponent<Collider>();
        if (col == null)
        {
            col = shard.AddComponent<BoxCollider>(); // Use BoxCollider for terrain collision
            Debug.Log("BoxCollider added to shard.");
        }
        col.isTrigger = false; // Non-trigger to stop on terrain

        // Add a separate trigger collider for player collection
        if (!shard.GetComponent<SphereCollider>())
        {
            SphereCollider triggerCol = shard.AddComponent<SphereCollider>();
            triggerCol.isTrigger = true; // Trigger for player detection
            triggerCol.radius = 2f; // Large enough to detect player
            Debug.Log("Trigger SphereCollider added to shard.");
        }

        Debug.Log("Shard spawned at position: " + spawnPosition + " with layer: " + LayerMask.LayerToName(shard.layer));
    }

    Vector3 CalculateSpawnPosition()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        float randomX = Random.Range(-10f, 10f); // X-axis: -10 to 10 as per exam
        float randomZ = Random.Range(30f, 40f);  // Z-axis: 30 to 40 as per exam

        Vector3 spawnOffset = forward * randomZ + cameraTransform.right * randomX;
        Vector3 spawnPosition = player.position + spawnOffset;

        // Raycast to find terrain height
        Ray ray = new Ray(spawnPosition + Vector3.up * 50f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Default")))
        {
            spawnPosition.y = hit.point.y + 5f; // Spawn 5 units above terrain
            Debug.Log("Shard spawn height adjusted to terrain: " + spawnPosition.y);
        }
        else
        {
            spawnPosition.y = player.position.y + 5f; // Fallback: 5 above player
            Debug.LogWarning("Raycast failed to hit terrain! Using player height + 5: " + spawnPosition.y);
        }

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
}