using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using DG.Tweening;
using MarkLight;

public class CharacterSelection : UIView
{

	// whether or not exiting this view results in a scene change
	private bool ChangeScene = true;

	public Region MainContentRegion;
	public Image MainContentCoverImage;

	public string SisterOneTitle = CharacterOne.FIRST_NAME;
	public string SisterTwoTitle = CharacterTwo.FIRST_NAME;
	public string SisterThreeTitle = CharacterThree.FIRST_NAME;

	public string SisterOneLabel = CharacterOne.FIRST_NAME;
	public string SisterTwoLabel = CharacterTwo.FIRST_NAME;
	public string SisterThreeLabel = CharacterThree.FIRST_NAME;

	public string SisterOneDescription = CharacterOne.DESCRIPTION;
	public string SisterTwoDescription = CharacterTwo.DESCRIPTION;
	public string SisterThreeDescription = CharacterThree.DESCRIPTION;

	public Image SisterOneFaceOpened;
	public Image SisterTwoFaceOpened;
	public Image SisterThreeFaceOpened;
	public Image SisterOneFaceClosed;
	public Image SisterTwoFaceClosed;
	public Image SisterThreeFaceClosed;
	private bool SisterOneBlinkingEyes = false;
	private bool SisterTwoBlinkingEyes = false;
	private bool SisterThreeBlinkingEyes = false;

	public Region SisterOneTabRegion;
	public Region SisterTwoTabRegion;
	public Region SisterThreeTabRegion;

	public TabPanel CharacterSelectionTabPanel;
	private bool tabPanelLoaded = false;

	public Image SisterOnePlatformImage;
	public Image SisterOnePlatformMarker;
	public Image CloudTypeAImage00;
	public Image CloudTypeALeftMarker00;
	public Image CloudTypeARightMarker00;
	public Image CloudTypeAImage10;
	public Image CloudTypeALeftMarker10;
	public Image CloudTypeARightMarker10;

	public Image CloudTypeAImage01;
	public Image CloudTypeALeftMarker01;
	public Image CloudTypeARightMarker01;
	public Image CloudTypeAImage11;
	public Image CloudTypeALeftMarker11;
	public Image CloudTypeARightMarker11;

	public Image CloudTypeAImage02;
	public Image CloudTypeALeftMarker02;
	public Image CloudTypeARightMarker02;
	public Image CloudTypeAImage12;
	public Image CloudTypeALeftMarker12;
	public Image CloudTypeARightMarker12;

	private GameObject characterInView = null;
	private string initialPrimaryCharacterId;

	// char stats
	public Label SisterOneScore;
	public Label SisterOneHealth;
	public Label SisterOneLives;
	public Label SisterTwoScore;
	public Label SisterTwoHealth;
	public Label SisterTwoLives;
	public Label SisterThreeScore;
	public Label SisterThreeHealth;
	public Label SisterThreeLives;

	// Use this for initialization
	void Start ()
	{
		InitializeView ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// if the sisters eyes are opened close then (in some random amount of time)
		if (!SisterOneBlinkingEyes) {
			SisterOneBlinkingEyes = true;
			StartCoroutine( BlinkSisterOnesEyes (4.0f*Random.value) );
		}
		if (!SisterTwoBlinkingEyes) {
			SisterTwoBlinkingEyes = true;
			StartCoroutine( BlinkSisterTwosEyes (4.0f*Random.value) );
		}
		if (!SisterThreeBlinkingEyes) {
			SisterThreeBlinkingEyes = true;
			StartCoroutine( BlinkSisterThreesEyes (4.0f*Random.value) );
		}
	}

	public override void Initialize () {

		base.Initialize ();

		Canvas canvas = gameObject.transform.parent.GetComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		GameManager.SetLayerForAllChildren (gameObject.transform.parent.gameObject, "OffStage");

		SisterOneLabel = SisterOneLabel.ToUpper ();
		SisterTwoLabel = SisterTwoLabel.ToUpper ();
		SisterThreeLabel = SisterThreeLabel.ToUpper ();

	}

	void InitializeView () {

		initialPrimaryCharacterId = PlayerState.Current.SelectedCharacterId;

		// I need to wait on some position values to get set
		// accurately
		StartCoroutine(AnimateSlowerLoadingObject());

	}

