public abstract class Inventory {
	private int _width;
	private int _height;
	
	protected Item[,] items;

	protected Inventory(int width, int height) {
		_width = width;
		_height = height;

		items = new Item[_width, _height];
	}
}