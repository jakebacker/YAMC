public enum BlockFace {
	All,
	Top, //Y+
	Bottom, //Y-
	Left, //X-
	Right, //X+
	Far, //Z+
	Near //Z-
}

public class Block {
	public RVector3 position;

	public Chunk chunk;

	public bool empty;

	public byte id;

	public Item item;

	/*
	 * Mining Levels:
	 * 0: Hand
	 * 1: Wood
	 * 2: Stone
	 * 3: Iron
	 * 4: Diamond
	 */
	public byte miningLevel;

	public bool hasTransparency = false;

	public Block ReturnBlock {
		get { return this; }
	}

	public Block(bool isEmpty) {
		empty = isEmpty;
	}

	public Block(Block block) {
		id = block.id;
		miningLevel = block.miningLevel;
		item = block.item;
		hasTransparency = block.hasTransparency;
	}

	public virtual void Break() {
	}
}