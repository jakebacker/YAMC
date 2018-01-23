using UnityEngine;

public class GBlock : MonoBehaviour {
	public int id = 1;
	private int _oldId;
	public Vector2 textureBlockSize;

	private Renderer _renderer;
	private Mesh _blockMesh;

	private Vector2[] _uvs;

	private int _verticiesIndex;

	private Texture _textureAtlas;
	private Vector2 _atlasSize;

	private Rect _uvsFront = new Rect(0.0f, 1.0f, 0.125f, 0.125f);
	private Rect _uvsBack = new Rect(0.125f, 0.875f, 0.125f, 0.125f);
	private Rect _uvsLeft = new Rect(0.25f, 0.75f, 0.125f, 0.125f);
	private Rect _uvsRight = new Rect(0.375f, 0.625f, 0.125f, 0.125f);
	private Rect _uvsTop = new Rect(0.5f, 0.5f, 0.125f, 0.125f);
	private Rect _uvsBottom = new Rect(0.625f, 0.375f, 0.125f, 0.125f);

	// Use this for initialization
	void Start() {
		_renderer = GetComponent<Renderer>();
		_blockMesh = GetComponent<MeshFilter>().mesh;

		_textureAtlas = transform.GetComponent<MeshRenderer>().material.mainTexture;
		_atlasSize = new Vector2(_textureAtlas.width / textureBlockSize.x, _textureAtlas.height / textureBlockSize.y);
		_uvs = new Vector2[_blockMesh.uv.Length];
		_uvs = _blockMesh.uv;

		SetUVs();
		_oldId = id;
	}

	void Update() {

		if (id < 0 && _renderer.enabled) {
			_renderer.enabled = false;
		} else if (id != _oldId) {
			if (!_renderer.enabled) {
				_renderer.enabled = true;
			}

			SetUVs();
			_oldId = id;
		}
	}

	void SetUVs() {
		Vector2 textureInterval = new Vector2(1 / _atlasSize.x, 1 / _atlasSize.y);

		Vector2 textureId = new Vector2(textureInterval.x * (id % _atlasSize.x),
			textureInterval.y * Mathf.FloorToInt(id / _atlasSize.y));

		_uvsFront = new Rect(textureId.x, textureId.y, textureInterval.x, textureInterval.y);

		// Same textures on all sides for now
		_uvsBack = _uvsFront;
		_uvsLeft = _uvsFront;
		_uvsRight = _uvsFront;
		_uvsTop = _uvsFront;
		_uvsBottom = _uvsFront;

		// - set UV coordinates - All of the numbers in comments are the original/broken ones

		// BACK    2    3    0    1
		_uvs[0] = new Vector2(_uvsFront.x, _uvsFront.y);
		_uvs[1] = new Vector2(_uvsFront.x + _uvsFront.width, _uvsFront.y);
		_uvs[2] = new Vector2(_uvsFront.x, _uvsFront.y - _uvsFront.height);
		_uvs[3] = new Vector2(_uvsFront.x + _uvsFront.width, _uvsFront.y - _uvsFront.height);

		// FRONT    6    7   10   11
		_uvs[6] = new Vector2(_uvsBack.x, _uvsBack.y);
		_uvs[7] = new Vector2(_uvsBack.x + _uvsBack.width, _uvsBack.y);
		_uvs[10] = new Vector2(_uvsBack.x, _uvsBack.y - _uvsBack.height);
		_uvs[11] = new Vector2(_uvsBack.x + _uvsBack.width, _uvsBack.y - _uvsBack.height);

		// LEFT   19   17   16   18
		_uvs[16] = new Vector2(_uvsLeft.x, _uvsLeft.y);
		_uvs[19] = new Vector2(_uvsLeft.x + _uvsLeft.width, _uvsLeft.y);
		_uvs[17] = new Vector2(_uvsLeft.x, _uvsLeft.y - _uvsLeft.height);
		_uvs[18] = new Vector2(_uvsLeft.x + _uvsLeft.width, _uvsLeft.y - _uvsLeft.height);

		// RIGHT   23   21   20   22
		_uvs[20] = new Vector2(_uvsRight.x, _uvsRight.y);
		_uvs[23] = new Vector2(_uvsRight.x + _uvsRight.width, _uvsRight.y);
		_uvs[21] = new Vector2(_uvsRight.x, _uvsRight.y - _uvsRight.height);
		_uvs[22] = new Vector2(_uvsRight.x + _uvsRight.width, _uvsRight.y - _uvsRight.height);

		// TOP    4    5    8    9
		_uvs[4] = new Vector2(_uvsTop.x, _uvsTop.y);
		_uvs[5] = new Vector2(_uvsTop.x + _uvsTop.width, _uvsTop.y);
		_uvs[8] = new Vector2(_uvsTop.x, _uvsTop.y - _uvsTop.height);
		_uvs[9] = new Vector2(_uvsTop.x + _uvsTop.width, _uvsTop.y - _uvsTop.height);

		// BOTTOM   15   13   12   14 a
		_uvs[12] = new Vector2(_uvsBottom.x, _uvsBottom.y);
		_uvs[13] = new Vector2(_uvsBottom.x + _uvsBottom.width, _uvsBottom.y);
		_uvs[15] = new Vector2(_uvsBottom.x, _uvsBottom.y - _uvsBottom.height);
		_uvs[14] = new Vector2(_uvsBottom.x + _uvsBottom.width, _uvsBottom.y - _uvsBottom.height);

		// - Assign the mesh its new UVs -
		_blockMesh.uv = _uvs;
	}
}