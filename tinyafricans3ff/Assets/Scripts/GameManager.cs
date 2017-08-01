using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using MarkLight;
using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;

public class GameManager : MonoBehaviour {

	public string ThisScene;
	public static GameManager Instance = null; 

	public static GameTitle GameTitleInScene;
	public static OptionsMenu OptionsMenuInScene;
	public static MainMenu MainMenuInScene;
	public static StartMenu StartMenuInScene;
	public static LevelMenu LevelMenuInScene;
	public static GameSettings GameSettingsInScene;
	public static SaveGameStateDialog SaveGameStateDialogInScene;
	public static LoginDialog LoginDialogInScene;
	public static CharacterCustomization CharacterCustomizationInScene;
	public static CharacterSelection CharacterSelectionInScene;
	public static HUDMenu HUDMenuInScene;

	// this is a record of where we are in the game.
	//
	//	1. when a scene is viewed for the first time it is added to the 
	//	the Dictionary along with default menu list containing one item
	//
	//	2. Each Scene is a key to a List that maintains where the user is regarding the
	//	scenes available menus
	private static Dictionary<object, ArrayList> MenuBreadCrumbsByScene = new Dictionary<object, ArrayList>();
	private static ArrayList SceneBreadCrumb = new ArrayList();
	public bool LoadingPlayerSettings = false;
	public bool LoadingGameSettings = false;
	public bool LoadingHeroData = false;
	public bool LoadingWorldData = false;

	void Awake() {
		
		//Check if instance already exists
		if (Instance == null) {
			//if not, set instance to this
			Instance = this;

			LoadingGameSettings = true;
			LoadingPlayerSettings = true;
			LoadingHeroData = true;
			LoadingWorldData = true;

			// only time game state will be null is if in slapsh scene in which case I will
			// assign main menu as next in most case.  in the case that it is not next,
			// I will let the Login dialog logic sort out what to do
			AddToSceneBreadCrumb (GameState.Constants.SCENE_START_MENU, GameState.Constants.START_MENU);
		}
		//If instance already exists and it's not this:
		else if (Instance != this) {
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);  		// allow this class to persist from scene-to-scene
		}
		DontDestroyOnLoad (gameObject);

