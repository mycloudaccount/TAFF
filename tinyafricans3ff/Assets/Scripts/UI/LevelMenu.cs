using UnityEngine;
using System.Collections;
using MarkLight;
using MarkLight.Views.UI;
using DG.Tweening;
using System.Collections.Generic;

public class LevelMenu : UIView
{

	public Region LevelTitleMountRegion;
	public Region LevelTitleRegion;
	public Label LevelTitle;
	public Label LevelDescription;
	private bool isLevelPanelOffScreen = true;
	public Region GoButtonRegion;

	public Region NavigationRegion;
	public Image NavigationRegionMountImage;
	public Image SunRaysTypeA01;
	public Material SunRaysMaterialTypeA;

	public Button LeftGreenArrowButton;
	public Button RightGreenArrowButton;

	private GameObject levelOneArrow;
	private GameObject levelTwoArrow;
	private GameObject levelThreeArrow;
	private GameObject levelFourArrow;

	void Awake(){
		levelOneArrow = GameObject.Find("Arrow1");		
		levelTwoArrow = GameObject.Find("Arrow2");		
		levelThreeArrow = GameObject.Find("Arrow3");		
		levelFourArrow = GameObject.Find("Arrow4");		
	}

	void Start () {
		InitializeView ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public override void Initialize () {

		base.Initialize ();

		SunRaysMaterialTypeA = Resources.Load ("Particles/Materials/Light_02", typeof(Material)) as Material;
		SunRaysMaterialTypeA.SetColor ("_TintColor", Color.magenta);

	}

	void InitializeView () {

		// set the Selected level and move to it (if nothing is select then start the player at level one)
		Debug.Log ("Has the player already selected a level to play?");
		if (PlayerState.Current.SelectedLevel == null || PlayerState.Current.SelectedLevel.Primed == false) {
			Debug.Log ("No");
			Debug.Log("Assigning Level One as players selected level: " + GameManager.Instance.SelectedWorld.ListOfLevels.LevelOne);
			PlayerState.Current.SelectedLevel = PrimeNewLevel(GameManager.Instance.SelectedWorld.ListOfLevels.LevelOne);
		} else {
			Debug.Log ("Yes");
		}
		LevelDetail selectedLevelDetail = GameManager.Instance.SelectedWorld.GetLevelFromId (PlayerState.Current.SelectedLevel.LevelId);
		Debug.Log ("The Players Selected Level is: " + selectedLevelDetail.LevelTitle);

		UpdateLevelInfoPanel (selectedLevelDetail, null);
		PointToLevelMarker (selectedLevelDetail, null);

		// I need to wait on some position values to get set
		// accurately
		StartCoroutine(AnimateSlowerLoadingObject());

		StartSunRayAnimations ();

	}

	IEnumerator AnimateSlowerLoadingObject () {
		
		yield return new WaitForSeconds(1);

		LoadMenu ();

		// Move to the correct level location  on the world map
		RotateMap ();
	
	}
		
	public void RotateMap () {
		
		if ( GameManager.Instance.CurrentWorldMap != null) {
			LevelDetail selectedLevelDetail = GameManager.Instance.SelectedWorld.GetLevelFromId (PlayerState.Current.SelectedLevel.LevelId);
			Debug.Log ("PlayerState.Current.SelectedLevel.AngleOnMap: " + selectedLevelDetail.AngleOnMap);
			float currentWorldMapRotation = GameManager.Instance.CurrentWorldMap.transform.localRotation.eulerAngles.z;
			Debug.Log ("Current World Map local z rotation: " + currentWorldMapRotation);
			float mapRotationAmount = -1.0f * (currentWorldMapRotation - 360.0f) + selectedLevelDetail.AngleOnMap;
			Debug.Log ("Moving Map by the following amount: " + mapRotationAmount);
			GameManager.Instance.CurrentWorldMap.transform.DORotate (new Vector3 (0.0f, 0.0f, mapRotationAmount), 1.0f, RotateMode.LocalAxisAdd).SetRelative();
		}

	}

	public void PunchMap (int direction) {

		if ( GameManager.Instance.CurrentWorldMap != null) {
			GameManager.Instance.CurrentWorldMap.transform.DOPunchRotation(new Vector3(0.0f,0.0f,direction * 5.0f), 0.5f, 5, 0.5f);
		}

	}

	public void PointToLevelMarker (LevelDetail newLd, LevelDetail oldLd) {

		if (oldLd != null) {
			GameObject oldArrow = GetArrowByName ("Arrow" + oldLd.LevelId);
			MoveArrowOut (oldArrow.transform);
		}

		if (newLd != null) {
			GameObject newArrow = GetArrowByName ("Arrow" + newLd.LevelId);
			MoveArrowIn (newArrow.transform);
		}

	}

	public void UpdateLevelInfoPanel (LevelDetail newLd, LevelDetail oldLd) {

		if (oldLd != null) {
			MoveLevelTitleRegionOut ();
		}

		if (newLd != null) {
			LevelTitle.Text.Value = newLd.LevelTitle;
			LevelDescription.Text.Value = newLd.LevelDescription;
			StartCoroutine(WaitToMoveLevelPanelIn());
		}

	}

	IEnumerator WaitToMoveLevelPanelIn () {

		while( isLevelPanelOffScreen == false )
		{
			yield return null;
		}

		MoveLevelTitleRegionIn ();

	}

	private void MoveArrowIn (Transform trans) {

		trans.DOScale (new Vector3 (3.7f, 3.7f, 3.7f), 0.1f);
		trans.DOLocalMoveY (10.0f , 1.5f, true).SetDelay(0.1f).OnComplete (() => BounceArrow(trans));

	}

	private void BounceArrow (Transform trans) {
		
		trans.DOScale (new Vector3 (3.7f, 3.7f, 4.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);

	}

	private void MoveArrowOut (Transform trans) {

		trans.DOLocalMoveY (120.0f, 1.5f, true).OnComplete (() => KillArrow(trans));

	}

	private void KillArrow (Transform trans) {

		trans.DOKill(true);

	}

	private GameObject GetArrowByName (string arrowName) {
	
		switch (arrowName) {

		case "Arrow1":
			return levelOneArrow;
		case "Arrow2":
			return levelTwoArrow;
		case "Arrow3":
			return levelThreeArrow;
		case "Arrow4":
			return levelFourArrow;
		default:
			return null;

		}

	}

	public void MoveLevelSelectorsOut () {

		LeftGreenArrowButton.transform.DOScale (new Vector3 (0.0f, 0.0f, 0.0f), 0.1f);
		RightGreenArrowButton.transform.DOScale (new Vector3 (0.0f, 0.0f, 0.0f), 0.1f);

	}

	public void MoveLevelSelectorsIn () {

		LeftGreenArrowButton.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.3f);
		RightGreenArrowButton.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.3f);

	}

