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

		chunkMesh.Clear();

		float blockSize = 1;

		for (int y = 0; y <= chunkSize.y; y++)
		{
			for (int x = 0; x <= chunkSize.x; x++)
			{
				for (int z = 0; z <= chunkSize.z; z++)
				{
					if (!chunkBlocks[x, y, z].empty)
					{
						if (CheckSides(new RVector3(x, y, z), BlockFace.Top))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x, y + blockSize, z));
							chunkVerticies.Add(new Vector3(x, y + blockSize, z + blockSize));
							chunkVerticies.Add(new Vector3(x + blockSize, y + blockSize, z + blockSize));
							chunkVerticies.Add(new Vector3(x + blockSize, y + blockSize, z));

							UpdateChunkUV();
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Bottom))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y,z+blockSize));

							UpdateChunkUV();
						}




						if(CheckSides(new RVector3(x,y,z),BlockFace.Right))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x+blockSize,y,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z+blockSize));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));

							UpdateChunkUV();
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Left))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x,y,z));

							UpdateChunkUV();
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Far))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z+blockSize));

							UpdateChunkUV();
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Near))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z));

							UpdateChunkUV();
						}

					}
				}
			}
		}
		FinalizeChunk();
	}

	// TODO: Clean up
	public bool CheckSides(RVector3 blockPosition, BlockFace blockFace) {
		int x, y, z;
		x = blockPosition.x;
		y = blockPosition.y;
		z = blockPosition.z;

		switch (blockFace)
		{
			case BlockFace.Top:
				if (y + 1 <= chunkSize.y)
				{
					if (!chunkBlocks[x, y + 1, z].empty)
					{
						return false;
					}
				}
				break;
			case BlockFace.Bottom:
				if (y - 1 >= 0)
				{
					if (!chunkBlocks[x, y - 1, z].empty)
					{
						return false;
					}
				}
				break;
			case BlockFace.Right:
				if (x + 1 <= chunkSize.x)
				{
					if (!chunkBlocks[x + 1, y, z].empty)
					{
						return false;
					}
				}
				break;
			case BlockFace.Left:
				if (x - 1 >= 0)
				{
					if (!chunkBlocks[x - 1, y, z].empty)
					{
						return false;
					}
				}
				break;
			case BlockFace.Far:
				if (z + 1 <= chunkSize.z)
				{
					if (!chunkBlocks[x, y, z + 1].empty)
					{
						return false;
					}
				}
				break;
			case BlockFace.Near:
				if (z - 1 >= 0)
				{
					if (!chunkBlocks[x, y, z - 1].empty)
					{
						return false;
					}
				}
				break;
		}
		return true;
	} 

	void UpdateChunkUV() {
		chunkTriangles.Add(VerticiesIndex);
		chunkTriangles.Add(VerticiesIndex + 1);
		chunkTriangles.Add(VerticiesIndex + 2);

		chunkTriangles.Add(VerticiesIndex + 2);
		chunkTriangles.Add(VerticiesIndex + 3);
		chunkTriangles.Add(VerticiesIndex);

		chunkUV.Add(new Vector2(0, 0));
		chunkUV.Add(new Vector2(0, 1));
		chunkUV.Add(new Vector2(1, 1));
		chunkUV.Add(new Vector2(1, 0));
	}

	void FinalizeChunk() {
		chunkMesh.vertices = chunkVerticies.ToArray();
		chunkMesh.triangles = chunkTriangles.ToArray();
		chunkMesh.uv = chunkUV.ToArray();
		chunkMesh.RecalculateNormals();
	}
}
