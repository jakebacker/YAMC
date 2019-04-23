using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class World : MonoBehaviour
{
    public GameObject ChunkPrefab;
    private Chunk _prefabScript;

    private const float ChunkSpacing = 17.0f;

    private const int Seed = 1234;
    private const int Intensity = 4;

    // For testing only
    private const int NumX = 10;
    private const int NumZ = 10;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateChunks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateChunks()
    {
        _prefabScript = ChunkPrefab.GetComponent<Chunk>();
        _prefabScript.seed = Seed;
        _prefabScript.intensity = Intensity;

        for (int x = 0; x < NumX; x++)
        {
            for (int z = 0; z < NumZ; z++)
            {
                Vector3 chunkPosition = new Vector3(x * ChunkSpacing, 0, z * ChunkSpacing);

                GameObject newChunk = Instantiate(ChunkPrefab);
                newChunk.transform.localPosition = chunkPosition;
            }
        } 
    }
}
