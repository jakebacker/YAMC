using System;
using UnityEngine;
// ReSharper disable UnusedMethodReturnValue.Local

public class PlayerController : MonoBehaviour
{

	public float speed { get; private set; }

	private Rigidbody _rb;
	private BoxCollider _boxColl;

	private GameObject _cameraObject;
	private Camera _cam;

	public GameObject selectorPrefab;
	private GameObject _selector;

	private Item[] _hotbar;
	private int _currentSelection = 1; // Number between 1 and 9

	private const int RANGE = 5;

	// ReSharper disable once RedundantDefaultMemberInitializer
	private bool _isHotbarInit = false;

	// Use this for initialization
	private void Start()
	{
		speed = 0.75f;

		_rb = gameObject.GetComponent<Rigidbody>();

		_boxColl = gameObject.GetComponent<BoxCollider>();

		_cameraObject = transform.Find("Camera").gameObject;
		_cam = _cameraObject.GetComponent<Camera>();

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;

		// Initialize hotbar
		_hotbar = new Item[9];
		for (int i = 0; i < _hotbar.Length; i++)
		{
			_hotbar[i] = new Item();
		}

		if (Game.hasStarted)
		{
			_hotbar[0] = Game.register.GetItem(0);
			_hotbar[1] = Game.register.GetItem(1);
			_hotbar[2] = Game.register.GetItem(2);
			_hotbar[3] = Game.register.GetItem(3);
			_hotbar[4] = Game.register.GetItem(4);
			_hotbar[5] = Game.register.GetItem(5);
			_hotbar[6] = Game.register.GetItem(6);
			_hotbar[7] = Game.register.GetItem(7);
			_hotbar[8] = Game.register.GetItem(8);
			_isHotbarInit = true;
		}

		_selector = GameObject.Find("Selector");
	}

	// Update is called once per frame
	private void Update()
	{
		if (Game.hasStarted && !_isHotbarInit)
		{
			_hotbar[0] = Game.register.GetItem(0);
			_hotbar[1] = Game.register.GetItem(1);
			_hotbar[2] = Game.register.GetItem(2);
			_hotbar[3] = Game.register.GetItem(3);
			_hotbar[4] = Game.register.GetItem(4);
			_hotbar[5] = Game.register.GetItem(5);
			_hotbar[6] = Game.register.GetItem(6);
			_hotbar[7] = Game.register.GetItem(7);
			_hotbar[8] = Game.register.GetItem(8);
			_isHotbarInit = true;
		}

		UpdateVision();
		UpdatePosition();

		SelectBlock();

		if (Input.GetMouseButtonDown(0))
		{
			Block block = SelectBlock();

			if (block != null)
			{
				BreakBlock(block);
			}
		}
		else if (Input.GetMouseButtonDown(1))
		{
			Debug.Log(_currentSelection);
			if (_hotbar[_currentSelection - 1].type == ItemType.Block)
			{
				PlaceBlock(_hotbar[_currentSelection - 1].block);
			}
		}

		// Select block
		if (Input.mouseScrollDelta.y > 0) // Right
		{
			if (_currentSelection == 9)
			{
				_currentSelection = 1;
			}
			else
			{
				_currentSelection++;
			}
		}
		else if (Input.mouseScrollDelta.y < 0)
		{
			if (_currentSelection == 1)
			{
				_currentSelection = 9;
			}
			else
			{
				_currentSelection--;
			}
		}


	}

	/// <summary>
	/// Updates the player's vision.
	/// </summary>
	private void UpdateVision()
	{

		Vector3 currentCamPos = _cameraObject.transform.rotation.eulerAngles;
		Vector3 currentPlayerPos = transform.rotation.eulerAngles;
		Vector3 newCamPos = currentCamPos;
		Vector3 newPlayerPos = currentPlayerPos;

		newCamPos.x += -Input.GetAxis("Mouse X");
		newPlayerPos.y += Input.GetAxis("Mouse Y");

		// HACK: This is horrible... 80 is also a totally random number
		// FIXME: This needs to allow looking down
		if ((newCamPos.x > 70 && newCamPos.x < 90) || newCamPos.x < -180)
		{
			newCamPos.x = currentCamPos.x;
		}

		_cameraObject.transform.rotation = Quaternion.Euler(newCamPos);

		transform.rotation = Quaternion.Euler(newPlayerPos);

		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

		if (_cameraObject.transform.rotation.eulerAngles.x > 80 && _cameraObject.transform.rotation.eulerAngles.x < 100)
		{
			Debug.LogWarning("STUCK PREVENTION ACTIVE!!!");
			_cameraObject.transform.rotation = new Quaternion(0, 0, 0, 0);
		}

		// TODO: Add stuck prevention for going very fast upwards
	}

