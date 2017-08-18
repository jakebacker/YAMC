using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public static Register register;
	public static bool hasStarted = false;

	// Use this for initialization
	void Start () {
		register = new Register();

		// Add mods

		PreInit();
		Init();
		PostInit();
		Debug.Log("Started");
		hasStarted = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// PreInit is to setup any sort of files/directories or really anything that the mod developer sees fit.
	/// </summary>
	void PreInit() {
		// Base PreInit
		RegisterBlocks();

		foreach (Mod m in register.GetMods()) {
			m.PreInit();
		}
	}

	/// <summary>
	/// Init is to initialize blocks, setup mods, etc.
	/// </summary>
	void Init() {
		// Base Init

		foreach (Mod m in register.GetMods()) {
			m.Init();
		}
	}

	/// <summary>
	/// PostInit is to clean anything up and to test _all_ functionality to prevent errors in game.
	/// </summary>
	void PostInit() {
		// Base PostInit

		foreach (Mod m in register.GetMods()) {
			m.PostInit();
		}
	}

	private void RegisterBlocks() {
		Block grassBlock = new Block(false);
		grassBlock.id = 0;
		register.AddItem(grassBlock, 0);

		Block dirtBlock = new Block(false);
		dirtBlock.id = 1;
		register.AddItem(dirtBlock, 1);

		Block stoneBlock = new Block(false);
		stoneBlock.id = 2;
		register.AddItem(stoneBlock, 2);

		Block woodenPlanksBlock = new Block(false);
		woodenPlanksBlock.id = 3;
		register.AddItem(woodenPlanksBlock, 3);

		Block woodBlock = new Block(false);
		woodBlock.id = 4;
		register.AddItem(woodBlock, 4);

		Block coalOre = new Block(false);
		coalOre.id = 5;
		register.AddItem(coalOre, 5);
	}
}