	public void MoveLevelTitleRegionIn () {

		LevelTitleRegion.transform.DOMove (LevelTitleMountRegion.transform.position, 1, true).
		SetEase(Ease.OutBounce).SetDelay(0.0f).
		OnComplete(() => AnimateGoButton(false));

	}

	public void MoveLevelTitleRegionOut () {

		GoButtonRegion.transform.DOScaleX (0.0f, 0.2f).SetEase(Ease.Linear).OnComplete(() => KillButton ());

		LevelTitleRegion.transform.DOLocalMoveY (400.0f, 0.7f, true).
		SetEase(Ease.Linear).SetDelay(0.2f).
		OnComplete(() => AnimateGoButton(true));

	}

	public void MoveNavigationRegionIn () {

		NavigationRegion.transform.DORotate (new Vector3(0,0,0), 0.3f, RotateMode.Fast).SetEase(Ease.OutBounce).SetDelay(0.8f);
		NavigationRegion.transform.DOMove (NavigationRegionMountImage.transform.position, 1, true).SetEase(Ease.OutBounce).SetDelay(0.8f);

	}

	public void MoveNavigationRegionOut (bool changeScene) {

		NavigationRegion.transform.DOKill (false);
		Tween navTween = NavigationRegion.transform.DOMove (new Vector3 (NavigationRegion.transform.position.x-200, NavigationRegion.transform.position.y, NavigationRegion.transform.position.z) , 0.5f, true).SetEase(Ease.InExpo);

		if (changeScene) {
			// go to the next scene
			navTween.OnComplete (()=>LoadNextScene());
		} else {
			GameManager.Instance.MoveInNextMenu (navTween);
		}

	}

