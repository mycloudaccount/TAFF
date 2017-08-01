using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.AI;
using SWS;
using System.Text.RegularExpressions;
using System;

public class LittleHeroManager : MonoBehaviour {

	Hero hero = null;
	public string HeroId;

	// workers that put together out hero
	HeadGearManager headGearManager;

	void Awake () {

		gameObject.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);

		headGearManager = new HeadGearManager ();

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (!getHeroConfigStatus ()) { // && !GameManager.Instance.LoadingHeroData) {

			switch (GameState.Instance.CurrentScene) {
			case GameState.Constants.SCENE_START_MENU:
				hero = HeroManager.Instance.Heros[HeroId];
				Debug.Log ("Found for a Little Hero with Id: " + hero.Id + "...");
				customizeHero ();
				configureAnimatorController ();
				configureStartMenuHeroMovement ();
				orientHero (0.6f, 1.8f, 180.0f);
				break;
			case GameState.Constants.SCENE_CHARACTER_SELECTION:
				hero = getHeroFromCurrentId ();
				Debug.Log ("Found for a Little Hero with Id: " + hero.Id + "...");
				customizeHero ();
				configureAnimatorController ();
				configureHeroForSelectionStage ();
				orientHero (1.0f, 5.5f, 200.0f);
				break;
			case GameState.Constants.SCENE_CHARACTER_CUSTOMIZATION:
				hero = getHeroFromCurrentId ();
				Debug.Log ("Found for a Little Hero with Id: " + hero.Id + "...");
				customizeHero ();
				configureAnimatorController ();
				configureHeroForCustomizationStage ();
				break;
			default:
				Debug.Log("Found unexpected scene.  Not going to place a hero here.");
				break;					
			}

			setHeroConfigStatus (true);
		
		}
	}

	private bool getHeroConfigStatus () {
	
		bool isConfigured = true;

		GameCharacter gC = GameManager.Instance.GetSelectedCharacter ();
		if (hero == null) {
			isConfigured = false;
			if (gC.Id == this.HeroId) {
				gC.IsHeroConfigured = false;
			}
		} else if (gC.Id == hero.Id) {
			isConfigured = gC.IsHeroConfigured;
		}
	
		return isConfigured;

	}

	private void setHeroConfigStatus (bool cStatus) {
	
		GameCharacter gC = GameManager.Instance.GetSelectedCharacter ();
		gC.IsHeroConfigured = cStatus;
	
	}

	private Hero getHeroFromCurrentId () {
		Hero theHero = HeroManager.Instance.Heros [PlayerState.Current.SelectedCharacterId];
		if (theHero == null) {
			PlayerState.Current.SelectedCharacterId = HeroId;
		}

		return HeroManager.Instance.Heros [PlayerState.Current.SelectedCharacterId];
	}

	private void customizeHero () {
		if (hero != null) {
			Debug.Log ("OK Found a Little Hero called: " + hero.FirstName + " " + hero.LastName);

			// check for exsisting settings for this hero.  if you do not find any just go with
			// default settings and set the associated heroInstance's setting to all initial default
			// settings
			GameCharacter gc = GameManager.Instance.GetHeroInstanceFromID (hero.Id);
			gc.AvatarId = hero.Id;
			gc.Id = hero.Id;

			string hairPath;
			if (gc.HairPath != null && gc.HairPath != "") {
				hairPath = gc.HairPath;
			} else {
				hairPath = hero.HairPath;
				gc.HairPath = hairPath;
			}
			headGearManager.AddComponent (gameObject, gc, HeadGearManager.Constants.HAIR);

			string clothesPath;
			if (gc.ClothesPath != null && gc.ClothesPath != "") {
				clothesPath = gc.ClothesPath;
			} else {
				clothesPath = hero.ClothesPath;
				gc.ClothesPath = clothesPath;
			}
			addClothes (gc);

			//headGearManager.AddComponent (gameObject, gc, HeadGearManager.Constants.HEADBAND);

		}
	}

	private void addClothes (GameCharacter gc) {
		string pathToHeroBase = "LittleHeroBase/Base";
		Transform theComponent = gameObject.transform.Find(pathToHeroBase);
		Utils.addMaterial (
			theComponent,
			"LittleHeros/Textures/" + gc.ClothesPath
		);
		Utils.addToonShader (theComponent);
	}    

	private void orientHero (float timeToEnter, float scaleFactor, float rotateFactor) {
		gameObject.transform.DORotate (new Vector3 (0.0f, rotateFactor, 0.0f), timeToEnter, RotateMode.LocalAxisAdd).SetEase (Ease.Linear).SetDelay (0.0f);
		gameObject.transform.DOScale (new Vector3 (scaleFactor, scaleFactor, scaleFactor), timeToEnter).SetEase (Ease.Linear).SetDelay (0.0f);
	}

	private void configureAnimatorController () {
		if (hero != null) {
			Transform BaseNormal = gameObject.transform.Find ("LittleHeroBase");
			Animator animator = BaseNormal.GetComponent<Animator> ();
			animator.runtimeAnimatorController = Resources.Load (hero.AnimatorController) as RuntimeAnimatorController;
		}
	}

	private void configureStartMenuHeroMovement () {
		Transform BaseNormal = gameObject.transform.Find("LittleHeroBase");
		NavMeshAgent nma = BaseNormal.GetComponent<NavMeshAgent>();
		nma.enabled = true;
		navMove nm = BaseNormal.GetComponent<navMove>();
		nm.enabled = true;
		HeroMotionController hmc = BaseNormal.GetComponent<HeroMotionController>();
		hmc.enabled = true;
	}

	private void configureHeroForSelectionStage () {
		if (hero != null) {
			Transform BaseNormal = gameObject.transform.Find("LittleHeroBase");
			GameManager.SetLayerForAllChildren (BaseNormal.gameObject, GameState.Constants.LAYER_ON_STAGE);
			BaseNormal.parent.position = new Vector3 (0.0f, -2.7f, 0.0f);
			BaseNormal.parent.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}
	}

	private void configureHeroForCustomizationStage () {
		if (hero != null) {
			Transform BaseNormal = gameObject.transform.Find("LittleHeroBase");
			GameManager.SetLayerForAllChildren (BaseNormal.gameObject, GameState.Constants.LAYER_ON_STAGE);
		}
	}

}
