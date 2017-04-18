using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFace : MonoBehaviour
{

	public Direction direction;

	public const int RENDER_DISTANCE = 8;

	private MeshRenderer meshRenderer;

	#pragma warning disable
	void Start()
	{
		if (direction == null)
		{
			Debug.LogWarning("BlockFace Direction is null!!!");
			direction = Direction.UP;
		}

		meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
	}

	void Update()
	{
		if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= RENDER_DISTANCE)
		{
			meshRenderer.enabled = false;
		}
		else
		{
			meshRenderer.enabled = true;
		}
	}
	
}
