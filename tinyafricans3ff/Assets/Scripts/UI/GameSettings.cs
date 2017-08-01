using MarkLight.Views.UI;
using UnityEngine;
using MarkLight.Views;
using DG.Tweening;
using MarkLight;
using System.Collections;

public class GameSettings : UIView
{

	public Image MainBackgroundImage;
	public ViewAnimation MainBackgroundImageInAnimation;
	public ViewAnimation MainBackgroundImageOutAnimation;
	public ViewAnimation MainContentRegionInAnimation;
	public ViewAnimation MainContentRegionOutAnimation;
	public Region MainContentRegion;
	public Region MainRegionMount;
	public Region MainRegion;

	public ElementSize OptionsWidth;
	public ElementSize OptionsHeight;
	public Material SunRaysMaterialTypeA;
	public Image SunRaysTypeA00;

	public TabPanel GameSettingsTabPanel;

	public Region SoundTabRegion;
	public Region DisplayTabRegion;

	public TabHeader SoundTabHeader;
	public TabHeader DisplayTabHeader;
	private bool SoundTabPanelLoaded = false;
	private bool DisplayTabPanelLoaded = false;

	public Image CogMount0Image;
	public Image CogMount1Image;
	public Image CogMount2Image;
	public Image CogMount3Image;
	public Image CogTypeAImageBkg;
	public Image CogTypeAImage0;
	public Image CogTypeAImage1;
	public Image CogTypeAImage2;
	public Image CogTypeAImage3;

	public Image CogTypeAImage4;
	public Image CogTypeAImage5;
	public Image CogTypeAImage6;
	public Image CogTypeAImage7;

	public _float MusicVolume;
	public Label MusicMuteLabel;
	public Button MusicMuteButton;
	public _float EffectsVolume;
	public Label EffectsMuteLabel;
	public Button EffectsMuteButton;

	// whether or not exiting this view results in a scene change
	private bool ChangeScene = false;

	void Awake(){

	}

	void Start () {
	}

	public override void Initialize () {
		base.Initialize ();
		OptionsWidth = new ElementSize(400, ElementSizeUnit.Pixels);
		OptionsHeight = new ElementSize(350, ElementSizeUnit.Pixels);

		SoundTabHeader.Scale.Value = new Vector3 (0.0f,0.0f,0.0f);
		DisplayTabHeader.Scale.Value = new Vector3 (0.0f,0.0f,0.0f);

		SunRaysMaterialTypeA = Resources.Load("Particles/Materials/Light_02", typeof(Material)) as Material;
		SunRaysMaterialTypeA.SetColor ("_TintColor", Color.magenta);
	}

	private void InitializeView () {
		
		MainBackgroundImage.Alpha.Value = 1.0f;
		MainContentRegion.Alpha.Value = 0.0f;

		// work around - I need to wait on some position values to get set
		// accurately
		StartCoroutine(AnimateSlowerLoadingObject());

		StartSunRayAnimations ();

		StartTabHeaderAnimation ();
	
		// LEAVE ABOVE ALONE


	}

	IEnumerator AnimateSlowerLoadingObject () {
		yield return new WaitForSeconds(1);
		StartHairCogAnimation ();
		StartClothesCogAnimation ();
	}

	public void LoadMenu () {
		LoadMenuSettings ();
		MoveMenuIn ();
	}

	private void LoadMenuSettings () {

		Debug.Log ("GameState.GameSettings.EffectsMuted: "+GameState.Instance.GameSettings.EffectsMuted);
		Debug.Log ("GameState.GameSettings.MusicMuted: "+GameState.Instance.GameSettings.MusicMuted);
		Debug.Log ("GameState.GameSettings.EffectsVolume: "+GameState.Instance.GameSettings.EffectsVolume);
		Debug.Log ("GameState.GameSettings.MusicVolume: "+GameState.Instance.GameSettings.MusicVolume);

		// load the initial sound settings
		EffectsMuteButton.ToggleValue.Value = GameState.Instance.GameSettings.EffectsMuted;
		UpdateMuteButtonState (EffectsMuteButton);
		MusicMuteButton.ToggleValue.Value = GameState.Instance.GameSettings.MusicMuted;
		UpdateMuteButtonState (MusicMuteButton);

		SetValue (() => EffectsVolume, GameState.Instance.GameSettings.EffectsVolume);
		SetValue (() => MusicVolume, GameState.Instance.GameSettings.MusicVolume);

	}