	public void LoadNextScene () {
	
		Debug.Log ("Done moving nav region out.  Let's move onto the next scene!");
		GameManager.Instance.LoadNextScene (false);
	
	}

	private void LoadMenuSettings () {

	}

	public void  LoadMenu () {

		LoadMenuSettings ();
		MoveMenuIn ();

	}

	private void MoveMenuIn () {

		MoveNavigationRegionIn ();

	}

	public void StartSunRayAnimations() {

		SunRaysMaterialTypeA.DOColor(Color.cyan, "_TintColor", 5).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		SunRaysTypeA01.transform.DORotate (new Vector3 (0.0f, 0.0f, 180.0f), 5, RotateMode.LocalAxisAdd).SetEase (Ease.Linear).SetLoops (-1);

	}

	public void DecreaseLevel (Button btn) {

		Debug.Log ("Changing Levels -");
		// Get lower level (if this is the lowest level then do nothing)
		if (GameManager.Instance.SelectedWorld.LevelCount != 0) {
			LevelDetail lDetail = GameManager.Instance.SelectedWorld.GetPreviousLevel (PlayerState.Current.SelectedLevel);
			if (lDetail != null) {
				// get button out of the way so user does not mess things up
				MoveLevelSelectorsOut ();
				LevelDetail selectedLevelDetail = GameManager.Instance.SelectedWorld.GetLevelFromId (PlayerState.Current.SelectedLevel.LevelId);
				UpdateLevelInfoPanel (lDetail, selectedLevelDetail);
				PointToLevelMarker (lDetail, selectedLevelDetail);
				PlayerState.Current.SelectedLevel = PrimeNewLevel(lDetail);
				RotateMap ();
			} else {
				PunchMap (-1);
			}
		}

	}

	public void IncreaseLevel (Button btn) {

		Debug.Log ("Changing Levels +");
		// Get higher level (if this is the lowest level then do nothing)
		if (GameManager.Instance.SelectedWorld.LevelCount != 0) {
			LevelDetail lDetail = GameManager.Instance.SelectedWorld.GetNextLevel (PlayerState.Current.SelectedLevel);
			if (lDetail != null) {
				// get button out of the way so user does not mess things up
				MoveLevelSelectorsOut ();
				LevelDetail selectedLevelDetail = GameManager.Instance.SelectedWorld.GetLevelFromId (PlayerState.Current.SelectedLevel.LevelId);
				UpdateLevelInfoPanel (lDetail, selectedLevelDetail);
				PointToLevelMarker (lDetail, selectedLevelDetail);
				PlayerState.Current.SelectedLevel = PrimeNewLevel(lDetail);
				RotateMap ();
			} else {
				PunchMap (1);
			}
		}

	}

	// create new level based on level (level one, level two, etc...) details
	public Level PrimeNewLevel (LevelDetail levelDetails) {

		Level newLevel = new Level();

		newLevel.LevelId = levelDetails.LevelId;

		return newLevel;

	}

	private void AnimateGoButton (bool isLevelPanelOffScreen) {
	
		SetLevelPanelOffScreen (isLevelPanelOffScreen);

		if (isLevelPanelOffScreen == false) {
			GoButtonRegion.transform.DOScaleX (1.0f, 1.0f).SetEase(Ease.OutBounce).OnComplete (() => LoopGoButton ());
		}

	}

	private void KillButton () {
	
		GoButtonRegion.transform.DOKill (false);	

	}

	private void LoopGoButton () {

		MoveLevelSelectorsIn ();
		GoButtonRegion.transform.DOPunchScale(new Vector3(0.1f,0.0f,0.0f), 0.4f, 4, 0.4f).SetLoops (-1, LoopType.Restart);

	}

	public void SetLevelPanelOffScreen (bool isLevelPanelOffScreen) {

		this.isLevelPanelOffScreen = isLevelPanelOffScreen;
	
	}

	public void Go (Button btn) {

		Debug.Log ("Adding the following scene to the bread crumb: " + "Level"+PlayerState.Current.SelectedLevel.LevelId);
		GameManager.Instance.AddToSceneBreadCrumb ("Level"+PlayerState.Current.SelectedLevel.LevelId, GameState.Constants.HUD_MENU);
		MoveLevelTitleRegionOut ();
		MoveLevelSelectorsOut ();
		MoveNavigationRegionOut (true);

	}

}

