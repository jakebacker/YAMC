using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFace : MonoBehaviour {

    public Direction direction;

    #pragma warning disable
	void Start () {
        if (direction == null)
        {
            Debug.LogWarning("BlockFace Direction is null!!!");
            direction = Direction.UP;
        }
	}
	
}