	private void SaveMenuSettings () {
		// save the settings
		GameState.Instance.GameSettings.EffectsMuted = EffectsMuteButton.ToggleValue.Value;
		GameState.Instance.GameSettings.MusicMuted = MusicMuteButton.ToggleValue.Value;
		GameState.Instance.GameSettings.MusicVolume = MusicVolume.Value;
		GameState.Instance.GameSettings.EffectsVolume = EffectsVolume.Value;

		// save the uber gamestate object
		GameManager.Instance.SaveGameState();
	}

	private void DiscardMenuSettings () {
		// load the settings
		EffectsMuteButton.ToggleValue.Value = GameState.Instance.GameSettings.EffectsMuted;
		MusicMuteButton.ToggleValue.Value = GameState.Instance.GameSettings.MusicMuted;
		MusicVolume.Value = GameState.Instance.GameSettings.MusicVolume;
		EffectsVolume.Value = GameState.Instance.GameSettings.EffectsVolume;

		GameManager.Instance.UpdateSinceLastSave = false;
	}

	private void MoveMenuIn () {
		MoveGameSettingsRegionIn ();
		MoveBackgroundIn ();
		MoveMainContentIn ();

		InitializeView ();
	}

	private void MoveMenuOut (bool changeScene) {
		SoundTabHeader.Scale.Value = new Vector3 (0.0f,0.0f,0.0f);
		DisplayTabHeader.Scale.Value = new Vector3 (0.0f,0.0f,0.0f);

		MusicMuteButton.transform.DOKill ();
		MusicMuteButton.Scale.Value = new Vector3 (0.8f, 0.8f, 1.0f);
		EffectsMuteButton.transform.DOKill ();
		EffectsMuteButton.Scale.Value = new Vector3 (0.8f, 0.8f, 1.0f);

		MoveGameSettingsRegionOut (changeScene);
		MoveBackgroundOut ();
		MoveMainContentOut ();
	}

	private void MoveGameSettingsRegionIn () {
		MainRegion.transform.position = MainRegionMount.transform.position;
	}

	private void MoveGameSettingsRegionOut (bool changeScene) {
		Tween tween = MainRegion.transform.DOMoveY (-480, 0.5f, true);
		tween.SetDelay (1.0f);
		if (changeScene) {
			tween.OnComplete (() => GameManager.Instance.LoadNextScene (false));
		} else {
			GameManager.Instance.MoveInNextMenu (tween);
		}
	}

	private void MoveBackgroundIn () {
		MainBackgroundImageInAnimation.StartAnimation ();
	}

	private void MoveBackgroundOut () {
		MainBackgroundImageOutAnimation.StartAnimation ();
	}

	private void MoveMainContentIn () {
		MainContentRegionInAnimation.StartAnimation ();
	}

	private void MoveMainContentOut () {
		MainContentRegionOutAnimation.StartAnimation ();
	}

