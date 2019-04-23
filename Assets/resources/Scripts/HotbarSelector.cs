using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelector : MonoBehaviour {

	public PlayerController player;

	private int previousSelection = 0;
	
	private Vector3 currentPosition = new Vector3(-260, -30, 0); // Initally set to the starting position
	private float xPositionIncrement = 65;
	
	// Use this for initialization
	void Start () {
		//transform.localPosition = currentPosition;
		currentPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		int hotbarPosition = player.GetHotbarSelection()-1;

		if (hotbarPosition != previousSelection) {
			float deltaX = xPositionIncrement * (hotbarPosition - previousSelection);
			Debug.Log(deltaX);

			currentPosition.x = currentPosition.x + deltaX;

			transform.localPosition = currentPosition;
			
			previousSelection = hotbarPosition;
		}
	}
}
