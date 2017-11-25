using UnityEngine;

public abstract class Inventory {
	private Vector2 size;
	
	protected Item[,] items;

	protected Inventory(int width, int height) {
		size.x = width;
		size.y = height;

		items = new Item[(int)size.x,(int)size.y];
	}
}