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
	public RVector3 position;

	public bool empty = false;

	public byte id = 0;

	/*
	 * Mining Levels:
	 * 0: Hand
	 * 1: Wood
	 * 2: Stone
	 * 3: Iron
	 * 4: Diamond
	 */
	public byte miningLevel = 0;

	public Block ReturnBlock {get{return this;}}

	public Block(bool isEmpty)
	{
		empty = isEmpty; 
	}
}
