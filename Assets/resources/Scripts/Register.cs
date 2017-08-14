using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register {

	private List<Block> blocks = new List<Block>(); 
	private List<Mod> mods = new List<Mod>();

	public void AddBlock(Block block, byte id) {
		blocks.Insert(id, block);
	}

	public void AddBlock(Block block) {
		blocks.Add(block);
	}

	public Block GetBlock(byte id) {

		if (id+1 <= blocks.Capacity)
		{
			return blocks[id];
		}
		else
		{
			Block block = new Block(false);
			block.id = id;
			return block;
		}
	}

	public List<Mod> GetMods() {
		return mods;
	}
}
