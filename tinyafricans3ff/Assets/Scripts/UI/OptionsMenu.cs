using MarkLight.Views.UI;
using UnityEngine;
using MarkLight.Views;
using DG.Tweening;
using MarkLight;

public class OptionsMenu : UIView
{

	public Image OptionsBackgroundImage;
	public ViewAnimation OptionsBackgroundImageInAnimation;
	public ViewAnimation OptionsBackgroundImageOutAnimation;
	public Group OptionsMenuButtonGroup;
	public ViewAnimation OptionsMenuButtonGroupInAnimation;
	public ViewAnimation OptionsMenuButtonGroupOutAnimation;
	public Region OptionsRegionMount;
	public Region OptionsRegion;

	public float GoButtonWidth = 70;
	public float GoButtonHeight = 65;
	public _ElementSize OptionLabelRegionHeight = new _ElementSize{Value=new ElementSize{Value=65}};

	void Awake(){
		
	}

	void Start () {
		InitializeView ();
	}

	public override void Initialize () {
	}

	private void InitializeView () {
		OptionsBackgroundImage.Alpha.Value = 1.0f;
		OptionsMenuButtonGroup.Alpha.Value = 0.0f;
	}

	public void LoadMenu () {
		MoveMenuIn ();
	}

	public void MoveMenuIn () {
		MoveOptionsRegionIn ();
		MoveBackgroundIn ();
		MoveOptionsButtonsIn ();
	}

	public void MoveMenuOut (bool changeScene) {
		MoveOptionsRegionOut (changeScene);
		MoveBackgroundOut ();
		MoveOptionsButtonsOut ();
	}

	private void MoveOptionsRegionIn () {
		OptionsRegion.transform.position = OptionsRegionMount.transform.position;
	}

	private void MoveOptionsRegionOut (bool changeScene) {
		Tween tween = OptionsRegion.transform.DOMoveY (-480, 0.5f, true);
		tween.SetDelay (1.0f);
		if (changeScene) {
			tween.OnComplete (() => GameManager.Instance.LoadNextScene (false));
		} else {
			GameManager.Instance.MoveInNextMenu (tween);
		}
	}

	private void MoveBackgroundIn () {
		OptionsBackgroundImageInAnimation.StartAnimation ();
	}

	private void MoveBackgroundOut () {
		OptionsBackgroundImageOutAnimation.StartAnimation ();
	}

	private void MoveOptionsButtonsIn () {
		OptionsMenuButtonGroupInAnimation.StartAnimation ();
	}

	private void MoveOptionsButtonsOut () {
		OptionsMenuButtonGroupOutAnimation.StartAnimation ();
	}

	public void CustomizeCharacter (Button btn) {
		GameManager.Instance.AddToSceneBreadCrumb (GameState.Constants.SCENE_CHARACTER_CUSTOMIZATION, GameState.Constants.CHARACTER_CUSTOMIZATION_MENU);
		MoveMenuOut (true);
	}

	public void GameSettings (Button btn) {
		GameManager.Instance.AddToMenuBreadCrumb (GameState.Constants.GAME_SETTINGS_MENU);
		MoveMenuOut (false);
	}

	public void ExitOptions (Button btn) {
		GameManager.Instance.RemoveFromMenuBreadCrumb ();
		MoveMenuOut (false);
	}

	public void SelectCharacter (Button btn) {
		GameManager.Instance.AddToSceneBreadCrumb (GameState.Constants.SCENE_CHARACTER_SELECTION, GameState.Constants.CHARACTER_SELECTION_MENU);
		MoveMenuOut (true);
	}

}