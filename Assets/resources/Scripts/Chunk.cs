using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Chunk : MonoBehaviour
{
	private Mesh _chunkMesh;
	private MeshCollider _chunkCollider;

#pragma warning disable 649
	private Bounds _bounds;
#pragma warning restore 649

	private RVector3 _chunkPosition;
	public RVector3 Position{ get{ return _chunkPosition;} set{ _chunkPosition = value;}}

	public RVector3 chunkSize;
	public RVector3 Size{ get{ return chunkSize;} set{chunkSize = value;}}

	public Block[,,] chunkBlocks;
	public Block[,,] ReturnChunkBlocks{ get { return chunkBlocks; } }

	public Chunk ThisChunk {get{ return this;}}

	public int seed;
	public int intensity;

	private List<Vector3> _chunkVerticies = new List<Vector3>();
	private List<Vector2> _chunkUv = new List<Vector2>();
	private List<int> _chunkTriangles = new List<int>();
	private int _verticiesIndex;

	public Vector2 textureBlockSize;
	private Texture _textureAtlas;
	private Vector2 _atlasSize;

	private bool _hasGenerated;

	private void Start ()
	{

		_chunkPosition = new RVector3(transform.position);
		_textureAtlas = transform.GetComponent<MeshRenderer>().material.mainTexture;
		_atlasSize = new Vector2(_textureAtlas.width / textureBlockSize.x, _textureAtlas.height / textureBlockSize.y);

		_chunkMesh = GetComponent<MeshFilter>().mesh;
		_chunkMesh.MarkDynamic();

		if (seed < 0)
		{
			seed = Random.Range(0, (int)Mathf.Round(Util.maxInt/500.0f));
		}

		if (Game.hasStarted)
		{
			GenerateChunk();
			_chunkCollider = GetComponent<MeshCollider>();

			_bounds.SetMinMax(transform.position, transform.position + chunkSize);
		}
	}

	private void Update() {
		if (Game.hasStarted && !_hasGenerated)
		{
			GenerateChunk();
			_chunkCollider = GetComponent<MeshCollider>();

			_bounds.SetMinMax(transform.position, transform.position + chunkSize);
		}
	}

	public void GenerateChunk() {

		float[,] chunkHeights = Noise.Generate(new RVector3(transform.position), chunkSize.x + 1, chunkSize.y + 1, seed, intensity);

		chunkBlocks = new Block[chunkSize.x + 1, chunkSize.y + 1, chunkSize.z + 1];

		for (int x = 0; x <= chunkSize.x; x++)
		{
			for (int z = 0; z <= chunkSize.z; z++)
			{
				for (int y = 0; y <= chunkSize.y; y++)
				{
					chunkBlocks[x, y, z] = new Block(true) {position = new RVector3(x, y, z)};

					if (!(y <= chunkHeights[x, z])) continue;
					chunkBlocks[x, y, z] = new Block(false);

					if (y == (int)Mathf.Floor(chunkHeights[x, z]))
					{
						chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(0)); // Grass
					}
					else if (y >= chunkHeights[x, z] - 5)
					{
						chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(1)); // Dirt
					}
					else
					{ // This block conatains any standard ore generation
						
						//int tempSeed = (int)Mathf.Floor(seed/2*3);

						float xNoise = (x + _chunkPosition.x) / 3.0f * 0.15f;
						float yNoise = (y + _chunkPosition.y) / 3.0f * 0.15f;
						float zNoise = (z + _chunkPosition.z) / 3.0f * 0.15f;
						
						float noise = Noise.Perlin3D(seed + xNoise, seed + yNoise, seed + zNoise);
						
						if (noise < 0.65)
						{
							chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(2)) {miningLevel = 1}; // Stone
						}
						else
						{
							chunkBlocks[x, y, z] = new Block(Game.register.GetBlock(1)); // Dirt
						}

					}
						
					chunkBlocks[x, y, z].position = new RVector3(x, y, z);
					chunkBlocks[x, y, z].chunk = this;
				}
			}
		}

		_hasGenerated = true;
		UpdateChunk();
	}

	public void UpdateChunk() {
		_chunkVerticies = new List<Vector3>();
		_chunkUv = new List<Vector2>();
		_chunkTriangles = new List<int>();

		_chunkMesh.Clear();

		float blockSize = 1;

		for (int y = 0; y <= chunkSize.y; y++)
		{
			for (int x = 0; x <= chunkSize.x; x++)
			{
				for (int z = 0; z <= chunkSize.z; z++)
				{
					if (chunkBlocks[x, y, z].empty) continue;
					if (CheckSides(new RVector3(x, y, z), BlockFace.Top))
					{
						_verticiesIndex = _chunkVerticies.Count;

						_chunkVerticies.Add(new Vector3(x, y + blockSize, z));
						_chunkVerticies.Add(new Vector3(x, y + blockSize, z + blockSize));
						_chunkVerticies.Add(new Vector3(x + blockSize, y + blockSize, z + blockSize));
						_chunkVerticies.Add(new Vector3(x + blockSize, y + blockSize, z));

						UpdateChunkUv(chunkBlocks[x,y,z].id);
					}

					if(CheckSides(new RVector3(x,y,z),BlockFace.Bottom))
					{
						_verticiesIndex = _chunkVerticies.Count;

						_chunkVerticies.Add(new Vector3(x,y,z));
						_chunkVerticies.Add(new Vector3(x+blockSize,y,z));
						_chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));
						_chunkVerticies.Add(new Vector3(x,y,z+blockSize));

						UpdateChunkUv(chunkBlocks[x,y,z].id);
					}

					if(CheckSides(new RVector3(x,y,z),BlockFace.Right))
					{
						_verticiesIndex = _chunkVerticies.Count;

						_chunkVerticies.Add(new Vector3(x+blockSize,y,z));
						_chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z));
						_chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z+blockSize));
						_chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));

						UpdateChunkUv(chunkBlocks[x,y,z].id);
					}

					if(CheckSides(new RVector3(x,y,z),BlockFace.Left))
					{
						_verticiesIndex = _chunkVerticies.Count;

						_chunkVerticies.Add(new Vector3(x,y,z+blockSize));
						_chunkVerticies.Add(new Vector3(x,y+blockSize,z+blockSize));
						_chunkVerticies.Add(new Vector3(x,y+blockSize,z));
						_chunkVerticies.Add(new Vector3(x,y,z));

						UpdateChunkUv(chunkBlocks[x,y,z].id);
					}

					if(CheckSides(new RVector3(x,y,z),BlockFace.Far))
					{
						_verticiesIndex = _chunkVerticies.Count;

						_chunkVerticies.Add(new Vector3(x,y,z+blockSize));
						_chunkVerticies.Add(new Vector3(x+blockSize,y,z+blockSize));
						_chunkVerticies.Add(new Vector3(x+blockSize,y+blockSize,z+blockSize));
						_chunkVerticies.Add(new Vector3(x,y+blockSize,z+blockSize));

						UpdateChunkUv(chunkBlocks[x,y,z].id);
					}

					if (!CheckSides(new RVector3(x, y, z), BlockFace.Near)) continue;
					_verticiesIndex = _chunkVerticies.Count;

					_chunkVerticies.Add(new Vector3(x, y, z));
					_chunkVerticies.Add(new Vector3(x, y + blockSize, z));
					_chunkVerticies.Add(new Vector3(x + blockSize, y + blockSize, z));
					_chunkVerticies.Add(new Vector3(x + blockSize, y, z));

					UpdateChunkUv(chunkBlocks[x, y, z].id);
				}
			}
		}
		FinalizeChunk();
	}

	// TODO: Clean up
	public bool CheckSides(RVector3 blockPosition, BlockFace blockFace) {
		int x = blockPosition.x;
		int y = blockPosition.y;
		int z = blockPosition.z;

		switch (blockFace)
		{
			case BlockFace.Top:
				if (y + 1 <= chunkSize.y)
				{
					if (!chunkBlocks[x, y + 1, z].empty && !chunkBlocks[x, y + 1, z].hasTransparency)
					{
						return false;
					}
				}
				break;
			case BlockFace.Bottom:
				if (y - 1 >= 0)
				{
					if (!chunkBlocks[x, y - 1, z].empty && !chunkBlocks[x, y - 1, z].hasTransparency)
					{
						return false;
					}
				}
				break;
			case BlockFace.Right:
				if (x + 1 <= chunkSize.x)
				{
					if (!chunkBlocks[x + 1, y, z].empty && !chunkBlocks[x + 1, y, z].hasTransparency)
					{
						return false;
					}
				}
				break;
			case BlockFace.Left:
				if (x - 1 >= 0)
				{
					if (!chunkBlocks[x - 1, y, z].empty && !chunkBlocks[x - 1, y, z].hasTransparency)
					{
						return false;
					}
				}
				break;
			case BlockFace.Far:
				if (z + 1 <= chunkSize.z)
				{
					if (!chunkBlocks[x, y, z + 1].empty && !chunkBlocks[x, y, z + 1].hasTransparency)
					{
						return false;
					}
				}
				break;
			case BlockFace.Near:
				if (z - 1 >= 0)
				{
					if (!chunkBlocks[x, y, z - 1].empty && !chunkBlocks[x, y, z - 1].hasTransparency)
					{
						return false;
					}
				}
				break;
			case BlockFace.All:
				break;
			default:
				throw new ArgumentOutOfRangeException("blockFace", blockFace, null);
		}
		return true;
	}

	private void UpdateChunkUv(byte blockId) {
		_chunkTriangles.Add(_verticiesIndex);
		_chunkTriangles.Add(_verticiesIndex + 1);
		_chunkTriangles.Add(_verticiesIndex + 2);

		_chunkTriangles.Add(_verticiesIndex + 2);
		_chunkTriangles.Add(_verticiesIndex + 3);
		_chunkTriangles.Add(_verticiesIndex);

		Vector2 textureInterval = new Vector2(1 / _atlasSize.x, 1 / _atlasSize.y);

		Vector2 textureId = new Vector2(textureInterval.x * (blockId % _atlasSize.x), textureInterval.y * Mathf.FloorToInt(blockId / _atlasSize.y));

		
		_chunkUv.Add(new Vector2(textureId.x,textureId.y-textureInterval.y));
		_chunkUv.Add(new Vector2(textureId.x+textureInterval.x,textureId.y-textureInterval.y));
		_chunkUv.Add(new Vector2(textureId.x+textureInterval.x,textureId.y));
		_chunkUv.Add(new Vector2(textureId.x,textureId.y));
	}

	private void UpdateCollider() {
		_chunkCollider.enabled = false;
		_chunkCollider.enabled = true;
	}

	private void FinalizeChunk() {
		_chunkMesh.vertices = _chunkVerticies.ToArray();
		_chunkMesh.triangles = _chunkTriangles.ToArray();
		_chunkMesh.uv = _chunkUv.ToArray();
		_chunkMesh.RecalculateNormals();

		if (gameObject.GetComponent<MeshCollider>() == null)
		{
			gameObject.AddComponent<MeshCollider>();
		}
	}

	/// <summary>
	/// Gets the block closest to the position provided
	/// </summary>
	/// <returns>The closest block</returns>
	/// <param name="position">The position closest to a block</param>
	public Block GetBlock(Vector3 position) {
		RVector3 rpos = new RVector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

		rpos.x -= _chunkPosition.x;
		rpos.z -= _chunkPosition.z;
		
		return chunkBlocks[rpos.x, rpos.y, rpos.z];
	}

	public void RemoveBlock(Block block) {
		RVector3 pos = block.position;

		chunkBlocks[pos.x, pos.y, pos.z].empty = true;
		UpdateChunk();
		UpdateCollider();
	}

	public Block AddBlock(Block blockProto, RVector3 position) {
		if (_bounds.Contains(position.ToVector3()))
		{
			Block block = new Block(blockProto)
			{
				position = position,
				chunk = this
			};
			chunkBlocks[position.x, position.y, position.z] = block;

			UpdateChunk();
			UpdateCollider();

			return block;
		}
		Debug.Log("Block not in chunk!");
		return null;
	}
}