		// gather all the game menus
		if (GameTitleInScene == null) {
			GameTitleInScene = Object.FindObjectOfType<GameTitle>();
		}
		if (OptionsMenuInScene == null) {
			OptionsMenuInScene = Object.FindObjectOfType<OptionsMenu>();
		}
		if (MainMenuInScene == null) {
			MainMenuInScene = Object.FindObjectOfType<MainMenu>();
		}
		if (StartMenuInScene == null) {
			StartMenuInScene = Object.FindObjectOfType<StartMenu>();
		}
		if (GameSettingsInScene == null) {
			GameSettingsInScene = Object.FindObjectOfType<GameSettings>();
		}
		if (CharacterCustomizationInScene == null) {
			CharacterCustomizationInScene = Object.FindObjectOfType<CharacterCustomization>();
		}
		if (SaveGameStateDialogInScene == null) {
			SaveGameStateDialogInScene = Object.FindObjectOfType<SaveGameStateDialog>();
		}
		if (LoginDialogInScene == null) {
			LoginDialogInScene = Object.FindObjectOfType<LoginDialog>();
		}
		if (CharacterSelectionInScene == null) {
			CharacterSelectionInScene = Object.FindObjectOfType<CharacterSelection>();
		}
		if (LevelMenuInScene == null) {
			LevelMenuInScene = Object.FindObjectOfType<LevelMenu>();
		}
		if (HUDMenuInScene == null) {
			HUDMenuInScene = Object.FindObjectOfType<HUDMenu>();
		}

	}

	void Start () {

		// setting up Dot Tween for rest of game
		DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

	}

	void Update () {


	}

	// list of saved games (a user can maintain multiple player states)
	private List<PlayerState> playerGames = new List<PlayerState>();
	public List<PlayerState> PlayerGames {
		get { return playerGames; }
		set { playerGames = value; }
	}
		
	public void SaveGameState() {

		// save game state
		string jsonData = JsonUtility.ToJson(GameState.Instance);
		Debug.Log (jsonData);
		UpdateUserDataRequest request = new UpdateUserDataRequest()
		{
			Data = new Dictionary<string, string>(){
				{"GameState", jsonData}
			}
		};
		PlayFabClientAPI.UpdateUserData(request, (result) =>
			{
				Debug.Log("Successfully updated game state with result: " + result.ToString());
			}, (error) =>
			{
				Debug.Log("Got error setting user data GameState");
				Debug.Log(error.ErrorDetails);
			});
				
		// save player state
		foreach(PlayerState ps in playerGames){
			if (ps.PlayerGameId == PlayerState.Current.PlayerGameId) {
				playerGames.Remove (ps);
				break;
			}
		}
		playerGames.Add(PlayerState.Current);
		foreach (PlayerState ps in playerGames) {
			jsonData = JsonUtility.ToJson (ps);
			Debug.Log (jsonData);
			request = new UpdateUserDataRequest () {
				Data = new Dictionary<string, string> () {
					{ "PlayerGame_" + ps.PlayerGameId, jsonData }
				}
			};
			PlayFabClientAPI.UpdateUserData (request, (result) => {
				Debug.Log ("Successfully updated player state: " + ps.PlayerGameId);
			}, (error) => {
				Debug.Log ("Got error setting user data PlayerState: " + ps.PlayerGameId);
				Debug.Log (error.ErrorDetails);
			});
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/gameState.ugd"); // Unity Game Data
		Debug.Log("Saving Game State to: " + Application.persistentDataPath + "/gameState.ugd");
		bf.Serialize(file, GameState.Instance);
		file.Close();

		// for now only add one instance of a player state at a time
		foreach(PlayerState ps in playerGames){
			if (ps.PlayerGameId == PlayerState.Current.PlayerGameId) {
				playerGames.Remove (ps);
				break;
			}
		}
		playerGames.Add(PlayerState.Current);

		bf = new BinaryFormatter();
		file = File.Create (Application.persistentDataPath + "/savedGames.ugd"); // Unity Game Data
		Debug.Log("Saving Player State to: " + Application.persistentDataPath + "/savedGames.ugd");
		bf.Serialize(file, playerGames);
		file.Close();

		GameManager.Instance.UpdateSinceLastSave = false;

	}

	public void LoadPlayerSettings() {

		GameManager.Instance.LoadingPlayerSettings = true;
		StartCoroutine (PlayerManager.Instance.LoadPlayerSettings(true));

	}

	public void LoadGameSettings() {

		string gameState = null;
		//List<string> playerStates = new List<string>();

		Debug.Log("Loading PlayFab Game Settings...");
		GameManager.Instance.LoadingGameSettings = true;

		GetUserDataRequest userDataRequest = new GetUserDataRequest()
		{
			PlayFabId = GameState.Instance.PlayFabUserId,
			Keys = null
		};
		PlayFabClientAPI.GetUserData(userDataRequest,(result) => {
			
			if ((result.Data == null) || (result.Data.Count == 0))
			{
				Debug.Log("No playfab user data available");
			}
			else
			{
				foreach (var item in result.Data)
				{
					Debug.Log("    " + item.Key + " = " + item.Value.Value);
					if (item.Key == "GameState") {
						Debug.Log("Loading Game Settings...");
						gameState = item.Value.Value;
					} /*else if (item.Key.Contains("PlayerGame_")) {
						Debug.Log("Loading Player Game State...");
						playerStates.Add(item.Value.Value);
					}*/
				}
			}

			// load the gamestate from user data
			if (gameState != null) {

				// last change to hold on to settings we do not want to replace
				// with ones stored on server
				if (GameState.Instance != null) {
				} 

				GameState.Instance = JsonUtility.FromJson<GameState>(gameState);
				Debug.Log("GameState Object Deserialized Successfully.");

			} 

			/*
			// load the player states from user data
			if (playerStates.Count != 0) {
				
				foreach (var playerState in playerStates) {
					PlayerState ps = JsonUtility.FromJson<PlayerState>(playerState);
					playerGames.Add (ps);
				}

				Debug.Log("All Saved Player Games Deserialized Successfully.");

			} 

			if (playerGames.Count > 0) {
				// in production mode rather than hardcoding to 0th element this is something the user will select
				PlayerState.Current = playerGames [0];
			} else {
				PlayerState.Current = new PlayerState ();
				GameState.Instance.CurrentPlayer = PlayerState.Current.PlayerGameId;

				// THIS IS THE FIRST TIME USER HAS ACCESSED THE GAME!!
				// let's assign some default values here

			}

			if (GameState.IsDevMode) {
				Debug.Log ("WARNING - IN DEV MODE!!!");

				// in production mode if SelectedCharacterId == null then just force user to assign
				// selected character
				if (PlayerState.Current.SelectedCharacterId == null){
					Debug.Log ("IN DEV MODE: No Set Primary Character Found So setting to a default one");
					PlayerState.Current.SelectedCharacterId = PlayerState.Constants.SISTER_ONE_ID;
				}
			}
			*/

			GameManager.Instance.LoadingGameSettings = false;

		}, (error) => {
			Debug.Log("Got error retrieving playfab user data:");
			Debug.Log(error.ErrorMessage);
		});
			
	}

	public void LoadTitleData() {

		// TODO: move this out into a separate method
		GameManager.Instance.LoadingWorldData = true;
		Debug.Log("Loading PlayFab World data...");
		GetTitleDataRequest request = new GetTitleDataRequest ();
		System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
		keys.Add ("WorldOneDetails");
		keys.Add ("GameWorlds");
		request.Keys = keys;

		PlayFabClientAPI.GetTitleData(request,(result) => {

			if ((result.Data == null) || (result.Data.Count == 0))
			{
				Debug.Log ("No playfab title data available");
			}
			else
			{

				foreach (var item in result.Data)
				{
					Debug.Log ("    " + item.Key + " = " + item.Value);
					if (item.Key == "GameWorlds") {
						Debug.Log ("Instantiating the list of GameWorlds");
						worlds = JsonUtility.FromJson<GameWorlds>(item.Value);
					}
				}

				foreach (var item in result.Data)
				{
					Debug.Log ("    " + item.Key + " = " + item.Value);
					if (item.Key == "WorldOneDetails") {
						Debug.Log ("Instantiating the SelectedWorld");
						selectedWorld = JsonUtility.FromJson<WorldOneDetails>(item.Value);
					}
				}

			}

			if (selectedWorld != null) {

				Debug.Log ("World loaded: " + selectedWorld.WorldId);
				Debug.Log ("Number of World Levels loaded: " + selectedWorld.LevelCount);

			} 
				
			GameManager.Instance.LoadingWorldData = false;
			Debug.Log("PlayFab World data Loaded Successfully.");

		}, (error) => {
			Debug.Log("Got error retrieving PlayFab World data: ");
			Debug.Log(error.ErrorMessage);
		});

	}

	public void LogIntoPlayfab () {
		Debug.Log ("Logging Into PlayFab...");
		PlayFabSettings.TitleId = GameState.Constants.PLAYFAB_TITLE_ID; //your title id goes here.
		LoginWithFacebookRequest request = new LoginWithFacebookRequest();
		request.AccessToken = GameState.Instance.FBUserAccessToken;
		request.TitleId = PlayFabSettings.TitleId;
		request.CreateAccount = true;
		PlayFabClientAPI.LoginWithFacebook(request, OnPlayFabLoginResult, OnPlayFabLoginError);
	}

	private void OnPlayFabLoginResult(PlayFab.ClientModels.LoginResult result) {
		GameState.Instance.PlayFabUserId = result.PlayFabId;

		Debug.Log ("User is now logged into PlayFab with the following result: " + result.PlayFabId);

		// TODO: Add the story scene and bulliten message scene (info message is something I might need all users to be aware ASAP) 
		//       to the list of possible sceens shown right after splash
		GameManager.Instance.LoadNextScene(false, true);
	}

	private void OnPlayFabLoginError(PlayFabError error) {
		Debug.Log ("User FAILED log into PlayFab with the following result: " + error);
	}

	public bool AssignDefaultData () {
		return true;
	}

	public bool AddToSceneBreadCrumb(string sceneName, string defaultMenuName) {

		// add scenee to sceen array list dont allow the following situation
		// sceneA - sceneB - sceneA
		if (GameManager.SceneBreadCrumb.Count!=0  && GameManager.SceneBreadCrumb.Contains(sceneName)) {
			return false;
		} else {
			GameManager.SceneBreadCrumb.Add (sceneName);
		}

		ArrayList newMenuList = new ArrayList();
		newMenuList.Add (defaultMenuName);

		// add a record of the scenes new menu to the scene menu dict
		if (GameManager.MenuBreadCrumbsByScene.Count!=0 && GameManager.MenuBreadCrumbsByScene.ContainsKey (sceneName)) {
			return false;
		} else {
			GameManager.MenuBreadCrumbsByScene.Add (sceneName, newMenuList);
		}

		return true;

	}

	public bool AddToMenuBreadCrumb(string menuName) {
	
		// get the menu in current scene (this is actually the current one)
		object currentScene = GetLastSceneInBreadCrumb ();

		// add the menu record to the scene
		if (currentScene != null) {
			ArrayList updatedMenuBreadCrumb = GameManager.MenuBreadCrumbsByScene.Get (currentScene);
			updatedMenuBreadCrumb.Add (menuName);
			// replace the current scene menu bread crumb with this new one
			GameManager.MenuBreadCrumbsByScene.Remove (currentScene);
			GameManager.MenuBreadCrumbsByScene.Add (currentScene, updatedMenuBreadCrumb);
		} else {
			return false;
		}

		return true;

	}

	public bool RemoveFromSceneBreadCrumb() {

		object sceneToBeRemoved = GetLastSceneInBreadCrumb ();

		// remove scene and also remove menu record for the scene
		if (GameManager.SceneBreadCrumb.Count > 0) {
			GameManager.SceneBreadCrumb.RemoveAt (GameManager.SceneBreadCrumb.Count-1);
			GameManager.MenuBreadCrumbsByScene.Remove (sceneToBeRemoved);
		} else {
			return false;
		}

		return true;

	}

	public bool RemoveFromMenuBreadCrumb () {

		// get the menu in current scene (this is actually the current one)
		object currentScene = GetLastSceneInBreadCrumb ();

		// remove menu record for the scene
		if (currentScene != null) {
			ArrayList updatedMenuBreadCrumb = GameManager.MenuBreadCrumbsByScene.Get (currentScene);
			updatedMenuBreadCrumb.RemoveAt (updatedMenuBreadCrumb.Count-1);
			// replace the current scene menu bread crumb with this new one
			GameManager.MenuBreadCrumbsByScene.Remove (currentScene);
			GameManager.MenuBreadCrumbsByScene.Add (currentScene, updatedMenuBreadCrumb);
		} else {
			return false;
		}

		return true;

	}

	public object GetLastSceneInBreadCrumb () {

		object lastScene = null;

		// remove the scene
		if (GameManager.SceneBreadCrumb.Count < 1) {
			return null;
		} else {
			// get the last scene in the Array - this si the end of the bread crumb
			IEnumerator sceneEnum = GameManager.SceneBreadCrumb.GetEnumerator ();
			while(sceneEnum.MoveNext()){
				lastScene = sceneEnum.Current;
			}
		}

		return lastScene;

	}

	public object GetLastMenuInBreadCrumb () {

		object lastMenu = null;

		// get the last scene (this is actually the current one)
		object currentScene = GetLastSceneInBreadCrumb ();

		ArrayList menusInScene = GameManager.MenuBreadCrumbsByScene.Get (currentScene);
		if (menusInScene != null) {
			// get the last menu in the Array - this si the end of the bread crumb
			IEnumerator menuEnum = menusInScene.GetEnumerator ();
			while(menuEnum.MoveNext()){
				lastMenu = menuEnum.Current;
			}
		}

		return lastMenu;

	}

	public GameCharacter GetSelectedCharacter () {
	
		return GetCharacter (PlayerState.Current.SelectedCharacterId);
	
	}

	public GameCharacter GetCharacter (string characterId) {
		
		switch (characterId)
		{
		case PlayerState.Constants.SISTER_ONE_ID: return PlayerState.Current.SisterOne;
		case PlayerState.Constants.SISTER_TWO_ID: return PlayerState.Current.SisterTwo;
		case PlayerState.Constants.SISTER_THREE_ID: return PlayerState.Current.SisterThree;
		default : return null; 
		}

	}

	public GameEnemy GetSelectedEnemy() {

		return GetEnemy(GameState.Instance.SelectedEnemyId);

	}

	public GameEnemy GetEnemy (string enemyId) {

		switch (enemyId)
		{
		case GameState.Constants.ENEMY_ONE_ID: return GameState.Instance.MonsterOne;
		default : return null; 
		}

	}

	public void LoadNextScene(bool quickLoad, bool getUserDataFromServer = false) {

		// get user data from server before loading the next scene
		// NOTE: state is held in Main Memory throughtout the game so: only do this as needed
		//       (ie game startup)
		if (getUserDataFromServer) {
			Debug.Log ("Checking if Client is Logged In...");
		} else {
			Debug.Log ("WARNING: Loading next scene without getting user data from server...");
		}
		// every new scene (except the splash scene) will result in a pull of the users data from playfab
		if (PlayFabClientAPI.IsClientLoggedIn() && getUserDataFromServer) {
			
			Debug.Log ("Yes Client is Logged In  ...Cool.");

			// Load Game Player Related Data
			LoadPlayerSettings ();

			// Load Game Player Related Data
			LoadGameSettings ();

			// Load Game Title Scoped Data
			LoadTitleData ();

			// Use game data to figure out what scene we need to load first
			// for example is story has never been shown we might start player 
			// off in the story scene
			AssignDefaultData ();

		}

		LoadingSceneManager loadSceneManager = Object.FindObjectOfType<LoadingSceneManager>();
		if (loadSceneManager != null) {
			GameState.Instance.CurrentScene = (string)GameManager.Instance.GetLastSceneInBreadCrumb();
			Debug.Log ("Loading the Next Scene (" + GameState.Instance.CurrentScene + ")...");
			loadSceneManager.LoadNextScene (GameState.Instance.CurrentScene, quickLoad);
		}

	}

	public void MoveInNextMenu (Tween tween) {

		switch ((string)GameManager.Instance.GetLastMenuInBreadCrumb()) {
		case GameState.Constants.START_MENU:
			tween.OnComplete (() => GameManager.StartMenuInScene.LoadMenu ());
			break;
		case GameState.Constants.MAIN_MENU:
			tween.OnComplete (() => GameManager.MainMenuInScene.LoadMenu ());
			break;
		case GameState.Constants.OPTIONS_MENU:
			tween.OnComplete (() => GameManager.OptionsMenuInScene.LoadMenu ());
			break;
		case GameState.Constants.GAME_SETTINGS_MENU:
			tween.OnComplete (() => GameManager.GameSettingsInScene.LoadMenu ());
			break;
		case GameState.Constants.CHARACTER_CUSTOMIZATION_MENU:
			tween.OnComplete (() => GameManager.CharacterCustomizationInScene.LoadMenu ());
			break;
		case GameState.Constants.CHARACTER_SELECTION_MENU:
			tween.OnComplete (() => GameManager.CharacterSelectionInScene.LoadMenu ());
			break;
		default:
			break;
		}

	}

	public void ExitMenu (bool saveSettings) {

		string lastMenu = (string)GameManager.Instance.GetLastMenuInBreadCrumb ();

		Debug.Log ("Exiting Menu " + lastMenu + " now.");
		switch (lastMenu) {
		case GameState.Constants.START_MENU:
			GameManager.StartMenuInScene.ExitMenu (saveSettings);
			break;
		case GameState.Constants.MAIN_MENU:
			GameManager.MainMenuInScene.ExitMenu (saveSettings);
			break;
		case GameState.Constants.GAME_SETTINGS_MENU:
			GameManager.GameSettingsInScene.ExitMenu (saveSettings);
			break;
		case GameState.Constants.CHARACTER_CUSTOMIZATION_MENU:
			GameManager.CharacterCustomizationInScene.ExitMenu (saveSettings);
			break;
		case GameState.Constants.CHARACTER_SELECTION_MENU:
			GameManager.CharacterSelectionInScene.ExitMenu (saveSettings);
			break;
		case GameState.Constants.HUD_MENU:
			GameManager.HUDMenuInScene.ExitMenu (saveSettings);
			break;
		default:
			Debug.Log ("WARNING: attempting to Exit a Menu with label: " + lastMenu + " that was not found!");
			Debug.Log ("WARNING: Not Exiting Menu.");
			break;
		}

	}

	public static void SetLayerForAllChildren(GameObject theParent, string layerName) {
		Debug.Log ("setting game object "+theParent.name+"'s children layer to: "+layerName);
		int layerNumber = LayerMask.NameToLayer (layerName);
		if (theParent == null) return;
		foreach (Transform trans in theParent.GetComponentsInChildren<Transform>(true)) {
			trans.gameObject.layer = layerNumber;
		}
	}

	// Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
	public string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return "#" + hex;
	}

	// Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
	public Color HexToColor(string hex)
	{

		if (hex.Contains("#")) {
			hex = hex.Substring (1);
		}

		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

		return new Color32(r,g,b, 255);

	}

	public GameObject LoadCharacterAvatar (string avatarId) {

		GameObject avatar = null;

		// initialize avatar the using prefab
		string fileName = avatarId + "Avatar";
		string fileLocation = "Characters/"+avatarId+"/"+fileName;
		avatar = Instantiate(Resources.Load(fileLocation, typeof(GameObject))) as GameObject;

		return avatar;

	}

	public GameObject LoadEnemyAvatar (string avatarId) {

		GameObject avatar = null;

		// initialize avatar the using prefab
		string fileName = avatarId + "Avatar";
		string fileLocation = "Enemies/"+avatarId+"/"+fileName;
		avatar = Instantiate(Resources.Load(fileLocation, typeof(GameObject))) as GameObject;

		return avatar;

	}

	public GameObject AddBackgroundToScene (string bgName) {

		GameObject bg = null;

		string fileLocation = "Backgrounds/"+bgName;
		bg = Instantiate(Resources.Load(fileLocation, typeof(GameObject))) as GameObject;
		bg.name = bgName;

		return bg;

	}

	public bool RemoveBackgroundFromScene (string bgName) {

		GameObject bg = null;

		bg = GameObject.Find(bgName);
		if (bg != null) {
			Destroy(bg);
		} else {
			return false;
		}

		return true;

	}

	private bool updateSinceLastSave = false;
	public bool UpdateSinceLastSave {
		get { return updateSinceLastSave; }
		set { updateSinceLastSave = value; }
	}

	private WorldOneDetails selectedWorld = null;
	public WorldOneDetails SelectedWorld {
		get { return selectedWorld; }
		set { selectedWorld = value; }
	}

	private GameObject currentWorldMap = null;
	public GameObject CurrentWorldMap {
		get { return currentWorldMap; }
		set { currentWorldMap = value; }
	}

	private GameWorlds worlds = null;
	public GameWorlds Worlds {
		get { return worlds; }
		set { worlds = value; }
	}

	public LevelDetail GetPreviousGameLevel (Level currentLevel, WorldDetails worldDetails) {

		return null;

	}

	public LevelDetail GetNextGameLevel (Level currentLevel, WorldDetails worldDetails) {

		return null;

	}

	public static Texture2D LoadImageAsTexture(string filePath) {

		Texture2D tex = null;

		//if (File.Exists(filePath))     {
			tex = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D));
		//}

		return tex;
	}

	public GameCharacter GetHeroInstanceFromID (string heroId) {

		switch (heroId) {
		case CharacterOne.ID:
			return CharacterOne.Instance;
		case CharacterTwo.ID:
			return CharacterTwo.Instance;
		case CharacterThree.ID:
			return CharacterThree.Instance;
		default:
			return null;
		}

	} 

}
