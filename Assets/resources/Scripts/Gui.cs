using UnityEngine;
using UnityEngine.UI;

public abstract class Gui {
	protected Image background;

	protected Vector2 size;

	protected Gui(int width, int height) {
		size.x = width;
		size.y = height;
	}

	protected void setBackground(Image img) {
		background = img;
	}
}
