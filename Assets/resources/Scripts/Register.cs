using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register {

	private List<Item> items = new List<Item>(); 
	private List<Mod> mods = new List<Mod>();

	public void AddItem(Item item, byte id) {
		items.Insert(id, item);
	}

	public void AddItem(Item item) {
		items.Add(item);
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
		if (id + 1 <= items.Count)
		{
			if (items[id] is Block)
			{
				return (Block)items[id];
			}
		}
		Block block = new Block(true);
		block.id = id;
		return block;
	}

	public List<Mod> GetMods() {
		return mods;
	}
}
