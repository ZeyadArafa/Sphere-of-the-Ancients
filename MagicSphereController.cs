using UnityEngine;

public class MagicSphereController : MonoBehaviour
{
    public Rigidbody rb;
    public Transform cima;
    public GameObject[] shields;
    public GameObject coreObject;
    public Transform portal;
    public RockSpawner rockSpawner;  // Assign in Inspector
    public ShardSpawner shardSpawner; // Assign in Inspector

    public float walkTorque = 50f;
    public float sprintTorque = 100f;
    public float maxAngularVelocity = 100f;
    public float jumpThrust = 50f;
    public float sprintJumpThrust = 100f;

    private int currentShields = 29;
    private float immunityTimer = 0f;
    private bool isImmune = false;
    private bool gameOver = false;
    private bool isGrounded;
    private Color normalColor = Color.blue;
    private Color immuneColor = Color.red;
    private Color winColor = Color.green;
    private Color goldColor = new Color32(255, 215, 0, 255);
    private Material sphereMaterial;
    private Color baseColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 10f;
        rb.maxAngularVelocity = maxAngularVelocity;
        if (cima == null) cima = Camera.main?.transform;

        Vector3 startPosition = new Vector3(1850f, 1.5f, 80f);
        Ray ray = new Ray(startPosition + Vector3.up * 100f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Default")))
        {
            startPosition.y = hit.point.y + 1f;
            transform.position = startPosition;
            Debug.Log($"Magic Sphere positioned at: {transform.position} (adjusted Y to terrain height)");
        }
        else
        {
            transform.position = startPosition;
            Debug.LogWarning($"Raycast did not hit the terrain! Magic Sphere placed at: {transform.position}");
        }

        if (coreObject != null)
        {
            coreObject.SetActive(true);
            Renderer coreRenderer = coreObject.GetComponent<Renderer>();
            if (coreRenderer != null)
            {
                sphereMaterial = coreRenderer.material;
                baseColor = sphereMaterial.color;
                coreRenderer.material.color = goldColor;
            }
            Light coreLight = coreObject.GetComponent<Light>();
            if (!coreLight) coreLight = coreObject.AddComponent<Light>();
            coreLight.color = goldColor;
            coreLight.range = 5f;
            coreLight.intensity = 5f;
        }

        gameObject.layer = LayerMask.NameToLayer("Player");
        currentShields = 29;

        if (shields != null && shields.Length > 0)
        {
            foreach (GameObject shield in shields)
            {
                if (shield != null) shield.SetActive(false);
            }
        }

        UpdateShieldVisuals();

        if (portal == null)
        {
            Debug.LogError("Portal is not assigned in MagicSphereController!");
        }

        if (rockSpawner == null)
        {
            Debug.LogError("RockSpawner not assigned in MagicSphereController! Rocks won't stop when losing.");
        }
        else
        {
            Debug.Log("RockSpawner assigned: " + rockSpawner.gameObject.name);
        }

        if (shardSpawner == null)
        {
            Debug.LogError("ShardSpawner not assigned in MagicSphereController! Shards won't stop when losing.");
        }
        else
        {
            Debug.Log("ShardSpawner assigned: " + shardSpawner.gameObject.name);
        }
    }

    void FixedUpdate()
    {
        if (gameOver) return;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        HandleMovement();
        HandleJumping();

        if (portal != null && Vector3.Distance(transform.position, portal.position) <= 50f && currentShields > 0)
        {
            WinGame();
        }
    }

