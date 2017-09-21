using UnityEngine;
using Debug = UnityEngine.Debug;

public class Game : MonoBehaviour {

	public static Register register;
	public static bool hasStarted;

	// Use this for initialization
	private void Start () {
		register = new Register();

		// Add mods

		PreInit();
		Init();
		PostInit();
		Debug.Log("Started");
		Debug.Log("Time Elapsed: " + Time.realtimeSinceStartup + " seconds");
		hasStarted = true;
	}
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

	/// <summary>
	/// PreInit is to setup any sort of files/directories or really anything that the mod developer sees fit.
	/// </summary>
	private void PreInit() {
		// Base PreInit
		RegisterBlocks();
		RegisterItems();
		UpdateBlocks();

		foreach (IMod m in register.GetMods()) {
			m.PreInit();
		}
	}

	/// <summary>
	/// Init is to initialize blocks, setup mods, etc.
	/// </summary>
	private void Init() {
		// Base Init

		foreach (IMod m in register.GetMods()) {
			m.Init();
		}
	}

	/// <summary>
	/// PostInit is to clean anything up and to test _all_ functionality to prevent errors in game.
	/// </summary>
	private void PostInit() {
		// Base PostInit

		foreach (IMod m in register.GetMods()) {
			m.PostInit();
		}
	}

	private void RegisterBlocks() {
		Block grassBlock = new Block(false) {id = 0};
		register.AddBlock(grassBlock, 0);

		Block dirtBlock = new Block(false) {id = 1};
		register.AddBlock(dirtBlock, 1);

		Block stoneBlock = new Block(false) {id = 2};
		register.AddBlock(stoneBlock, 2);

		Block woodenPlanksBlock = new Block(false) {id = 3};
		register.AddBlock(woodenPlanksBlock, 3);

		Block woodBlock = new Block(false) {id = 4};
		register.AddBlock(woodBlock, 4);

		Block coalOre = new Block(false) {id = 5};
		register.AddBlock(coalOre, 5);
	}
	
	private void RegisterItems()
	{
		Item grassBlock = new Item
		{
			type = ItemType.Block,
			block = register.GetBlock(0),
			id = 0
		};
		register.AddItem(grassBlock);

		Item dirtBlock = new Item
		{
			type = ItemType.Block,
			block = register.GetBlock(1),
			id = 1
		};
		register.AddItem(dirtBlock);

		Item stoneBlock = new Item
		{
			type = ItemType.Block,
			block = register.GetBlock(2),
			id = 2
		};
		register.AddItem(stoneBlock);

		Item woodenPlanksBlock = new Item
		{
			type = ItemType.Block,
			block = register.GetBlock(3),
			id = 3
		};
		register.AddItem(woodenPlanksBlock);

		Item woodBlock = new Item
		{
			type = ItemType.Block,
			block = register.GetBlock(4),
			id = 4
		};
		register.AddItem(woodBlock);

		Item coalOre = new Item
		{
			type = ItemType.Block,
			block = register.GetBlock(5),
			id = 5
		};
		register.AddItem(coalOre);
	}
	
	// Run after RegisterBlocks and RegisterItems
	private void UpdateBlocks()
	{
		Block[] blocks = register.GetBlocks();
		foreach (Block block in blocks)
		{
			block.item = register.GetItem(block.id);
			register.AddBlock(block, block.id);
		}
	}
}