	public void Exit (Button btn) {

		// mute setting has changed
		if (EffectsMuteButton.ToggleValue.Value != GameState.Instance.GameSettings.EffectsMuted) {
			GameManager.Instance.UpdateSinceLastSave = true;
		} else if (MusicMuteButton.ToggleValue.Value != GameState.Instance.GameSettings.MusicMuted) {
			GameManager.Instance.UpdateSinceLastSave = true;
		} else if (MusicVolume.Value != GameState.Instance.GameSettings.MusicVolume) {
			GameManager.Instance.UpdateSinceLastSave = true;
		} else if (EffectsVolume.Value != GameState.Instance.GameSettings.EffectsVolume) {
			GameManager.Instance.UpdateSinceLastSave = true;
		}

		// if we have changed any settings since the last save then we better warn the user
		if (GameManager.Instance.UpdateSinceLastSave) {
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

	public void ToggleMute (Button btn) {
		btn.ToggleValue.Value = !btn.ToggleValue.Value;
		UpdateMuteButtonState (btn);
	}

	private void UpdateMuteButtonState (Button btn) {
		if (btn.ToggleValue.Value) {
			StartMuteButtonAnimation(btn);
			UpdateMuteLabelState (btn, true);
		} else {
			StopMuteButtonAnimation(btn);
			UpdateMuteLabelState (btn, false);
		}
	}

	private void UpdateMuteLabelState (Button btn, bool isVisible) {
		if (btn.Id == "MusicMuteButton") {
			MusicMuteLabel.IsVisible.Value = isVisible;
		} else if (btn.Id == "EffectsMuteButton") {
			EffectsMuteLabel.IsVisible.Value = isVisible;
		}
	}

	private void StartMuteButtonAnimation (Button btn) {
		Tween tween = btn.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.5f);
		tween.SetLoops (-1, LoopType.Restart);
	}

	private void StopMuteButtonAnimation (Button btn) {
		btn.transform.DOKill (false);
		btn.transform.DOScale (new Vector3 (0.8f,0.8f,1.0f), 0.3f);
	}

	// COG AND TAB BACKGROUND ANIMATION CRAP ///////////////////////////////////////////////////////
	// DONT PLACE ANYTHING BELOW HERE //////////////////////////////////////////////////////////////

	public void StartSunRayAnimations() {

		SunRaysMaterialTypeA.DOColor(Color.cyan, "_TintColor", 5).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		SunRaysTypeA00.transform.DORotate (new Vector3 (0.0f, 0.0f, 180.0f), 5, RotateMode.LocalAxisAdd).SetEase (Ease.Linear).SetLoops (-1);

	}

	public void AnimateSelectedTab() {

		if (GameSettingsTabPanel.SelectedTab.Id == "SoundTab" && SoundTabPanelLoaded) {
			RestartHairCogAnimation ();
		} else if (GameSettingsTabPanel.SelectedTab.Id == "DisplayTab" && DisplayTabPanelLoaded) {
			RestartClothesCogAnimation ();
		}

	}

	public void StartTabHeaderAnimation() {

		StartHairTabHeaderAnimation ();

	}
	private void StartHairTabHeaderAnimation() {

		Tweener tweener = SoundTabHeader.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.5f);
		tweener.SetDelay (1.0f);
		tweener.SetEase (Ease.OutBounce);
		tweener.OnComplete (StartClothesTabHeaderAnimation);

	}
	private void StartClothesTabHeaderAnimation() {

		Tweener tweener = DisplayTabHeader.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.5f);
		tweener.SetEase (Ease.OutBounce);

	}

	public void StartHairCogAnimation() {

		CogTypeAImage0.transform.DOKill (false);
		CogTypeAImage1.transform.DOKill (false);
		CogTypeAImage2.transform.DOKill (false);
		CogTypeAImage3.transform.DOKill (false);

		CogTypeAImage0.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage1.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage2.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage3.transform.position = CogTypeAImageBkg.transform.position;

		AnimateCog (
			CogTypeAImage0, 
			CogMount0Image.transform.position, 
			new Vector3 (0.5f, 0.5f, 1), 
			90.0f,
			0.8f, 
			0.0f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage1, 
			CogMount1Image.transform.position, 
			new Vector3 (1.2f, 1.2f, 1), 
			-180.0f,
			1.0f, 
			0.5f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage2, 
			CogMount2Image.transform.position, 
			new Vector3 (0.8f, 0.8f, 1), 
			-180.0f,
			1.5f, 
			0.8f,
			4.0f
		);

		AnimateCog (
			CogTypeAImage3, 
			CogMount3Image.transform.position, 
			new Vector3 (1.0f, 1.0f, 1), 
			-270.0f,
			1.5f, 
			1.8f,
			3.0f
		);

		SoundTabPanelLoaded = true;

	}

	public void StartClothesCogAnimation() {

		CogTypeAImage4.transform.DOKill (false);
		CogTypeAImage5.transform.DOKill (false);
		CogTypeAImage6.transform.DOKill (false);
		CogTypeAImage7.transform.DOKill (false);

		CogTypeAImage4.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage5.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage6.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage7.transform.position = CogTypeAImageBkg.transform.position;

		AnimateCog (
			CogTypeAImage4, 
			CogMount0Image.transform.position, 
			new Vector3 (0.5f, 0.5f, 1), 
			90.0f,
			0.8f, 
			0.0f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage5, 
			CogMount1Image.transform.position, 
			new Vector3 (1.2f, 1.2f, 1), 
			-180.0f,
			1.0f, 
			0.5f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage6, 
			CogMount2Image.transform.position, 
			new Vector3 (0.8f, 0.8f, 1), 
			-180.0f,
			1.5f, 
			0.8f,
			4.0f
		);

		AnimateCog (
			CogTypeAImage7, 
			CogMount3Image.transform.position, 
			new Vector3 (1.0f, 1.0f, 1), 
			-270.0f,
			1.5f, 
			1.8f,
			3.0f
		);

		DisplayTabPanelLoaded = true;

	}

	private void AnimateCog(Image cog, Vector3 newPosition, Vector3 newScale, float deltaRotation, float animationDuration, float animationDelay, float loopDuration) {

		Tweener scaleTween = cog.transform.DOScale (newScale, animationDuration);
		scaleTween.SetDelay (animationDelay);
		scaleTween.SetEase (Ease.OutBounce);

		Vector3 newLoc = newPosition;
		Tweener moveTween = cog.transform.DOMove (newLoc, animationDuration, true);
		moveTween.SetDelay (animationDelay);
		moveTween.SetEase (Ease.OutBounce);
		moveTween.OnComplete(()=>LoopCogAnimation(cog, deltaRotation, loopDuration));

	}

	private Vector3 getNewPosition(Vector3 oldPosition, Vector3 deltaPosition) {

		Vector3 newPosition = new Vector3(
			oldPosition.x + deltaPosition.x,
			oldPosition.y + deltaPosition.y,
			oldPosition.z + deltaPosition.z
		);

		return newPosition;

	}

	public void LoopCogAnimation(Image cog, float deltaRotation, float loopDuration) {

		cog.transform.DORotate (new Vector3 (0, 0, deltaRotation), loopDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);

	}

	public void RestartHairCogAnimation() {

		Tweener sunRayBeforeTween = SunRaysTypeA00.transform.DOScale (new Vector3(0.0f,0.0f,0.0f), 0.2f).SetEase(Ease.Linear);
		sunRayBeforeTween.OnComplete (StartHairCogAnimation);

		SoundTabRegion.transform.DOKill (false);
		Tweener scaleTween = SoundTabRegion.transform.DOPunchScale (new Vector3(0.2f,0.2f,0.2f), 0.5f, 9, 1.0f);
		scaleTween.SetDelay (0.0f);

		Tweener sunRayAfterTween = SunRaysTypeA00.transform.DOScale (new Vector3(1.0f,1.0f,1.0f), 0.8f).SetEase(Ease.Linear);
		sunRayAfterTween.SetDelay (0.5f);

	}

	public void RestartClothesCogAnimation() {

		Tweener sunRayBeforeTween = SunRaysTypeA00.transform.DOScale (new Vector3(0.0f,0.0f,0.0f), 0.2f).SetEase(Ease.Linear);
		sunRayBeforeTween.OnComplete (StartClothesCogAnimation);

		DisplayTabRegion.transform.DOKill (false);
		Tweener scaleTween = DisplayTabRegion.transform.DOPunchScale (new Vector3(0.2f,0.2f,0.2f), 0.5f, 9, 1.0f);
		scaleTween.SetDelay (0.0f);

		Tweener sunRayAfterTween = SunRaysTypeA00.transform.DOScale (new Vector3(1.0f,1.0f,1.0f), 0.8f).SetEase(Ease.Linear);
		sunRayAfterTween.SetDelay (0.5f);

	}


}