	IEnumerator AnimateSlowerLoadingObject () {
		
		yield return new WaitForSeconds(1);

		CloudTypeAImage00.transform.position = CloudTypeARightMarker00.transform.position;
		Tween tween = CloudTypeAImage00.transform.DOMove (CloudTypeALeftMarker00.transform.position, 2.0f, false);
		tween.SetEase (Ease.Linear);
		tween.SetLoops (-1, LoopType.Restart);

		CloudTypeAImage10.transform.position = CloudTypeARightMarker10.transform.position;
		tween = CloudTypeAImage10.transform.DOMove (CloudTypeALeftMarker10.transform.position, 1.8f, false);
		tween.SetEase (Ease.Linear);
		tween.SetLoops (-1, LoopType.Restart);

		CloudTypeAImage01.transform.position = CloudTypeARightMarker01.transform.position;
		tween = CloudTypeAImage01.transform.DOMove (CloudTypeALeftMarker01.transform.position, 2.0f, false);
		tween.SetEase (Ease.Linear);
		tween.SetLoops (-1, LoopType.Restart);

		CloudTypeAImage11.transform.position = CloudTypeARightMarker11.transform.position;
		tween = CloudTypeAImage11.transform.DOMove (CloudTypeALeftMarker11.transform.position, 1.8f, false);
		tween.SetEase (Ease.Linear);
		tween.SetLoops (-1, LoopType.Restart);

		CloudTypeAImage02.transform.position = CloudTypeARightMarker02.transform.position;
		tween = CloudTypeAImage02.transform.DOMove (CloudTypeALeftMarker02.transform.position, 2.0f, false);
		tween.SetEase (Ease.Linear);
		tween.SetLoops (-1, LoopType.Restart);

		CloudTypeAImage12.transform.position = CloudTypeARightMarker12.transform.position;
		tween = CloudTypeAImage12.transform.DOMove (CloudTypeALeftMarker12.transform.position, 1.8f, false);
		tween.SetEase (Ease.Linear);
		tween.SetLoops (-1, LoopType.Restart);

		// switch tabs to the one associated with the current primary character
		int tIndex = GetTabIndexFromName (PlayerState.Current.SelectedCharacterId);
		CharacterSelectionTabPanel.SelectTab(tIndex, true);
		// add current primary character to the view
		MoveInCharacter (PlayerState.Current.SelectedCharacterId);

		// set the character stats
		SisterOneScore.Text.Value = PlayerState.Current.SisterOne.Score + " pts";
		SisterOneHealth.Text.Value = PlayerState.Current.SisterOne.Health.ToString() + " hit pnts";
		SisterOneLives.Text.Value = PlayerState.Current.SisterOne.Lives.ToString() + " lives";
		SisterTwoScore.Text.Value = PlayerState.Current.SisterTwo.Score + " pts";
		SisterTwoHealth.Text.Value = PlayerState.Current.SisterTwo.Health.ToString() + " hit pnts";
		SisterTwoLives.Text.Value = PlayerState.Current.SisterTwo.Lives.ToString() + " lives";
		SisterThreeScore.Text.Value = PlayerState.Current.SisterThree.Score + " pts";
		SisterThreeHealth.Text.Value = PlayerState.Current.SisterThree.Health.ToString() + " hit pnts";
		SisterThreeLives.Text.Value = PlayerState.Current.SisterThree.Lives.ToString() + " lives";

		LoadMenu ();

	}

	private void LoadMenuSettings () {

	}

	public void  LoadMenu () {
		LoadMenuSettings ();
		MoveMenuIn ();
	}

	private void MoveMenuIn () {
		MainContentRegion.transform.DOKill (false);
		MainContentCoverImage.transform.DOScaleX (0.0f, 1.0f).SetEase(Ease.OutBounce);
		MainContentRegion.transform.DOScaleX (1.0f, 1.0f).SetEase(Ease.InExpo);
		tabPanelLoaded = true;
	}

	public void Save (Button btn) {
		ExitMenu (true);
	}

	public void Exit (Button btn) {

		// has the selected character changed since prior to entering this scene
		if (initialPrimaryCharacterId != PlayerState.Current.SelectedCharacterId) {
			GameManager.Instance.UpdateSinceLastSave = true;
		}

		// if we have changed any settings since the last save then we better warn the user
		if (GameManager.Instance.UpdateSinceLastSave) {
			if (characterInView != null) {
				// make sure character does not look like it is standing over top of the save dialog
				Destroy ( characterInView );
			}
			GameManager.SaveGameStateDialogInScene.ShowDialog ();
		} else {
			ExitMenu (false);
		}

	}

	public void ExitMenu (bool saveSettings) {

		if (saveSettings) {
			SaveMenuSettings ();
		} else {
			if (GameManager.Instance.UpdateSinceLastSave) {
				DiscardMenuSettings ();
			}
		}
		GameManager.Instance.RemoveFromMenuBreadCrumb ();
		MoveMenuOut (ChangeScene);

	}

	private void MoveMenuOut (bool changeScene) {		

		MainContentRegion.transform.DOKill (false);
		Tween navTween = MainContentRegion.transform.DOScaleX (0.0f, 1.0f).SetEase(Ease.InExpo);
		// if character is already in the view then lets remove it
		if (characterInView != null) {
			Destroy (characterInView);
		}

		if (changeScene) {		
			// go back to the previous scene
			GameManager.Instance.RemoveFromSceneBreadCrumb (); 
			navTween.OnComplete (()=>GameManager.Instance.LoadNextScene(true));
		} else {
			GameManager.Instance.MoveInNextMenu (navTween);
		}

	}

