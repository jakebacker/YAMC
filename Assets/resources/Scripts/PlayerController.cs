using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{

	int speed { get; set; }

	Rigidbody rb;
	BoxCollider boxColl;

	GameObject cameraObject;
	Camera cam;
	Vector3 cameraCenter;
	Vector3 currentVisionCollison = new Vector3(0, 0, 0);

	const float RANGE = 5.0f;

	// Use this for initialization
	void Start()
	{
		speed = 1;

		rb = this.gameObject.GetComponent<Rigidbody>();

		boxColl = this.gameObject.GetComponent<BoxCollider>();

		cameraObject = this.transform.Find("Camera").gameObject;
		cam = cameraObject.GetComponent<Camera>();
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{

		UpdateVision();
		UpdatePosition();
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

			if (!Physics.Raycast(this.transform.position, this.transform.forward, 1))
			{
				transform.position = closestBound;
			}

		}

		if (Input.GetKey("s"))
		{
			//transform.position += ((-transform.forward)/15)*speed;

			closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((-transform.forward / 18) * speed));
			closestBound += ((-transform.forward / 1000) * speed);

			if (!Physics.Raycast(this.transform.position, -this.transform.forward, 1))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("a"))
		{
			//transform.position += ((-transform.right)/10)*speed;

			closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((-transform.right / 10) * speed));
			closestBound += ((-transform.right / 1000) * speed);

			if (!Physics.Raycast(this.transform.position, -this.transform.right, 1))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("d"))
		{
			//transform.position += (transform.right/10)*speed;

			closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((transform.right / 10) * speed));
			closestBound += ((transform.right / 1000) * speed);

			if (!Physics.Raycast(this.transform.position, this.transform.right, 1))
			{
				transform.position = closestBound;
			}
		}

		if (Input.GetKey("space") && Physics.Raycast(this.transform.position, -transform.up, 1))
		{
			rb.AddForceAtPosition(transform.up * 270, -transform.up);
		}

	}

	/// <summary>
	/// Gets the block from camera's look vector.
	/// </summary>
	/// <returns>The block from look vector. NOTE: This may return null</returns>
	GameObject GetBlockFromLookVector()
	{
		cameraCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane));

		//Debug.Log(cameraCenter);

		RaycastHit rayHit;

		Ray raycast = new Ray(cameraCenter, cameraObject.transform.forward);
		if (Physics.Raycast(raycast, out rayHit, RANGE))
		{
			if (rayHit.transform.gameObject.CompareTag("Block"))
			{
				currentVisionCollison = rayHit.point;
				return rayHit.transform.gameObject;
			}
		}

		return null;
	}

	/// <summary>
	/// Gets the block face from look vector.
	/// </summary>
	/// <returns>The block face from look vector.</returns>
	GameObject GetBlockFaceFromLookVector()
	{
		GameObject block = GetBlockFromLookVector();

		if (block == null)
		{
			return null;
		}

		double bestDistance = Int32.MaxValue;
		double currentDistance;
		GameObject selectedChild = null;

		foreach (GameObject child in Util.GetChildren(block))
		{
			currentDistance = Vector3.Distance(child.transform.position, currentVisionCollison);
			if (currentDistance < bestDistance)
			{
				bestDistance = currentDistance;
				selectedChild = child;
			}
		}

		return selectedChild;
	}
}