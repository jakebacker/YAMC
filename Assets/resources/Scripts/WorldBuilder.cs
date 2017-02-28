using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GenerateChunk(new Vector2(-4, -4), 0);
        GenerateChunk(new Vector2(4, 4), 0);
        GenerateBlock(new Vector3(0,1,1), BlockType.GRASS_BLOCK);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Generates a block.
    /// </summary>
    /// <param name="location">Block Location.</param>
    /// <param name="blockType">Block type.</param>
    void GenerateBlock(Vector3 location, string blockType) {
        ((GameObject)Instantiate(Resources.Load("Prefabs/" + blockType))).transform.position = location;
    }

    /// <summary>
    /// Generates an 8 by 8 layer of blocks.
    /// </summary>
    /// <param name="startingPos">The most negative corner of the layer (in x and z)</param>
    /// <param name="yHeight">The y height of the layer</param>
    /// <param name="type">The type of block to make the layer with</param>
    void GenerateLayer(Vector2 startingPos, int yHeight, string type) {
        // For now, ignore that yHeight has to change the blocks
        for (int x=(int)startingPos.x; x<(int)startingPos.x+8; x++) {
            for (int z = (int)startingPos.y; z < (int)startingPos.y + 8; z++) {
                GenerateBlock(new Vector3(x, yHeight, z), type);
            }
        }
    }

    /// <summary>
    /// Generates a chunk from maxHeight down to -20.
    /// </summary>
    /// <param name="startingPos">Starting position.</param>
    /// <param name="maxHeight">Top height of the chunk</param>
    void GenerateChunk(Vector2 startingPos, int maxHeight) {
        // Generate from top down to prevent buggyness
        GenerateLayer(startingPos, maxHeight, BlockType.GRASS_BLOCK);
        for (int i = maxHeight-1; i > -20; i--) { // 20 blocks tall for now
            GenerateLayer(startingPos, i, BlockType.DIRT_BLOCK);
        }
    }
}
