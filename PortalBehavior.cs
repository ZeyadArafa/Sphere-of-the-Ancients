using UnityEngine;

public class PortalBehavior : MonoBehaviour
{
    private MagicSphereController magicSphere; // Reference to the Magic Sphere controller
    private bool hasTriggeredWin = false; // Flag to ensure the win condition is triggered only once

    void Start()
    {
        // Set the portal's position to X=930, Y=215, Z=1180 as per requirements
        Vector3 portalPosition = new Vector3(930f, 215f, 1180f);
        transform.position = portalPosition;
        transform.localScale = new Vector3(100f, 100f, 100f); // Set scale as per requirements
        Debug.Log($"Portal initialized at position: {transform.position}, scale: {transform.localScale}");

        // Find the Magic Sphere in the scene if not assigned
        if (magicSphere == null)
        {
            GameObject sphereObject = GameObject.FindGameObjectWithTag("Player");
            if (sphereObject != null)
            {
                magicSphere = sphereObject.GetComponent<MagicSphereController>();
                if (magicSphere != null)
                {
                    Debug.Log($"Magic Sphere reference assigned. Sphere position: {magicSphere.transform.position}");
                }
                else
                {
                    Debug.LogError("MagicSphereController component not found on the player object! Please ensure the Magic Sphere has the MagicSphereController script.");
                }
            }
            else
            {
                Debug.LogError("Player object with tag 'Player' not found in the scene! Please tag the Magic Sphere as 'Player'.");
            }
        }

        // Add a Point Light component for the blinking effect as per requirements
        Light portalLight = gameObject.GetComponent<Light>();
        if (!portalLight)
        {
            portalLight = gameObject.AddComponent<Light>();
        }
        portalLight.type = LightType.Point;
        portalLight.color = Color.blue;
        portalLight.range = 100f;
        portalLight.intensity = 8f;
        portalLight.shadows = LightShadows.Soft; // Enable shadows as per requirements
        StartCoroutine(BlinkLight(portalLight)); // Start the blinking effect
    }

    void Update()
    {
        // Skip if the win condition has already been triggered
        if (hasTriggeredWin)
        {
            return;
        }

        // Check if the Magic Sphere is within 50 units of the portal
        if (magicSphere != null)
        {
            float distanceToSphere = Vector3.Distance(transform.position, magicSphere.transform.position);
            int currentShields = magicSphere.GetCurrentShields();
            Debug.Log($"Distance to Magic Sphere: {distanceToSphere:F2}, Sphere position: {magicSphere.transform.position}, Portal position: {transform.position}, Shields: {currentShields}");

            // Check winning condition: within 50 units and has at least 1 shield
            if (distanceToSphere <= 50f && currentShields > 0)
            {
                Debug.Log($"Winning condition met! Distance to portal: {distanceToSphere:F2}, Shields: {currentShields}");
                TriggerWin();
            }
            else
            {
                if (distanceToSphere > 50f)
                {
                    Debug.Log($"Magic Sphere is too far from the portal. Distance: {distanceToSphere:F2} (must be <= 50 units)");
                }
                if (currentShields <= 0)
                {
                    Debug.Log("Magic Sphere has no shields left, cannot win.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Magic Sphere reference is not assigned! Cannot check winning condition.");
        }
    }

    void TriggerWin()
    {
        hasTriggeredWin = true; // Prevent multiple triggers
        if (magicSphere != null)
        {
            magicSphere.WinGame(); // Call the WinGame method on the Magic Sphere
            Debug.Log("WinGame triggered on Magic Sphere.");
        }
        else
        {
            Debug.LogError("Magic Sphere reference is null! Cannot trigger win behavior.");
        }
    }

    // Coroutine to handle the gradual blinking effect of the portal's light
    private System.Collections.IEnumerator BlinkLight(Light light)
    {
        float minIntensity = 4f; // Minimum intensity for the blinking effect
        float maxIntensity = 8f; // Maximum intensity (as per requirements)
        float blinkSpeed = 1f; // Speed of the blinking (adjustable for ambiance)

        while (true)
        {
            // Gradually increase intensity from min to max
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * blinkSpeed;
                light.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
                yield return null;
            }

            // Gradually decrease intensity from max to min
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * blinkSpeed;
                light.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
                yield return null;
            }
        }
    }
}