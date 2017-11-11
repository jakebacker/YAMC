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


	public static float Perlin3D(float x, float y, float z) {
		float AB = Mathf.PerlinNoise(x, y);
		float BC = Mathf.PerlinNoise(y, z);
		float AC = Mathf.PerlinNoise(x, z);
		
		float BA = Mathf.PerlinNoise(y, z);
		float CB = Mathf.PerlinNoise(z, y);
		float CA = Mathf.PerlinNoise(z, x);

		float ABC = AB + BC + AC + BA + CB + CA;

		return ABC / 6f;
	}
}

