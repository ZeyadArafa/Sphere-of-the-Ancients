using UnityEngine;

public class SphereStatsDisplay : MonoBehaviour
{
    public Transform sphereTransform; // Magic Sphere's transform
    public Transform portalTransform; // Portal's transform
    public MagicSphereController sphereController; // Sphere's controller

    private Rect statsBox = new Rect(10, 10, 250, 120); // GUI box size

    void Start()
    {
        // Auto-assign sphereTransform
        if (!sphereTransform)
        {
            sphereTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (!sphereTransform) Debug.LogError("Sphere Transform not found! Tag the sphere as 'Player'.");
        }

        // Auto-assign portalTransform
        if (!portalTransform)
        {
            portalTransform = GameObject.FindGameObjectWithTag("Portal")?.transform;
            if (!portalTransform) Debug.LogError("Portal Transform not found! Tag the portal as 'Portal'.");
        }

        // Auto-assign sphereController
        if (!sphereController)
        {
            sphereController = sphereTransform?.GetComponent<MagicSphereController>();
            if (!sphereController) Debug.LogError("MagicSphereController not found on sphere!");
        }
    }

    void OnGUI()
    {
        if (sphereController == null || sphereTransform == null || portalTransform == null)
        {
            GUI.Label(statsBox, "Stats unavailable: Missing references!");
            return;
        }

        GUI.Box(statsBox, "Sphere Stats");

        // Exam: Remaining Armor as percentage
        float armorPercentage = (float)sphereController.GetCurrentShields() / 29f * 100f;
        // Exam: Moving Speed (Slow or Fast)
        string speedText = Input.GetKey(KeyCode.LeftShift) && IsMoving() ? "Fast" : "Slow";
        // Exam: Absolute Distance to Portal
        float distanceToPortal = Vector3.Distance(sphereTransform.position, portalTransform.position);
        // Exam: Player Status
        string statusText = GetPlayerStatus();

        GUI.Label(new Rect(statsBox.x + 10, statsBox.y + 20, statsBox.width - 20, 20),
            $"Remaining Armor: {armorPercentage:F1}%");
        GUI.Label(new Rect(statsBox.x + 10, statsBox.y + 40, statsBox.width - 20, 20),
            $"Moving Speed: {speedText}");
        GUI.Label(new Rect(statsBox.x + 10, statsBox.y + 60, statsBox.width - 20, 20),
            $"Distance to Portal: {distanceToPortal:F1} units");
        GUI.Label(new Rect(statsBox.x + 10, statsBox.y + 80, statsBox.width - 20, 20),
            $"Status: {statusText}");
    }

    private bool IsMoving()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        return x != 0 || z != 0;
    }

    private string GetPlayerStatus()
    {
        if (sphereController.HasWon()) return "Won";
        if (sphereController.HasLost()) return "Lost";
        if (IsMoving()) return "Climbing";
        return "Resting";
    }
}