using UnityEngine;

public class RainSplashSpawner : MonoBehaviour
{
    public ParticleSystem splashEffect;
    public Transform player;
    public int splashCount = 50;
    public float radius = 20f;

    void Update()
    {
        for (int i = 0; i < splashCount; i++)
        {
            Vector3 randomPos = player.position + new Vector3(
                Random.Range(-radius, radius),
                0f,
                Random.Range(-radius, radius)
            );

            Ray ray = new Ray(randomPos + Vector3.up * 10f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 20f))
            {
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
                {
                    position = hit.point,
                    startColor = new Color(1, 1, 1, 0.7f),
                    startSize = Random.Range(0.3f, 0.5f)
                };
                splashEffect.Emit(emitParams, 1);
            }
        }
    }
}
