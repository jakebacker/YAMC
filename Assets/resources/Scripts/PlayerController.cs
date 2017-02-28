using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    int speed {get; set;}

    Rigidbody rb;
    BoxCollider boxColl;

    GameObject cameraObject;
    Camera cam;
    Vector3 cameraCenter;

    GameObject currentSelectedObject;
    GameObject lastSelectedObject;

	// Use this for initialization
	void Start () {
        speed = 1;

        rb = this.gameObject.GetComponent<Rigidbody>();

        boxColl = this.gameObject.GetComponent<BoxCollider>();

        cameraObject = this.transform.FindChild("Camera").gameObject;
        cam = cameraObject.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateVision();
        UpdatePosition();
        currentSelectedObject = GetBlockFromLookVector();

        // Find the Selector, Kill it, respawn a new selector at the right position
        if (currentSelectedObject != lastSelectedObject)
        {
            if (GameObject.FindGameObjectWithTag("Selector") != null)
            {
                GameObject.Destroy(GameObject.FindGameObjectWithTag("Selector"));
            }

            if (currentSelectedObject != null)
            {
                ((GameObject)Instantiate(Resources.Load("Prefabs/BoxSelector"))).transform.position = (currentSelectedObject.transform.position + new Vector3(0.0f,0.0f,0.5f));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            BreakBlock(currentSelectedObject);
            GameObject.Destroy(GameObject.FindGameObjectWithTag("Selector"));
        }

        lastSelectedObject = currentSelectedObject;
	}


    /// <summary>
    /// Updates the player's vision.
    /// </summary>
    void UpdateVision() {

        Vector3 currentCamPos = cameraObject.transform.rotation.eulerAngles;
        Vector3 currentPlayerPos = this.transform.rotation.eulerAngles;
        Vector3 newCamPos = currentCamPos;
        Vector3 newPlayerPos = currentPlayerPos;

        newCamPos.x += -Input.GetAxis("Mouse X");
        newPlayerPos.y += Input.GetAxis("Mouse Y");

        // HACK: This is horrible... 80 is also a totally random number
        // FIXME: This needs to allow looking down
        if ((newCamPos.x > 55 && newCamPos.x < 80)  || newCamPos.x < -180)
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

    bool hasJumped = false;

    /// <summary>
    /// Updates the player's position.
    /// </summary>
    void UpdatePosition() {

        // FIXME: Jumping in a certain way can cause fast upward movement/double jumping

        Vector3 closestBound;

        Collider[] colliders;

        if (Input.GetKey("w"))
        {
            //transform.position += (transform.forward/8)*speed;
            closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((transform.forward/8)*speed)); // Gets point on bounding box that is closest to the new position
            closestBound += ((transform.forward / 1000) * speed); // Add the moving Vector to the closest point on the bounding box

            colliders = Physics.OverlapBox(closestBound, new Vector3(0, 0, 0));

            if (CanMove(colliders))
            {
                transform.position = closestBound;
            }

        }

        if (Input.GetKey("s"))
        {
            //transform.position += ((-transform.forward)/15)*speed;

            closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((-transform.forward/18)*speed));
            closestBound += ((-transform.forward / 1000) * speed);

            colliders = Physics.OverlapBox(closestBound, new Vector3(0, 0, 0));

            if (CanMove(colliders))
            {
                transform.position = closestBound;
            }
        }

        if (Input.GetKey("a"))
        {
            //transform.position += ((-transform.right)/10)*speed;

            closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((-transform.right/10)*speed));
            closestBound += ((-transform.right / 1000) * speed);

            colliders = Physics.OverlapBox(closestBound, new Vector3(0, 0, 0));

            if (CanMove(colliders))
            {
                transform.position = closestBound;
            }
        }

        if (Input.GetKey("d"))
        {
            //transform.position += (transform.right/10)*speed;

            closestBound = boxColl.bounds.ClosestPoint(this.transform.position + ((transform.right/10)*speed));
            closestBound += ((transform.right / 1000) * speed);

            colliders = Physics.OverlapBox(closestBound, new Vector3(0, 0, 0));

            if (CanMove(colliders))
            {
                transform.position = closestBound;
            }
        }

        if (Input.GetKey("space") && !hasJumped)
        {
            hasJumped = true;
            rb.AddForce(transform.up*225);
        }

    }

    /// <summary>
    /// Determines whether this player instance can move based on the specified colliders.
    /// </summary>
    /// <returns><c>true</c> if this instance can move; otherwise, <c>false</c>.</returns>
    /// <param name="colliders">Colliders.</param>
    bool CanMove(Collider[] colliders) {
        
        bool canMove = true;

        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Block"))
            {
                canMove = false;
            }
        }

        return canMove;
    }

    /// <summary>
    /// Gets the block from camera's look vector.
    /// </summary>
    /// <returns>The block from look vector. NOTE: This may return null</returns>
    GameObject GetBlockFromLookVector() {
        cameraCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane));

        //Debug.Log(cameraCenter);

        RaycastHit rayHit;

        if (Physics.Raycast(cameraCenter, cameraObject.transform.forward, out rayHit))
        {
            if (rayHit.transform.gameObject.CompareTag("Block"))
            {
                if (Vector3.Distance(this.transform.position, rayHit.transform.position) < 5) // Anything >= 10 is too far to reach
                {
                    return rayHit.transform.gameObject;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Breaks a block.
    /// </summary>
    /// <param name="block">The block to break</param>
    void BreakBlock(GameObject block) {
        Destroy(block);
        // Play animation
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.CompareTag("Block"))
        {
            if (hasJumped)
            {
                hasJumped = false;
            }
        }
    }
}