	private void MoveInCharacter (string gameCharacterId) {
	
		Debug.Log ("Adding a new Character to the Character Selection Scene...");

		// if character is already in the view then lets remove it
		if (characterInView != null) {
			Destroy (characterInView);
		}

		// update primary character
		Debug.Log ("Updating Primary Character...");
		PlayerState.Current.SelectedCharacterId = gameCharacterId;

		// add character to scene from prefab
		string fileLocation = "LittleHeros/LittleHero";
		Debug.Log ("Instantiating New Hero...");
		characterInView = Instantiate(Resources.Load(fileLocation, typeof(GameObject))) as GameObject;
	
	}

	private void SwapCharacters (string gameCharacterId, Tween charScaleTween) {
	
		Debug.Log ("Swap Characters..."+gameCharacterId);
		if (characterInView != null && charScaleTween != null) {
			Debug.Log ("Waiting for Character to Scale before Moving new one in...");
			charScaleTween.OnComplete (() => MoveInCharacter (gameCharacterId));
		} else {
			MoveInCharacter (gameCharacterId);
		}
	
	}

	private void SaveMenuSettings () {

		// save the uber gamestate object
		GameManager.Instance.SaveGameState();

	}

	private void DiscardMenuSettings () {

	}

	public void AnimateSelectedTab() {

		if (characterInView != null) {
			Debug.Log ("Scaling the old character down prior to bringing on a new one (DONT SCALE ALL THE WAY DOWN => NAN)...");
			Sequence seq = DOTween.Sequence();
			seq.Append (characterInView.transform.DOScale (new Vector3 (0.2f, 0.2f, 0.2f), 0.2f));
			seq.Append (characterInView.transform.DOMoveY (100.0f, 0.2f, true));
		}

		Debug.Log ("Punching TabPanel for Effect...");
		if (CharacterSelectionTabPanel.SelectedTab.Id == "SisterOneTab" && tabPanelLoaded) {

			Debug.Log ("Pulling up Character One...");
			SisterOneTabRegion.transform.DOKill (false);
			Tweener scaleTween = SisterOneTabRegion.transform.DOPunchScale (new Vector3 (0.2f, 0.2f, 0.2f), 0.5f, 9, 1.0f);
			scaleTween.SetDelay (0.0f);
			scaleTween.OnComplete (() => MoveInCharacter (CharacterOne.ID));
		
		} else if (CharacterSelectionTabPanel.SelectedTab.Id == "SisterTwoTab" && tabPanelLoaded) {
		
			Debug.Log ("Pulling up Character Two...");
			SisterOneTabRegion.transform.DOKill (false);
			Tweener scaleTween = SisterTwoTabRegion.transform.DOPunchScale (new Vector3 (0.2f, 0.2f, 0.2f), 0.5f, 9, 1.0f);
			scaleTween.SetDelay (0.0f);
			scaleTween.OnComplete (() => MoveInCharacter (CharacterTwo.ID));
		
		} else if (CharacterSelectionTabPanel.SelectedTab.Id == "SisterThreeTab" && tabPanelLoaded) {
		
			Debug.Log ("Pulling up Character Three...");
			SisterOneTabRegion.transform.DOKill (false);
			Tweener scaleTween = SisterThreeTabRegion.transform.DOPunchScale (new Vector3 (0.2f, 0.2f, 0.2f), 0.5f, 9, 1.0f);
			scaleTween.SetDelay (0.0f);
			scaleTween.OnComplete (() => MoveInCharacter (CharacterThree.ID));
		
		}

	}

	IEnumerator BlinkSisterOnesEyes (float waitToBlink) {

		// wait waitToBlink seconds before closing the eyes
		yield return new WaitForSeconds(waitToBlink);

		// blink sister one's eyes
		SisterOneFaceClosed.IsVisible.Value = true;

		// wait before opening eyes again
		yield return new WaitForSeconds(0.2f);

		SisterOneFaceClosed.IsVisible.Value = false;

		SisterOneBlinkingEyes = false;

	}

	IEnumerator BlinkSisterTwosEyes (float waitToBlink) {

		// wait waitToBlink seconds before closing the eyes
		yield return new WaitForSeconds(waitToBlink);

		// blink sister one's eyes
		SisterTwoFaceClosed.IsVisible.Value = true;

		// wait before opening eyes again
		yield return new WaitForSeconds(0.2f);

		SisterTwoFaceClosed.IsVisible.Value = false;

		SisterTwoBlinkingEyes = false;

	}

	IEnumerator BlinkSisterThreesEyes (float waitToBlink) {

		// wait waitToBlink seconds before closing the eyes
		yield return new WaitForSeconds(waitToBlink);

		// blink sister one's eyes
		SisterThreeFaceClosed.IsVisible.Value = true;

		// wait before opening eyes again
		yield return new WaitForSeconds(0.2f);

		SisterThreeFaceClosed.IsVisible.Value = false;

		SisterThreeBlinkingEyes = false;

	}

	private int GetTabIndexFromName (string characterId) {

		if (characterId == CharacterOne.ID) {
			return 0;
		} 
		if (characterId == CharacterTwo.ID) {
			return 1;
		} 
		if (characterId == CharacterThree.ID) {
			return 2;
		} 

		return 0;

	}


}

