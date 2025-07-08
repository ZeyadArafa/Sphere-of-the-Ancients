using UnityEngine;

public class MagicSphereSetup : MonoBehaviour
{
    public Transform coreObject; // Core sphere object
    private GameObject corePointLightObject; // GameObject for the Point Light
    private Light corePointLight; // Point Light component

    public Transform[] shieldParts; // 29 shield parts
    private Renderer[] shieldRenderers; // Shield part renderers

    private Color darkerBlue = new Color(0f, 0f, 0.5f, 1f); // Darker blue for shield
    private Color defaultGold = new Color(1f, 0.843f, 0f, 0.5f); // Transparent gold for core

    void Start()
    {
        // Set world position
        transform.position = new Vector3(1850f, 1.5f, 80f);

        // Setup core sphere material and attach light
        SetupCoreObject();

        // Setup shield materials & shadows
        SetupShields();
    }

    private void SetupCoreObject()
    {
        if (!coreObject)
        {
            Debug.LogError("Core Object not assigned!");
            return;
        }

        Renderer coreRenderer = coreObject.GetComponent<Renderer>();
        if (!coreRenderer)
        {
            Debug.LogError("Core Object has no Renderer component!");
            return;
        }

        // Apply transparent gold material
        Material goldMaterial = new Material(Shader.Find("Standard"));
        goldMaterial.color = defaultGold;
        goldMaterial.SetFloat("_Mode", 3); // Enable transparency
        goldMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        goldMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        goldMaterial.SetInt("_ZWrite", 0);
        goldMaterial.DisableKeyword("_ALPHATEST_ON");
        goldMaterial.EnableKeyword("_ALPHABLEND_ON");
        goldMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        goldMaterial.renderQueue = 3000;

        coreRenderer.material = goldMaterial;

        // Create a separate GameObject for the Point Light
        corePointLightObject = new GameObject("CorePointLight");
        corePointLightObject.transform.SetParent(coreObject); // Attach it to the Core Sphere
        corePointLightObject.transform.localPosition = Vector3.zero; // Center it inside the sphere

        corePointLight = corePointLightObject.AddComponent<Light>();
        corePointLight.type = LightType.Point;
        corePointLight.color = new Color(239f / 255f, 248f / 255f, 207f / 255f); // As per exam: (239,248,207)
        corePointLight.range = 10f; // As per exam requirements
        corePointLight.intensity = 10f; // As per exam requirements
        corePointLight.shadows = LightShadows.Soft; // Enable soft shadows as per exam preference
        corePointLight.shadowStrength = 1f; // Maximum shadow strength for visibility
        corePointLight.shadowBias = 0.05f; // Default bias to prevent shadow acne
        corePointLight.shadowNormalBias = 0.4f; // Adjust for better shadow quality on curved surfaces
        corePointLight.shadowNearPlane = 0.1f; // Tight near plane for precision
        corePointLight.renderMode = LightRenderMode.ForcePixel; // Highest priority rendering (pixel lighting)
    }

    private void SetupShields()
    {
        if (shieldParts == null || shieldParts.Length != 29)
        {
            Debug.LogError("Shield parts array is not set or has incorrect size!");
            return;
        }

        shieldRenderers = new Renderer[shieldParts.Length];

        for (int i = 0; i < shieldParts.Length; i++)
        {
            if (shieldParts[i] != null)
            {
                shieldRenderers[i] = shieldParts[i].GetComponent<Renderer>();
                if (shieldRenderers[i] != null)
                {
                    shieldRenderers[i].material = new Material(Shader.Find("Standard"));
                    shieldRenderers[i].material.color = darkerBlue;

                    // Enable Shadow Casting
                    shieldRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    shieldRenderers[i].receiveShadows = true;
                }
            }
        }
    }
}