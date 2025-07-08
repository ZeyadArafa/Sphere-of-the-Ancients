using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Light pointLight;
    public float blinkSpeed = 2f;  // Adjust speed for ambiance
    public float minIntensity = 0f;
    public float maxIntensity = 8f;

    private float time;

    void Start()
    {
        if (pointLight == null)
            pointLight = GetComponent<Light>();
    }

    void Update()
    {
        time += Time.deltaTime * blinkSpeed;
        pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(time) + 1) / 2);
    }
}
