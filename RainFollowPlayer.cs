using UnityEngine;

public class RainFollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the Magical Sphere's transform
    public float heightAbovePlayer = 40f; // Distance above the player

    void Update()
    {
        if (player != null)
        {
            // Update the rain's position to follow the player on X and Z, keeping Y fixed above
            Vector3 newPosition = new Vector3(player.position.x, player.position.y + heightAbovePlayer, player.position.z);
            transform.position = newPosition;
        }
    }
}