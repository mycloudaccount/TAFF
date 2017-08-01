using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	PlayMakerFSM[] playMakerFSMs;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("Starting CharacterManager");
		playMakerFSMs = gameObject.transform.parent.gameObject.GetComponents<PlayMakerFSM>();
		playMakerFSMs[0].FsmVariables.GetFsmBool("SceneLoaded").Value = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool PrepareCharacterForScene () {

		Debug.Log ("PrepareCharacterForScene has been called in scene: " + GameState.Instance.CurrentScene);

		// configure all player components
		if (GameState.Instance.CurrentScene == GameState.Constants.SCENE_START_MENU) {
			EnableCharacterComponentsForMainMenu ();
		} else if (GameState.Instance.CurrentScene == GameState.Constants.SCENE_MAIN_MENU) {
			EnableCharacterComponentsForMainMenu ();
		} else if (GameState.Instance.CurrentScene == GameState.Constants.SCENE_CHARACTER_CUSTOMIZATION) {
			EnableCharacterComponentsForCharCustView ();
			// set layer to onstage
			GameManager.SetLayerForAllChildren (gameObject, "OnStage");
		} else if (GameState.Instance.CurrentScene == GameState.Constants.SCENE_CHARACTER_SELECTION) {
			EnableCharacterComponentsForCharSelectionView ();
			// set layer to onstage
			GameManager.SetLayerForAllChildren (gameObject, "OnStage");
		} else if (GameState.Instance.CurrentScene.StartsWith("Challenge")) {
			EnableCharacterComponentsForChallengeView ();
			// set layer to onstage
			GameManager.SetLayerForAllChildren (gameObject, "HeroStage");
		} else {
			Debug.Log ("PLEASE MAKE SURE THAT THE SCENE HAS BEEN ADDED TO: PrepareCharacterForScene() of CharacterManager.cs");
			playMakerFSMs[0].Fsm.Event("PlayerNotPrepared");
			return false;
		}

		LoadCharacterCustomization (gameObject);

		Debug.Log ("The character settings are now: ");
		Debug.Log ("scale - " + gameObject.transform.localScale);

		playMakerFSMs[0].Fsm.Event("PlayerPrepared");
	
		return true;

	}

	// load the players saved customizations (hair color, clothes colors, etc...)
	private void LoadCharacterCustomization (GameObject playerAvatar) {

		// access GameState.Instance.PlayerState to get all the customization settings
		// for this character (if player is using all three chars there
		// will be customization settings for each one)


	}

	private void EnableCharacterComponentsForMainMenu() {

		foreach(var component in gameObject.GetComponents<Component>())
		{

			switch(component.GetType ().Name){

				case Constants.NAVAGENTMOVER:
					component.GetComponent<NavAgentMover> ().enabled = true;
					break;
				case Constants.PLAYMAKERFSM:
					PlayMakerFSM pmfsm = component.GetComponent<PlayMakerFSM> ();
					Debug.Log ("Preparing FSM: " + pmfsm.FsmName + " for character " + pmfsm.gameObject.name);

					// set the time we will wait before having the character walk onto the stage
					if (pmfsm.FsmName == "PositionControllerFSM") {

						if (pmfsm.gameObject.name.Contains ("Ayo")) {
							pmfsm.FsmVariables.GetFsmFloat ("TimeToWaitBackstage").Value = Random.Range(12, 18);
						} else {
							pmfsm.FsmVariables.GetFsmFloat("TimeToWaitBackstage").Value =  Random.Range(0, 10);
						}

					}

					pmfsm.enabled = true;
					break;
				case Constants.NAVMESHAGENT:
					component.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = true;
					break;
				case Constants.ANIMATOR:
					component.GetComponent<Animator> ().enabled = true;
					break;
				default:
					break;

			}

		}

	}

	private void EnableCharacterComponentsForCharCustView() {

		foreach(var component in gameObject.GetComponents<Component>())
		{

			switch(component.GetType ().Name){

			case Constants.BOTCONTROLSCRIPT:
				component.GetComponent<BotControlScript> ().enabled = true;
				break;
			//case Constants.CAPSULECOLLIDER:
			//	component.GetComponent<CapsuleCollider> ().enabled = true;
			//	break;
			case Constants.ANIMTRIGGERS:
				component.GetComponent<AnimTriggers> ().enabled = true;
				break;
			case Constants.ANIMATOR:
				component.GetComponent<Animator> ().enabled = true;
				break;
			//case Constants.TOONCHARACTERCONTROLLER:
			//	component.GetComponent<ToonCharacterController> ().enabled = true;
			//	break;
			default:
				break;

			}

		}

	}

	private void EnableCharacterComponentsForCharSelectionView() {

		foreach(var component in gameObject.GetComponents<Component>())
		{

			switch(component.GetType ().Name){

			case Constants.BOTCONTROLSCRIPT:
				component.GetComponent<BotControlScript> ().enabled = true;
				break;
			case Constants.ANIMTRIGGERS:
				component.GetComponent<AnimTriggers> ().enabled = true;
				break;
			case Constants.ANIMATOR:
				component.GetComponent<Animator> ().enabled = true;
				break;
			default:
				break;

			}

		}

	}
		
	private void EnableCharacterComponentsForChallengeView() {

		foreach(var component in gameObject.GetComponents<Component>())
		{

			switch(component.GetType ().Name){

			case Constants.BOTCONTROLSCRIPT:
				component.GetComponent<BotControlScript> ().enabled = true;
				break;
			case Constants.ANIMTRIGGERS:
				component.GetComponent<AnimTriggers> ().enabled = true;
				break;
			case Constants.ANIMATOR:
				component.GetComponent<Animator> ().enabled = true;
				break;
			default:
				break;

			}

		}


		// let's also scale the player to approriate size
		gameObject.transform.localScale = new Vector3 (0.88f,0.88f,0.88f);

	}


	public static class Constants
	{
		
		// list of components attached to a character game object
		public const string TRANSFORM = "Transform";
		public const string ANIMATOR = "Animator";
		public const string BOTCONTROLSCRIPT = "BotControlScript";
		public const string RIGIDBODY = "Rigidbody";
		public const string CAPSULECOLLIDER = "CapsuleCollider";
		public const string ANIMTRIGGERS = "AnimTriggers";
		public const string TOONCHARACTERCONTROLLER = "ToonCharacterController";
		public const string NAVMESHAGENT = "NavMeshAgent";
		public const string PLAYMAKERFSM = "PlayMakerFSM";
		public const string NAVAGENTMOVER = "NavAgentMover";
		public const string PLAYERMANAGER = "PlayerManager";

	}

}
