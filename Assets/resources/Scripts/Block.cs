using System;

public enum BlockFace {
	All,
	Top, //Y+
	Bottom, //Y-
	Left, //X-
	Right, //X+
	Far, //Z+
	Near //Z-
}

public class Block
{
	public bool empty = false;

	public Block ReturnBlock {get{return this;}}

	public Block(bool isEmpty)
	{
		empty = isEmpty; 
	}
}
