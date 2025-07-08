using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RockBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private float minVelocity = 0.1f;
    private bool hasCollidedWithTerrain = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            Debug.Log("Rock Rigidbody - isKinematic: " + rb.isKinematic + ", UseGravity: " + rb.useGravity);
        }
        else
        {
            Debug.LogError("Rock missing Rigidbody!", gameObject);
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false;
            Debug.Log("Rock Collider set as non-trigger: " + !collider.isTrigger);
        }
        else
        {
            collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = false;
            Debug.Log("Added SphereCollider to rock, isTrigger: " + collider.isTrigger);
        }

        gameObject.tag = "Rock";
        gameObject.layer = LayerMask.NameToLayer("Rocks");
        Debug.Log("Rock initialized at position: " + transform.position + ", Layer: " + LayerMask.LayerToName(gameObject.layer));
    }

    void Update()
    {
        if (hasCollidedWithTerrain &&
            rb.linearVelocity.magnitude < minVelocity &&
            rb.angularVelocity.magnitude < minVelocity)
        {
            Debug.Log("Rock stopped moving on terrain, destroying: " + gameObject.name);
            Destroy(gameObject);
        }
        else if (transform.position.y < -10f) // Cleanup if falls below terrain
        {
            Debug.LogWarning("Rock fell below y=-10, destroying: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public bool IsMoving()
    {
        bool moving = rb.linearVelocity.magnitude >= minVelocity ||
                      rb.angularVelocity.magnitude >= minVelocity;
        Debug.Log("Rock speed: " + rb.linearVelocity.magnitude + ", Angular speed: " + rb.angularVelocity.magnitude + ", IsMoving: " + moving);
        return moving;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Rock collided with: " + collision.gameObject.name + " on layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            hasCollidedWithTerrain = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Collider col = GetComponent<Collider>();
        if (col is SphereCollider sc)
        {
            Gizmos.DrawWireSphere(transform.position, sc.radius);
        }
        else if (col is BoxCollider bc)
        {
            Gizmos.DrawWireCube(transform.position, bc.size);
        }
    }
}