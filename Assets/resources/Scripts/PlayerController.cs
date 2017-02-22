using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    int speed {get; set;}

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        speed = 1;
        rb = this.gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateVision();
        UpdatePosition();
	}

    void UpdateVision() {

        GameObject camera = this.transform.FindChild("Camera").gameObject;

        Vector3 currentCamPos = camera.transform.rotation.eulerAngles;
        Vector3 currentPlayerPos = this.transform.rotation.eulerAngles;
        Vector3 newCamPos = currentCamPos;
        Vector3 newPlayerPos = currentPlayerPos;

        newCamPos.x += -Input.GetAxis("Mouse X");
        newPlayerPos.y += Input.GetAxis("Mouse Y");

        // HACK: This is horrible... 80 is also a totally random number
        if ((newCamPos.x > 55 && newCamPos.x < 80)  || newCamPos.x < -180)
        {
            newCamPos.x = currentCamPos.x;
        }

        camera.transform.rotation = Quaternion.Euler(newCamPos);

        this.transform.rotation = Quaternion.Euler(newPlayerPos);

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        if (camera.transform.rotation.eulerAngles.x > 80 && camera.transform.rotation.eulerAngles.x < 100)
        {
            Debug.LogWarning("STUCK PREVENTION ACTIVE!!!");
            camera.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        // TODO: Add stuck prevention for going very fast upwards
    }

    bool hasJumped = false;
    void UpdatePosition() {

        // FIXME: Moving into a block causes the player to glitch into it

        if (Input.GetKey("w"))
        {
           transform.position += (transform.forward/10)*speed;
        }

        if (Input.GetKey("s"))
        {
            transform.position += ((-transform.forward)/15)*speed;
        }

        if (Input.GetKey("a"))
        {
            transform.position += ((-transform.right)/10)*speed;
        }

        if (Input.GetKey("d"))
        {
            transform.position += (transform.right/10)*speed;
        }

        if (Input.GetKey("space") && !hasJumped)
        {
            hasJumped = true;
            rb.AddForce(transform.up*225);
        }
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