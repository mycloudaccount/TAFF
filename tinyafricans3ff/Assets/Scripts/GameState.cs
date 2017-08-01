using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{

	private GameState () {

		GameSettings = Settings.Instance;

		MonsterOne = EnemyOne.Instance;

	}

	public static bool IsDevMode = true;

	private static GameState instance;
	public static GameState Instance {
		get { 
			if(instance == null) instance = new GameState();
			return instance;
		}
		set { instance = value; }
	}

	public string SelectedEnemyId;

	public Settings GameSettings;

	public EnemyOne MonsterOne;

	public int ScreenWidth;

	public int ScreenHeight;

	// making these dudes private allows me to ignore them
	// during serialization
	private string currentMenu;
	public string CurrentMenu {
		get { return currentMenu; }
		set { currentMenu = value; }
	}

	// making these dudes private allows me to ignore it
	// during serialization
	private string currentScene;
	public string CurrentScene {
		get { return currentScene; }
		set { currentScene = value; }
	}

	public string CurrentPlayer;

	// making these dudes private allows me to ignore it
	// during serialization
	private string fBUserAccessToken;
	public string FBUserAccessToken {
		get { return fBUserAccessToken; }
		set { fBUserAccessToken = value; }
	}

	public string PlayFabUserId;

	public static class Constants
	{

		// list of scenes
		public const string SCENE_CHARACTER_CUSTOMIZATION = "CharacterCustomization";
		public const string SCENE_CHARACTER_SELECTION = "CharacterSelection";
		public const string SCENE_LEVEL_MENU = "LevelMenu";
		public const string SCENE_MAIN_MENU = "MainMenu";
		public const string SCENE_START_MENU = "CharacterDevEnv";
		public const string SCENE_SPLASH = "Splash";

		// list of menus
		public const string CHARACTER_SELECTION_MENU = "CharacterSelectionMenu";
		public const string CHARACTER_CUSTOMIZATION_MENU = "CharacterCustomizationMenu";
		public const string GAME_SETTINGS_MENU = "GameSettingsMenu";
		public const string MAIN_MENU = "MainMenu";
		public const string START_MENU = "StartMenu";
		public const string LEVEL_MENU_MENU = "LevelMenu";
		public const string OPTIONS_MENU = "OptionsMenu";
		public const string HUD_MENU = "HUDMenu";
		public const string CHALLENGE_MENU = "ChallengeHUD";

		// default Game Settings
		public const float DEFAULT_EFFECTS_VOLUME = 70;
		public const float DEFAULT_MUSIC_VOLUME = 50;

		// list of available monsters
		public const string ENEMY_ONE_ID = EnemyOne.ID;

		// PlayFab Title ID
		public const string PLAYFAB_TITLE_ID = "9D59";

		// Layer names
		public const string LAYER_ON_STAGE = "OnStage";

		// Default number of coins
		public const int DEFAULT_COIN_COUNT = 900;

	}

}

