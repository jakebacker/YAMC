using System;
using UnityEngine;

public class Util
{

	public static int maxInt = Int32.MaxValue;

	/// <summary>
	/// Gets the children of a block.
	/// </summary>
	/// <returns>The children</returns>
	/// <param name="block">The block</param>
	public static GameObject[] GetChildren(GameObject block)
	{
		int count = block.transform.childCount;

		if (count == 0)
		{
			Debug.LogError("No Children Found!");
			return null;
		}

		GameObject[] children = new GameObject[count];

		for (int i = 0; i < count; i++)
		{
			children[i] = block.transform.GetChild(i).gameObject;
		}

		return children;
	}
}
