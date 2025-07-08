using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs;  // Array for 3 rock prefabs
    public Transform player;          // Player transform
    public Transform cameraTransform; // Camera transform

    private float timeSinceLastRock = 0f;
    private float nextRockDelay;
    private float gameTimer = 0f;
    private bool spawningActive = true;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<MagicSphereController>()?.transform;
            if (player == null)
            {
                Debug.LogError("No player found! Rocks won't spawn.");
                spawningActive = false;
                return;
            }
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("No main camera found! Rocks won't spawn.");
                spawningActive = false;
                return;
            }
        }

        nextRockDelay = Random.Range(0.1f, 0.5f); // 100ms-500ms
    }

    void Update()
    {
        if (!spawningActive) return;

        gameTimer += Time.deltaTime;
        if (gameTimer < 5f) return; // Wait 5 seconds

        timeSinceLastRock += Time.deltaTime;
        if (timeSinceLastRock >= nextRockDelay)
        {
            SpawnRock();
            timeSinceLastRock = 0f;
            nextRockDelay = Random.Range(0.1f, 0.5f);
        }
    }

    void SpawnRock()
    {
        if (rockPrefabs == null || rockPrefabs.Length == 0) return;

        GameObject rockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
        Vector3 spawnPosition = CalculateSpawnPosition();
        GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
        rock.transform.localScale = new Vector3(50, 50, 50);
        rock.tag = "Rock";

        Rigidbody rb = rock.GetComponent<Rigidbody>() ?? rock.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        Vector3 randomTorque = Random.insideUnitSphere * Random.Range(200f, 400f);
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        if (!rock.GetComponent<RockBehavior>())
            rock.AddComponent<RockBehavior>();
    }

    Vector3 CalculateSpawnPosition()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0; // Keep it horizontal
        forward.Normalize();

        float randomX = Random.Range(-10f, 10f);
        float randomZ = Random.Range(30f, 40f);
        Vector3 offset = forward * randomZ + cameraTransform.right * randomX;

        Vector3 spawnPosition = player.position + offset;
        spawnPosition.y = player.position.y + 40f; // Fixed 40 units above player

        return spawnPosition;
    }

    public void StopSpawning()
    {
        spawningActive = false;
    }

    public void StartSpawning()
    {
        spawningActive = true;
        gameTimer = 0f;
        timeSinceLastRock = 0f;
        nextRockDelay = Random.Range(0.1f, 0.5f);
    }
}