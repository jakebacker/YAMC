using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{

	float speed { get; set; }

	Rigidbody rb;
	BoxCollider boxColl;

	GameObject cameraObject;
	Camera cam;

	public GameObject selectorPrefab;
	GameObject selector;

	Block[] hotbar;
	int currentSelection = 1; // Number between 1 and 9

	const int RANGE = 5;

	bool isHotbarInit = false;

	// Use this for initialization
	void Start()
	{
		speed = 0.75f;

		rb = this.gameObject.GetComponent<Rigidbody>();

		boxColl = this.gameObject.GetComponent<BoxCollider>();

		cameraObject = this.transform.Find("Camera").gameObject;
		cam = cameraObject.GetComponent<Camera>();

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;

		// Initialize hotbar
		hotbar = new Block[9];
		for (int i = 0; i < hotbar.Length; i++)
		{
			hotbar[i] = new Block(true);
		}

		if (Game.hasStarted)
		{
			hotbar[0] = (Block)Game.register.GetBlock(0);
			hotbar[1] = (Block)Game.register.GetBlock(1);
			hotbar[2] = (Block)Game.register.GetBlock(2);
			hotbar[3] = (Block)Game.register.GetBlock(3);
			hotbar[4] = (Block)Game.register.GetBlock(4);
			hotbar[5] = (Block)Game.register.GetBlock(5);
			hotbar[6] = (Block)Game.register.GetBlock(6);
			hotbar[7] = (Block)Game.register.GetBlock(7);
			hotbar[8] = (Block)Game.register.GetBlock(8);
			isHotbarInit = true;
		}

		selector = GameObject.Find("Selector");
	}

	// Update is called once per frame
	void Update()
	{
		if (Game.hasStarted && !isHotbarInit)
		{
			hotbar[0] = (Block)Game.register.GetBlock(0);
			hotbar[1] = (Block)Game.register.GetBlock(1);
			hotbar[2] = (Block)Game.register.GetBlock(2);
			hotbar[3] = (Block)Game.register.GetBlock(3);
			hotbar[4] = (Block)Game.register.GetBlock(4);
			hotbar[5] = (Block)Game.register.GetBlock(5);
			hotbar[6] = (Block)Game.register.GetBlock(6);
			hotbar[7] = (Block)Game.register.GetBlock(7);
			hotbar[8] = (Block)Game.register.GetBlock(8);
			isHotbarInit = true;
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
			Debug.Log(currentSelection);
			PlaceBlock(hotbar[currentSelection-1]);	
		}

		// Select block
		if (Input.mouseScrollDelta.y > 0) // Right
		{
			if (currentSelection == 9)
			{
				currentSelection = 1;
			}
			else
			{
				currentSelection++;
			}
		}
		else if (Input.mouseScrollDelta.y < 0)
		{
			if (currentSelection == 1)
			{
				currentSelection = 9;
			}
			else
			{
				currentSelection--;
			}
		}


	}

	/// <summary>
	/// Updates the player's vision.
	/// </summary>
	void UpdateVision()
	{

		Vector3 currentCamPos = cameraObject.transform.rotation.eulerAngles;
		Vector3 currentPlayerPos = this.transform.rotation.eulerAngles;
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

		cameraObject.transform.rotation = Quaternion.Euler(newCamPos);

		this.transform.rotation = Quaternion.Euler(newPlayerPos);

		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

		if (cameraObject.transform.rotation.eulerAngles.x > 80 && cameraObject.transform.rotation.eulerAngles.x < 100)
		{
			Debug.LogWarning("STUCK PREVENTION ACTIVE!!!");
			cameraObject.transform.rotation = new Quaternion(0, 0, 0, 0);
		}

		// TODO: Add stuck prevention for going very fast upwards
	}

	/// <summary>
	/// Updates the player's position.
	/// </summary>
	void UpdatePosition()
	{

		// FIXME: Jumping in a certain way can cause fast upward movement/double jumping

		Vector3 closestBound;

		if (Input.GetKey("w"))
		{
			//transform.position += (transform.forward/8)*speed;
			closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((transform.forward / 8) * speed)); // Gets point on bounding box that is closest to the new position
			closestBound += ((transform.forward / 1000) * speed); // Add the moving Vector to the closest point on the bounding box

			if (!Physics.Raycast(this.transform.position, this.transform.forward, 0.5f))
			{
				transform.position = closestBound;
			}

		}

		if (Input.GetKey("s"))
		{
			//transform.position += ((-transform.forward)/15)*speed;

			closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((-transform.forward / 18) * speed));
			closestBound += ((-transform.forward / 1000) * speed);

			if (!Physics.Raycast(this.transform.position, -this.transform.forward, 0.5f))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("a"))
		{
			//transform.position += ((-transform.right)/10)*speed;

			closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((-transform.right / 10) * speed));
			closestBound += ((-transform.right / 1000) * speed);

			if (!Physics.Raycast(this.transform.position, -this.transform.right, 0.5f))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("d"))
		{
			//transform.position += (transform.right/10)*speed;

			closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((transform.right / 10) * speed));
			closestBound += ((transform.right / 1000) * speed);

			if (!Physics.Raycast(this.transform.position, this.transform.right, 0.5f))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("space") && Physics.Raycast(this.transform.position, -transform.up, 0.1f))
		{
			rb.AddForceAtPosition(transform.up * 500, -transform.up);
		}

	}

	void BreakBlock(Block block) {
		block.Break();
		block.chunk.RemoveBlock(block);
	}

	Block PlaceBlock(Block blockProto) {
		BlockFace side = BlockFace.All;
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
		}

		Vector3 center = newPosition.ToVector3();
		center.x += 0.5f;
		center.y += 0.5f;
		center.z += 0.5f;

		if (Physics.OverlapBox(center, new Vector3(0.4f, 0.4f, 0.4f)).Length == 0)
		{
			return block.chunk.AddBlock(blockProto, newPosition); // This is going to change
		}

		return null;
	}

	/// <summary>
	/// Gets the block from camera's look vector.
	/// </summary>
	/// <returns>The block from look vector</returns>
	Block GetBlockFromLookVector(out BlockFace side)
	{
		RaycastHit hit;
		Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, RANGE);


		if (hit.transform == null)
		{
			side = BlockFace.All;
			return null;
		}

		if (hit.transform.tag == "Block")
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

	Block GetBlockFromLookVector() {
		BlockFace temp;
		return GetBlockFromLookVector(out temp);
	}

	/// <summary>
	/// Selects the block that the player is looking at
	/// </summary>
	/// <returns>The block.</returns>
	Block SelectBlock() {
		Block block = GetBlockFromLookVector();

		if (block != null)
		{

			if (selector == null)
			{
				if (selectorPrefab == null)
				{
					Debug.LogError("Selector prefab does not exist!");
				}
				else
				{
					selector = Instantiate(selectorPrefab);
					selector.name = "Selector";
				}
			}

			selector.SetActive(true);
			selector.transform.position = new Vector3(block.position.x + 0.5f, block.position.y + 0.5f, block.position.z + 0.5f);

			return block;
		}

		if (selector != null)
		{
			selector.SetActive(false);
		}
		return null;
	}
		
	BlockFace GetBlockSide(RaycastHit hit) {
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