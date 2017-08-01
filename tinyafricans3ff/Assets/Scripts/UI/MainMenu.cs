using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenu : UIView {

	public float MainMenuButtonGroupTopOffset = 250;
	public float PlayButtonRightOffset = 400;
	public float OptionsButtonLeftOffset = 400;

	public Button PlayButton;
	public Button OptionsButton;

	public static Object AyoObj;

	// whether or not exiting this view results in a scene change
	private bool ChangeScene = false;

	public void Awake() {
		GameState.Instance.ScreenWidth = Screen.width;
		GameState.Instance.ScreenHeight = Screen.height;
	}

	public void Start() {

		GameState.Instance.CurrentMenu = (string)GameManager.Instance.GetLastMenuInBreadCrumb ();
		GameState.Instance.CurrentScene = (string)GameManager.Instance.GetLastSceneInBreadCrumb ();

		Debug.Log ("Current Scene is now: "+ GameState.Instance.CurrentScene );
		Debug.Log ("Current Menu is now: " + GameState.Instance.CurrentMenu);
		if (GameState.Instance.CurrentMenu == GameState.Constants.MAIN_MENU) {
			MoveMenuIn ();
		} else if (GameState.Instance.CurrentMenu == GameState.Constants.OPTIONS_MENU) {
			GameManager.OptionsMenuInScene.MoveMenuIn ();
		}

	}

	public void Play()
	{
		GameManager.Instance.AddToSceneBreadCrumb (GameState.Constants.SCENE_LEVEL_MENU, GameState.Constants.LEVEL_MENU_MENU);
		MoveMenuOut (true);
	}

	public void Options()
	{
		GameManager.Instance.AddToMenuBreadCrumb (GameState.Constants.OPTIONS_MENU);
		MoveMenuOut (false);
	}

	public void LoadMenu () {
		LoadMenuSettings ();
		MoveMenuIn ();
	}

	public void Exit (Button btn) {

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

	private void LoadMenuSettings () {
	
		// there will probably never be any menu settings for the main menu....
		// leaving this here for consistancy, and because...  you never know...  ;-)
	
	}

	private void SaveMenuSettings () {
		// there will probably never be any menu settings for the main menu....
		// leaving this here for consistancy, and because...  you never know...  ;-)
	}

	private void DiscardMenuSettings () {
		// there will probably never be any menu settings for the main menu....
		// leaving this here for consistancy, and because...  you never know...  ;-)
	}

	public void MoveMenuIn () {

		GameManager.GameTitleInScene.TitleMoveIn ();

		Vector3 newLoc;
		newLoc = new Vector3(
			PlayButton.transform.position.x + PlayButton.Offset.Value.Right.Value,
			PlayButton.transform.position.y,
			0.0f
		);
		PlayButton.transform.DOMove(newLoc,0.4f,true).SetDelay(2.0f).SetEase(Ease.Linear, 3.0f, 1.0f).OnComplete(LoopPlayButton);

		newLoc = new Vector3(
			OptionsButton.transform.position.x - OptionsButton.Offset.Value.Left.Value,
			OptionsButton.transform.position.y,
			0.0f
		);
		OptionsButton.transform.DOMove(newLoc,0.4f,true).SetDelay(2.0f).SetEase(Ease.Linear, 3.0f, 1.0f);

	}

	public void MoveMenuOut (bool changeScene) {

		GameManager.GameTitleInScene.TitleMoveOut ();

		Vector3 newLoc;
		newLoc = new Vector3(
			PlayButton.transform.position.x - PlayButtonRightOffset,
			PlayButton.transform.position.y,
			0.0f
		);

		Tween tween = PlayButton.transform.DOMove (newLoc, 0.5f, true);
		tween.SetDelay (0.0f).SetEase (Ease.Linear, 3.0f, 1.0f);

		// TODO: handle scene change here
		if (changeScene) {
			tween.OnComplete (() => GameManager.Instance.LoadNextScene (false));
		} else {
			GameManager.Instance.MoveInNextMenu (tween);
		}

		newLoc = new Vector3(
			OptionsButton.transform.position.x + OptionsButtonLeftOffset,
			OptionsButton.transform.position.y,
			0.0f
		);
		OptionsButton.transform.DOMove(newLoc,0.5f,true).SetDelay(0.0f).SetEase(Ease.Linear, 3.0f, 1.0f);

	}

	void LoopPlayButton() {

		PlayButton.transform.DOScale(new Vector3(1.1f,1.1f,1.0f), 0.8f).SetLoops(-1, LoopType.Yoyo);

	}

}
