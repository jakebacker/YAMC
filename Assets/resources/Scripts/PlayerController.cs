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

	const int RANGE = 5;

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

		Debug.Log(Mathf.FloorToInt(5.0f));
		Debug.Log(Mathf.FloorToInt(5.1f));
		Debug.Log(Mathf.FloorToInt(5.5f));
		Debug.Log(Mathf.FloorToInt(5.9f));
		Debug.Log(Mathf.FloorToInt(6.0f));
	}

	// Update is called once per frame
	void Update()
	{

		UpdateVision();
		UpdatePosition();

		SelectBlock();
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
		if ((newCamPos.x > 55 && newCamPos.x < 80) || newCamPos.x < -180)
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

	/// <summary>
	/// Gets the block from camera's look vector.
	/// </summary>
	/// <returns>The block from look vector. NOTE: This may return null</returns>
	Block GetBlockFromLookVector() // This needs some tuning
	{
		RaycastHit hit;
		Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, RANGE);

		if (hit.transform == null)
		{
			return null;
		}

		if (hit.transform.tag == "Block")
		{
			Chunk chunk = hit.transform.gameObject.GetComponent<Chunk>();


			Vector3 newPoint = hit.point;
			newPoint -= new Vector3(0, 0.1f, 0);

			GameObject collMark = GameObject.Find("CollMark");
			if (collMark != null)
			{
				collMark.transform.position = newPoint;
			}

			return chunk.GetBlock(newPoint);
		}

		return null;
	}

	/// <summary>
	/// Selects the block that the player is looking at
	/// </summary>
	/// <returns>The block.</returns>
	Block SelectBlock() {
		Block block = GetBlockFromLookVector();

		if (block != null)
		{
			GameObject selector = GameObject.Find("Selector");

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

			selector.transform.position = new Vector3(block.position.x + 0.5f, block.position.y + 0.5f, block.position.z + 0.5f);

			return block;
		}

		return null;
	}
}