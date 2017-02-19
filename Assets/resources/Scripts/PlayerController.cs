using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateVision();
	}

    void UpdateVision() {
        Vector3 currentPos = this.transform.rotation.eulerAngles;
        Vector3 newPos = currentPos;

        newPos.x += -Input.GetAxis("Mouse X");
        newPos.y += Input.GetAxis("Mouse Y");

        Debug.Log("Current Pos: " + currentPos.x);
        Debug.Log("New Pos: " + newPos.x);


        // HACK: This is horrible... 80 is also a tottally random number
        if ((newPos.x > 30 && newPos.x < 80)  || newPos.x < -180)
        {
            newPos.x = currentPos.x;
        }

        Debug.Log("Changed Pos: " + newPos.x);

        this.transform.rotation = Quaternion.Euler(newPos);
    }
}