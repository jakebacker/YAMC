using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Noise{

	public static float[,] Generate(int xSize, int ySize, int seed, float intensity) {
		float[,] noise = new float[xSize, ySize];

		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				float xNoise = (float)x / xSize * intensity;
				float yNoise = (float)y / ySize * intensity;

				noise[x, y] = Mathf.PerlinNoise(seed + xNoise, seed + yNoise) * intensity;
			}
		}
		return noise;
	}

}
