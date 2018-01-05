using System.Collections.Generic;
using UnityEngine;

public class GBlock : MonoBehaviour {
	public int id = 1;
	private int old_id;
	public Vector2 textureBlockSize;

	private Mesh _blockMesh;

	private Vector2[] uvs;

	private int _verticiesIndex;

	private Texture _textureAtlas;
	private Vector2 _atlasSize;

	private const int SIZE = 10;

	private Rect uvsFront = new Rect(0.0f, 1.0f, 0.125f, 0.125f);
	private Rect uvsBack = new Rect(0.125f, 0.875f, 0.125f, 0.125f);
	private Rect uvsLeft = new Rect(0.25f, 0.75f, 0.125f, 0.125f);
	private Rect uvsRight = new Rect(0.375f, 0.625f, 0.125f, 0.125f);
	private Rect uvsTop = new Rect(0.5f, 0.5f, 0.125f, 0.125f);
	private Rect uvsBottom = new Rect(0.625f, 0.375f, 0.125f, 0.125f);

	// Use this for initialization
	void Start() {
		_blockMesh = GetComponent<MeshFilter>().mesh;

		_textureAtlas = transform.GetComponent<MeshRenderer>().material.mainTexture;
		_atlasSize = new Vector2(_textureAtlas.width / textureBlockSize.x, _textureAtlas.height / textureBlockSize.y);
		uvs = new Vector2[_blockMesh.uv.Length];
		uvs = _blockMesh.uv;
		
		SetUVs();
		old_id = id;
	}

	void Update() {
		if (id != old_id) {
			SetUVs();
			old_id = id;
		}
	}

	void SetUVs() {
		Vector2 textureInterval = new Vector2(1 / _atlasSize.x, 1 / _atlasSize.y);

		Vector2 textureId = new Vector2(textureInterval.x * (id % _atlasSize.x), textureInterval.y * Mathf.FloorToInt(id / _atlasSize.y));
		
		uvsFront = new Rect(textureId.x, textureId.y, textureInterval.x, textureInterval.y);
		
		// Same textures on all sides for now
		uvsBack = uvsFront;
		uvsLeft = uvsFront;
		uvsRight = uvsFront;
		uvsTop = uvsFront;
		uvsBottom = uvsFront;
		
		// - set UV coordinates - All of the numbers in comments are the original/broken ones

		// BACK    2    3    0    1
		uvs[0] = new Vector2(uvsFront.x, uvsFront.y);
		uvs[1] = new Vector2(uvsFront.x + uvsFront.width, uvsFront.y);
		uvs[2] = new Vector2(uvsFront.x, uvsFront.y - uvsFront.height);
		uvs[3] = new Vector2(uvsFront.x + uvsFront.width, uvsFront.y - uvsFront.height);

		// FRONT    6    7   10   11
		uvs[6] = new Vector2(uvsBack.x, uvsBack.y);
		uvs[7] = new Vector2(uvsBack.x + uvsBack.width, uvsBack.y);
		uvs[10] = new Vector2(uvsBack.x, uvsBack.y - uvsBack.height);
		uvs[11] = new Vector2(uvsBack.x + uvsBack.width, uvsBack.y - uvsBack.height);

		// LEFT   19   17   16   18
		uvs[16] = new Vector2(uvsLeft.x, uvsLeft.y);
		uvs[19] = new Vector2(uvsLeft.x + uvsLeft.width, uvsLeft.y);
		uvs[17] = new Vector2(uvsLeft.x, uvsLeft.y - uvsLeft.height);
		uvs[18] = new Vector2(uvsLeft.x + uvsLeft.width, uvsLeft.y - uvsLeft.height);

		// RIGHT   23   21   20   22
		uvs[20] = new Vector2(uvsRight.x, uvsRight.y);
		uvs[23] = new Vector2(uvsRight.x + uvsRight.width, uvsRight.y);
		uvs[21] = new Vector2(uvsRight.x, uvsRight.y - uvsRight.height);
		uvs[22] = new Vector2(uvsRight.x + uvsRight.width, uvsRight.y - uvsRight.height);

		// TOP    4    5    8    9
		uvs[4] = new Vector2(uvsTop.x, uvsTop.y);
		uvs[5] = new Vector2(uvsTop.x + uvsTop.width, uvsTop.y);
		uvs[8] = new Vector2(uvsTop.x, uvsTop.y - uvsTop.height);
		uvs[9] = new Vector2(uvsTop.x + uvsTop.width, uvsTop.y - uvsTop.height);

		// BOTTOM   15   13   12   14 a
		uvs[12] = new Vector2(uvsBottom.x, uvsBottom.y);
		uvs[13] = new Vector2(uvsBottom.x + uvsBottom.width, uvsBottom.y);
		uvs[15] = new Vector2(uvsBottom.x, uvsBottom.y - uvsBottom.height);
		uvs[14] = new Vector2(uvsBottom.x + uvsBottom.width, uvsBottom.y - uvsBottom.height);

		// - Assign the mesh its new UVs -
		_blockMesh.uv = uvs;
	}
}