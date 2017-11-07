using UnityEngine;

public struct Noise{

	public static float[,] Generate(RVector3 chunkPosition, int xSize, int ySize, int seed, float intensity, int minHeight=50) {
		float[,] noise = new float[xSize, ySize];

		for (int x = chunkPosition.x; x < xSize+chunkPosition.x; x++)
		{
			for (int y = chunkPosition.z; y < ySize+chunkPosition.z; y++)
			{
				float xNoise = (float)x / xSize * intensity;
				float yNoise = (float)y / ySize * intensity;

				noise[x-chunkPosition.x, y-chunkPosition.z] = Mathf.PerlinNoise(seed + xNoise, seed + yNoise) * intensity+minHeight;
			}
		}
		return noise;
	}

}
