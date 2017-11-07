using System.Collections.Generic;

public class Register {

	private readonly List<Block> _blocks = new List<Block>();
	private readonly List<Item> _items = new List<Item>();
	private readonly List<IMod> _mods = new List<IMod>();

	public void AddItem(Item item, byte id) {
		_items.Insert(id, item);
	}

	public void AddItem(Item item) {
		_items.Add(item);
	}

	public void AddBlock(Block block)
	{
		_blocks.Add(block);
	}

	public void AddBlock(Block block, byte id)
	{
		_blocks.Insert(id, block);
	}

	public Item GetItem(byte id) {

		if (id+1 <= _items.Count)
		{
			return _items[id];
		}

		Item item = new Item();
		return item;
	}

	public Block GetBlock(byte id) {
		if (id + 1 <= _blocks.Count)
		{
			return _blocks[id];
		}
		Block block = new Block(true) {id = id};
		return block;
	}

	public Block[] GetBlocks()
	{
		Block[] blocksArray = new Block[_blocks.Count];
		_blocks.CopyTo(blocksArray);
		return blocksArray;
	}

	public List<IMod> GetMods() {
		return _mods;
	}
}