	/// <summary>
	/// Updates the player's position.
	/// </summary>
	private void UpdatePosition()
	{

		// FIXME: Jumping in a certain way can cause fast upward movement/double jumping

		Vector3 closestBound;

		if (Input.GetKey("w"))
		{
			//transform.position += (transform.forward/8)*speed;
			closestBound = _boxColl.bounds.ClosestPoint(transform.position + ((transform.forward / 8) * speed)); // Gets point on bounding box that is closest to the new position
			closestBound += ((transform.forward / 1000) * speed); // Add the moving Vector to the closest point on the bounding box

			if (!Physics.Raycast(transform.position, transform.forward, 0.5f))
			{
				transform.position = closestBound;
			}

		}

		if (Input.GetKey("s"))
		{
			//transform.position += ((-transform.forward)/15)*speed;

			closestBound = _boxColl.bounds.ClosestPoint(transform.position + ((-transform.forward / 18) * speed));
			closestBound += ((-transform.forward / 1000) * speed);

			if (!Physics.Raycast(transform.position, -transform.forward, 0.5f))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("a"))
		{
			//transform.position += ((-transform.right)/10)*speed;

			closestBound = _boxColl.bounds.ClosestPoint(transform.position + ((-transform.right / 10) * speed));
			closestBound += ((-transform.right / 1000) * speed);

			if (!Physics.Raycast(transform.position, -transform.right, 0.5f))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("d"))
		{
			//transform.position += (transform.right/10)*speed;

			closestBound = _boxColl.bounds.ClosestPoint(transform.position + transform.right / 10 * speed);
			closestBound += transform.right / 1000 * speed;

			if (!Physics.Raycast(transform.position, transform.right, 0.5f))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("space") && Physics.Raycast(transform.position, -transform.up, 0.1f))
		{
			_rb.AddForceAtPosition(transform.up * 500, -transform.up);
		}

	}

	private void BreakBlock(Block block) {
		block.Break();
		block.chunk.RemoveBlock(block);
	}

	private Block PlaceBlock(Block blockProto) {
		BlockFace side;
		Block block = GetBlockFromLookVector(out side);

		if (side == BlockFace.All || block == null)
		{
			return null;
		}

		RVector3 newPosition = new RVector3(block.position.ToVector3());

		switch(side) {
			case BlockFace.Bottom:
				newPosition.y -= 1;
				break;
			case BlockFace.Top:
				newPosition.y += 1;
				break;
			case BlockFace.Far:
				newPosition.z += 1;
				break;
			case BlockFace.Near:
				newPosition.z -= 1;
				break;
			case BlockFace.Left:
				newPosition.x -= 1;
				break;
			case BlockFace.Right:
				newPosition.x += 1;
				break;
			case BlockFace.All:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		Vector3 center = newPosition.ToVector3();
		center.x += 0.5f;
		center.y += 0.5f;
		center.z += 0.5f;

		return Physics.OverlapBox(center, new Vector3(0.4f, 0.4f, 0.4f)).Length == 0 ? 
			block.chunk.AddBlock(blockProto, newPosition) : null;
	}

	/// <summary>
	/// Gets the block from camera's look vector.
	/// </summary>
	/// <returns>The block from look vector</returns>
	private Block GetBlockFromLookVector(out BlockFace side)
	{
		RaycastHit hit;
		Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, RANGE);


		if (hit.transform == null)
		{
			side = BlockFace.All;
			return null;
		}

		if (hit.transform.CompareTag("Block"))
		{
			Chunk chunk = hit.transform.gameObject.GetComponent<Chunk>();

			Vector3 newPoint = hit.point;


			BlockFace face = GetBlockSide(hit);
			side = face;
			switch (face)
			{
				case BlockFace.Top:
					newPoint -= new Vector3(0, 0.1f, 0);
					break;
				case BlockFace.Bottom:
					newPoint += new Vector3(0, 0.1f, 0);
					break;
				case BlockFace.Right:
					newPoint -= new Vector3(0.1f, 0, 0);
					break;
				case BlockFace.Left:
					newPoint += new Vector3(0.1f, 0, 0);
					break;
				case BlockFace.Near:
					newPoint += new Vector3(0, 0, 0.1f);
					break;
				case BlockFace.Far:
					newPoint -= new Vector3(0, 0, 0.1f);
					break;
			}

			GameObject collMark = GameObject.Find("CollMark");
			if (collMark != null)
			{
				collMark.transform.position = newPoint;
			}

			return chunk.GetBlock(newPoint);
		}

		side = BlockFace.All;

		return null;
	}

	private Block GetBlockFromLookVector() {
		BlockFace temp;
		return GetBlockFromLookVector(out temp);
	}

	/// <summary>
	/// Selects the block that the player is looking at
	/// </summary>
	/// <returns>The block.</returns>
	private Block SelectBlock() {
		Block block = GetBlockFromLookVector();

		if (block != null)
		{

			if (_selector == null)
			{
				if (selectorPrefab == null)
				{
					Debug.LogError("Selector prefab does not exist!");
				}
				else
				{
					_selector = Instantiate(selectorPrefab);
					_selector.name = "Selector";
				}
			}

			RVector3 pos = block.position;
			pos += block.chunk.Position;
			pos.y = block.position.y;

			if (_selector == null) throw new NullReferenceException("Selector has not been initialized!");
			_selector.SetActive(true);
			_selector.transform.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f);

			return block;
		}

		if (_selector != null)
		{
			_selector.SetActive(false);
		}
		return null;
	}

	private BlockFace GetBlockSide(RaycastHit hit) {
		BlockFace face = BlockFace.All;

		Vector3 normal = hit.normal;

		normal = hit.transform.TransformDirection(normal);

		if (normal == hit.transform.up)
		{
			face = BlockFace.Top;
		}
		else if (normal == -hit.transform.up)
		{
			face = BlockFace.Bottom;
		}
		else if (normal == hit.transform.right)
		{
			face = BlockFace.Right;
		}
		else if (normal == -hit.transform.right)
		{
			face = BlockFace.Left;
		}
		else if (normal == hit.transform.forward)
		{
			face = BlockFace.Far;
		}
		else if (normal == -hit.transform.forward)
		{
			face = BlockFace.Near;
		}

		return face;
	}
}