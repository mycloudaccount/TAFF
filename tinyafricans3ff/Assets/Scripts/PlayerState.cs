using System;
using UnityEngine;

[Serializable]
public class PlayerState
{

	public static PlayerState Current;

	public PlayerState () {

		SisterOne = CharacterOne.Instance;
		SisterTwo = CharacterTwo.Instance;
		SisterThree = CharacterThree.Instance;

		// generate a GUID for this instance of the Player
		PlayerGameId = Guid.NewGuid().ToString();

	}

	// saved game uid
	public string PlayerGameId;

	public string PlayerGameName = Constants.DEFAULT_GAME_NAME;

	public CharacterOne SisterOne;
	public CharacterTwo SisterTwo;
	public CharacterThree SisterThree;

	public string SelectedCharacterId;

	public Level SelectedLevel;

	public World SelectedWorld;

	public LevelList ListOfLevels = new LevelList ();

	[Serializable]
	public class LevelList {

		public Level LevelOne;
		public Level LevelTwo;
		public Level LevelThree;
		public Level LevelFour;
		public Level LevelFive;
		public Level LevelSix;
		public Level LevelSeven;
		public Level LevelEight;
		public Level LevelNine;
		public Level LevelTen;

	}

	public static class Constants
	{
		
		// all available characters (users can work with any or all of these player avatars)
		public const string SISTER_ONE_ID = CharacterOne.ID;
		public const string SISTER_TWO_ID = CharacterTwo.ID;
		public const string SISTER_THREE_ID = CharacterThree.ID;

		// default game name (user starts off with a default game saved automatically)
		public const string DEFAULT_GAME_NAME = "DEFAULT GAME";

	}

}

