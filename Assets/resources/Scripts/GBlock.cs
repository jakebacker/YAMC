using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBlock : MonoBehaviour {

	public int id = 1;
	public Vector2 textureBlockSize;

	private Mesh _blockMesh;
	
	private List<Vector3> _blockVerticies = new List<Vector3>();
	private List<Vector2> _blockUv = new List<Vector2>();
	private List<int> _blockTriangles = new List<int>();
	
	private int _verticiesIndex;

	private Texture _textureAtlas;
	private Vector2 _atlasSize;

	private const int SIZE = 10;

	// Use this for initialization
	void Start () {
		_blockMesh = GetComponent<MeshFilter>().mesh;
		
		_textureAtlas = transform.GetComponent<MeshRenderer>().material.mainTexture;
		_atlasSize = new Vector2(_textureAtlas.width / textureBlockSize.x, _textureAtlas.height / textureBlockSize.y);
		
		_verticiesIndex = _blockVerticies.Count;

		_blockVerticies.Add(new Vector3(transform.position.x - SIZE, transform.position.y, transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y,transform.position.z - SIZE));

		UpdateChunkUv((byte)id);

		_verticiesIndex = _blockVerticies.Count;

		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y - SIZE,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y - SIZE,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z));

		UpdateChunkUv((byte)id);
		
		_verticiesIndex = _blockVerticies.Count;

		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y - SIZE,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y - SIZE,transform.position.z));

		UpdateChunkUv((byte)id);

		_verticiesIndex = _blockVerticies.Count;

		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z - SIZE));

		UpdateChunkUv((byte)id);

		_verticiesIndex = _blockVerticies.Count;

		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y,transform.position.z));

		UpdateChunkUv((byte)id);

		_verticiesIndex = _blockVerticies.Count;

		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y - SIZE,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x - SIZE,transform.position.y,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y,transform.position.z - SIZE));
		_blockVerticies.Add(new Vector3(transform.position.x,transform.position.y - SIZE,transform.position.z - SIZE));

		UpdateChunkUv((byte)id);
		
		_blockMesh.vertices = _blockVerticies.ToArray();
		_blockMesh.triangles = _blockTriangles.ToArray();
		_blockMesh.uv = _blockUv.ToArray();
		_blockMesh.RecalculateNormals();
	}
	
	private void UpdateChunkUv(byte blockId) {
		_blockTriangles.Add(_verticiesIndex);
		_blockTriangles.Add(_verticiesIndex + 1);
		_blockTriangles.Add(_verticiesIndex + 2);

		_blockTriangles.Add(_verticiesIndex + 2);
		_blockTriangles.Add(_verticiesIndex + 3);
		_blockTriangles.Add(_verticiesIndex);

		Vector2 textureInterval = new Vector2(1 / _atlasSize.x, 1 / _atlasSize.y);

		Vector2 textureId = new Vector2(textureInterval.x * (blockId % _atlasSize.x), textureInterval.y * Mathf.FloorToInt(blockId / _atlasSize.y));

		
		_blockUv.Add(new Vector2(textureId.x,textureId.y-textureInterval.y));
		_blockUv.Add(new Vector2(textureId.x+textureInterval.x,textureId.y-textureInterval.y));
		_blockUv.Add(new Vector2(textureId.x+textureInterval.x,textureId.y));
		_blockUv.Add(new Vector2(textureId.x,textureId.y));
	}
}