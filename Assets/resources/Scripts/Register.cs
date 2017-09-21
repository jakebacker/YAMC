using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register {

	private List<Block> blocks = new List<Block>();
	private List<Item> items = new List<Item>();
	private List<Mod> mods = new List<Mod>();

	public void AddItem(Item item, byte id) {
		this.items.Insert(id, item);
	}

	public void AddItem(Item item) {
		this.items.Add(item);
	}

	public void AddBlock(Block block)
	{
		this.blocks.Add(block);
	}

	public void AddBlock(Block block, byte id)
	{
		this.blocks.Insert(id, block);
	}

	public Item GetItem(byte id) {

		if (id+1 <= items.Count)
		{
			return items[id];
		}

		Item item = new Item();
		return item;
	}

	public Block GetBlock(byte id) {
		if (id + 1 <= this.blocks.Count)
		{
			return (Block)this.blocks[id];
		}
		Block block = new Block(true);
		block.id = id;
		return block;
	}

	public Block[] GetBlocks()
	{
		Block[] blocksArray = new Block[blocks.Count];
		blocks.CopyTo(blocksArray);
		return blocksArray;
	}

	public List<Mod> GetMods() {
		return mods;
	}
}
