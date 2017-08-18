using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Chunk : MonoBehaviour
{
	Mesh chunkMesh;
	MeshCollider chunkCollider;

	Bounds bounds;

	RVector3 chunkPosition;
	public RVector3 Position{ get{ return chunkPosition;} set{ chunkPosition = value;}}

	public RVector3 chunkSize;
	public RVector3 Size{ get{ return chunkSize;} set{chunkSize = value;}}

	public Block[,,] chunkBlocks;
	public Block[,,] ReturnChunkBlocks{ get { return chunkBlocks; } }

	public Chunk ThisChunk {get{ return this;}}

	public int seed;
	public int intensity;

	List<Vector3> chunkVerticies = new List<Vector3>();
	List<Vector2> chunkUV = new List<Vector2>();
	List<int> chunkTriangles = new List<int>();
	int VerticiesIndex;

	public Vector2 textureBlockSize;
	Texture textureAtlas;
	Vector2 atlasSize;

	bool hasGenerated = false;

	void Start () {
		textureAtlas = transform.GetComponent<MeshRenderer>().material.mainTexture;
		atlasSize = new Vector2(textureAtlas.width / textureBlockSize.x, textureAtlas.height / textureBlockSize.y);

		chunkMesh = this.GetComponent<MeshFilter>().mesh;
		chunkMesh.MarkDynamic();

		if (seed < 0)
		{
			seed = Random.Range(0, (int)Mathf.Round(Util.maxInt/500));
		}

		if (Game.hasStarted)
		{
			GenerateChunk();
			chunkCollider = this.GetComponent<MeshCollider>();

			bounds.SetMinMax(this.transform.position, this.transform.position + chunkSize);
		}
	}

	void Update() {
		if (Game.hasStarted && !hasGenerated)
		{
			GenerateChunk();
			chunkCollider = this.GetComponent<MeshCollider>();

			bounds.SetMinMax(this.transform.position, this.transform.position + chunkSize);
		}
	}

	public void GenerateChunk() {

		float[,] chunkHeights = Noise.Generate(chunkSize.x + 1, chunkSize.y + 1, seed, intensity);

		chunkBlocks = new Block[chunkSize.x + 1, chunkSize.y + 1, chunkSize.z + 1];

		for (int x = 0; x <= chunkSize.x; x++)
		{
			for (int z = 0; z <= chunkSize.z; z++)
			{
				for (int y = 0; y <= chunkSize.y; y++)
				{
					chunkBlocks[x, y, z] = new Block(true);
					chunkBlocks[x, y, z].position = new RVector3(x, y, z);

					if (y <= chunkHeights[x, z])
					{
						chunkBlocks[x, y, z] = new Block(false);

						if (y == Mathf.Floor(chunkHeights[x, z]))
						{
							chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(0)); // Grass
						}
						else if (y >= chunkHeights[x, z] - 5)
						{
							chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(1)); // Dirt
						}
						else
						{
							int tempSeed = (int)Mathf.Floor(seed/2*3);
							if (Mathf.PerlinNoise(tempSeed + x, tempSeed + y) < 0.7)
							{
								chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(2)); // Stone
								chunkBlocks[x, y, z].miningLevel = 1;
							}
							else
							{
								chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(1));
							}

						}

						chunkBlocks[x, y, z].position = new RVector3(x, y, z);
						chunkBlocks[x, y, z].chunk = this;

					}
				}
			}
		}

		hasGenerated = true;
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

							UpdateChunkUV(chunkBlocks[x,y,z].id);
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Bottom))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y,z+blockSize));

							UpdateChunkUV(chunkBlocks[x,y,z].id);
						}




						if(CheckSides(new RVector3(x,y,z),BlockFace.Right))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x+blockSize,y,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z+blockSize));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));

							UpdateChunkUV(chunkBlocks[x,y,z].id);
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Left))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x,y,z));

							UpdateChunkUV(chunkBlocks[x,y,z].id);
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Far))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z+blockSize));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z+blockSize));

							UpdateChunkUV(chunkBlocks[x,y,z].id);
						}

						if(CheckSides(new RVector3(x,y,z),BlockFace.Near))
						{
							VerticiesIndex = chunkVerticies.Count;

							chunkVerticies.Add(new Vector3(x,y,z));
							chunkVerticies.Add(new Vector3(x,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z));
							chunkVerticies.Add(new Vector3(x+blockSize,y,z));

							UpdateChunkUV(chunkBlocks[x,y,z].id);
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

	void UpdateChunkUV(byte blockID) {
		chunkTriangles.Add(VerticiesIndex);
		chunkTriangles.Add(VerticiesIndex + 1);
		chunkTriangles.Add(VerticiesIndex + 2);

		chunkTriangles.Add(VerticiesIndex + 2);
		chunkTriangles.Add(VerticiesIndex + 3);
		chunkTriangles.Add(VerticiesIndex);

		Vector2 textureInterval = new Vector2(1 / atlasSize.x, 1 / atlasSize.y);

		Vector2 textureID = new Vector2(textureInterval.x * (blockID % atlasSize.x), textureInterval.y * Mathf.FloorToInt(blockID / atlasSize.y));

		chunkUV.Add(new Vector2(textureID.x,textureID.y-textureInterval.y));
		chunkUV.Add(new Vector2(textureID.x+textureInterval.x,textureID.y-textureInterval.y));
		chunkUV.Add(new Vector2(textureID.x+textureInterval.x,textureID.y));
		chunkUV.Add(new Vector2(textureID.x,textureID.y));
	}

	void UpdateCollider() {
		chunkCollider.enabled = false;
		chunkCollider.enabled = true;
	}

	void FinalizeChunk() {
		chunkMesh.vertices = chunkVerticies.ToArray();
		chunkMesh.triangles = chunkTriangles.ToArray();
		chunkMesh.uv = chunkUV.ToArray();
		chunkMesh.RecalculateNormals();

		if (this.gameObject.GetComponent<MeshCollider>() == null)
		{
			this.gameObject.AddComponent<MeshCollider>();
		}
	}

	/// <summary>
	/// Gets the block closest to the position provided
	/// </summary>
	/// <returns>The closest block</returns>
	/// <param name="position">The position closest to a block</param>
	public Block GetBlock(Vector3 position) {
		RVector3 rpos = new RVector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

		return chunkBlocks[rpos.x, rpos.y, rpos.z];
	}

	public void RemoveBlock(Block block) {
		RVector3 pos = block.position;

		chunkBlocks[pos.x, pos.y, pos.z].empty = true;
		UpdateChunk();
		UpdateCollider();
	}

	public Block AddBlock(Block blockProto, RVector3 position) {
		if (bounds.Contains(position.ToVector3()))
		{
			Block block = new Block(blockProto);
			block.position = position;
			block.chunk = this;
			chunkBlocks[position.x, position.y, position.z] = block;

			UpdateChunk();
			UpdateCollider();

			return block;
		}
		Debug.Log("Block not in chunk!");
		return null;
	}
}
