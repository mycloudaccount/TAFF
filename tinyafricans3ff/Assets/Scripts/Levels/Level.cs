using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
	
	public int LevelId = -1;

	// how long the player lasted in this level
	public int Duration = 0;
	public int Score = 0;

	public int CopperCoinCount = 0;
	public int SilverCoinCount = 0;
	public int GoldCoinCount = 0;

	public int DiamondGemCount = 0;
	public int RubyGemCount = 0;
	public int SaphireGemCount = 0;

	// has this level actually been primed
	public bool Primed = false;

	// has this level been played
	public bool Played = false;

}
