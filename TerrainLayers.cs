using UnityEngine;

public class TerrainLayers : MonoBehaviour
{
    private Terrain terrain;
    public float[] thresholds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrain = gameObject.GetComponent<Terrain>();
        ApplyLayersBasedOnHeight();
    }

    void ApplyLayersBasedOnHeight()
    {
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = terrainData.alphamapLayers;

        float maxHeight = GetMaxHeight();

        float[,,] splatMap = new float[height, width, layers];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float h = terrainData.GetHeight(y, x);
                float normalizedHeight = h / maxHeight;
                int layerIndex = GetLayerBasedOnHeight(normalizedHeight);


                for(int l = 0; l<layers; l++)
                {
                    if (l == layerIndex)
                        splatMap[x, y, l] = 1f;
                    else
                        splatMap[x, y, l] = 0f;
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatMap);
    }

    int GetLayerBasedOnHeight(float height)
    {
        TerrainData terrainData = terrain.terrainData;
        if (terrainData.alphamapLayers == 1)
            return 0;

        int l;
        for(l = 0; l < thresholds.Length ; l++)
        {
            if (height <= thresholds[l])
                return l;
        }

        return l;
       
    }
    float GetMaxHeight()
    {
        TerrainData data = terrain.terrainData;
        int width = data.heightmapResolution;
        int height = data.heightmapResolution;

        float maxHeight = float.MinValue;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float heightValue = data.GetHeight(y, x);
                if (heightValue > maxHeight)
                    maxHeight = heightValue;
            }
        }

        return maxHeight;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
