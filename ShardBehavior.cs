using UnityEngine;

public class ShardBehavior : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Shard trigger entered by: " + other.gameObject.name + " on layer: " + LayerMask.LayerToName(other.gameObject.layer));

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MagicSphereController controller = other.GetComponent<MagicSphereController>();
            if (controller != null)
            {
                controller.CollectShard(); // Gain 1 shield
                Debug.Log($"Shard collected at {transform.position} by {other.gameObject.name}!");
                Destroy(gameObject); // Shard disappears
            }
            else
            {
                Debug.LogError("No MagicSphereController found on player object: " + other.gameObject.name);
            }
        }
        else
        {
            Debug.Log("Shard trigger ignored: Not the Player layer.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Shard collided with: " + collision.gameObject.name + " on layer: " + LayerMask.LayerToName(collision.gameObject.layer));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc != null) Gizmos.DrawWireSphere(transform.position, sc.radius);
    }
}