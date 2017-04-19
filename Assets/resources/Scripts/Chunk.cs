using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Chunk : MonoBehaviour
{
	Mesh chunkMesh;

	RVector3 chunkPosition;
	public RVector3 Position{ get{ return chunkPosition;} set{ chunkPosition = value;}}

	public RVector3 chunkSize;
	public RVector3 Size{ get{ return chunkSize;} set{chunkSize = value;}}

	public Block[,,] chunkBlocks;
	public Block[,,] ReturnChunkBlocks{ get { return chunkBlocks; } }

	public Chunk ThisChunk {get{ return this;}}

	List<Vector3> chunkVerticies = new List<Vector3>();
	List<Vector2> chunkUV = new List<Vector2>();
	List<int> chunkTriangles = new List<int>();
	int VerticiesIndex;

	void Awake () {
		chunkMesh = this.GetComponent<MeshFilter>().mesh;
		GenerateChunk();
	}

	public void GenerateChunk() {
		chunkBlocks = new Block[chunkSize.x + 1, chunkSize.y + 1, chunkSize.z + 1];

		for (int x = 0; x <= chunkSize.x; x++)
		{
			for (int z = 0; z <= chunkSize.z; z++)
			{
				for (int y = 0; y <= chunkSize.y; y++)
				{
					chunkBlocks[x, y, z] = new Block(false);
				}
			}
		}

		UpdateChunk();
	}

	public void UpdateChunk() {
		chunkVerticies = new List<Vector3>();
		chunkUV = new List<Vector2>();
		chunkTriangles = new List<int>();
	}
}