    void Update()
    {
        if (gameOver) return;

        if (isImmune)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0f)
            {
                isImmune = false;
                UpdateShieldVisuals();
            }
        }
    }

    void HandleMovement()
    {
        if (gameOver) return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 moveVector = new Vector3(x, 0, z);
        moveVector = Quaternion.AngleAxis(cima.rotation.eulerAngles.y, Vector3.up) * moveVector;
        moveVector.Normalize();
        float torqueMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintTorque : walkTorque;
        if (moveVector != Vector3.zero)
        {
            Vector3 torque = new Vector3(moveVector.z, 0, -moveVector.x) * torqueMultiplier;
            rb.AddTorque(torque, ForceMode.Force);
            Quaternion lookAt = Quaternion.LookRotation(moveVector, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt, 360 * Time.deltaTime);
        }
    }

    void HandleJumping()
    {
        if (gameOver) return;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            float appliedJumpThrust = Input.GetKey(KeyCode.LeftShift) ? sprintJumpThrust : jumpThrust;
            rb.AddForce(Vector3.up * appliedJumpThrust, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameOver) return;

        if (collision.gameObject.CompareTag("Rock") && !isImmune)
        {
            RockBehavior rock = collision.gameObject.GetComponent<RockBehavior>();
            if (rock != null && rock.IsMoving())
            {
                Debug.Log("Rock hit player, taking damage");
                TakeDamage();
            }
        }
    }

    public void CollectShard()
    {
        if (gameOver) return;

        if (currentShields < 29)
        {
            currentShields++;
            UpdateShieldVisuals();
            Debug.Log($"Shield collected, current shields: {currentShields}");
        }
        else
        {
            Debug.Log("Shields at max (29), shard not collected.");
        }
    }

    void TakeDamage()
    {
        if (gameOver) return;

        if (currentShields > 0)
        {
            currentShields--;
            Debug.Log($"Shields left: {currentShields}");
            UpdateShieldVisuals();
            isImmune = true;
            immunityTimer = 5f;
            SetShieldColor(immuneColor);
        }
        if (currentShields <= 0)
        {
            LoseGame();
        }
    }

    void UpdateShieldVisuals()
    {
        if (shields != null && shields.Length > 0)
        {
            for (int i = 0; i < shields.Length; i++)
            {
                if (shields[i] != null)
                {
                    bool shouldBeActive = i < currentShields;
                    shields[i].SetActive(shouldBeActive);
                    if (i < currentShields && !isImmune)
                    {
                        shields[i].GetComponent<Renderer>().material.color = normalColor;
                    }
                }
            }
        }
    }

    void SetShieldColor(Color color)
    {
        if (shields != null && shields.Length > 0)
        {
            for (int i = 0; i < currentShields; i++)
            {
                if (shields[i] != null)
                {
                    shields[i].GetComponent<Renderer>().material.color = color;
                }
            }
        }
    }

    public void WinGame()
    {
        gameOver = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (rockSpawner != null)
        {
            rockSpawner.StopSpawning();
            Debug.Log("RockSpawner stopped in WinGame.");
        }
        if (shardSpawner != null)
        {
            shardSpawner.StopSpawning();
            Debug.Log("ShardSpawner stopped in WinGame.");
        }

        SetShieldColor(winColor);
        StartCoroutine(AscendSphere());
        Debug.Log("You Win!");
    }

    void LoseGame()
    {
        gameOver = true;
        Debug.Log("LoseGame called: Player has lost.");

        if (rockSpawner != null)
        {
            rockSpawner.StopSpawning();
            Debug.Log("RockSpawner stopped in LoseGame: Falling rocks should cease.");
        }
        else
        {
            Debug.LogError("RockSpawner not assigned in LoseGame!");
        }

        if (shardSpawner != null)
        {
            shardSpawner.StopSpawning();
            Debug.Log("ShardSpawner stopped in LoseGame: Falling shards should cease.");
        }
        else
        {
            Debug.LogError("ShardSpawner not assigned in LoseGame!");
        }

        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (coreObject != null && currentShields <= 0)
        {
            Renderer coreRenderer = coreObject.GetComponent<Renderer>();
            if (coreRenderer != null)
            {
                coreRenderer.material.color = Color.black;
                Debug.Log("Core turned black.");
            }
        }

        Debug.Log("Game Over!");
    }

    System.Collections.IEnumerator AscendSphere()
    {
        while (true)
        {
            transform.Translate(Vector3.up * 0.1f, Space.World);
            transform.Rotate(Vector3.one * 1f, Space.Self);
            yield return null;
        }
    }

    public int GetCurrentShields()
    {
        return currentShields;
    }

    public bool HasWon()
    {
        return gameOver && currentShields > 0;
    }

    public bool HasLost()
    {
        return gameOver && currentShields <= 0;
    }
}