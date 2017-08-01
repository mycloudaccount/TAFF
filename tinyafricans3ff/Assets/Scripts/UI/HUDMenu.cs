using UnityEngine;
using System.Collections;
using MarkLight;
using MarkLight.Views.UI;
using DG.Tweening;
using System.Collections.Generic;

public class HUDMenu : UIView
{

	public Region MainContentRegion;
	public Region MainContentRegionMount;

	private bool ChangeScene = true;

	void Awake(){
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

		// set the level boss according to the current level
		GameState.Instance.SelectedEnemyId = EnemyOne.ID;

		Canvas canvas = gameObject.transform.parent.GetComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;

	}

	void InitializeView () {

		// I need to wait on some position values to get set
		// accurately
		StartCoroutine(AnimateSlowerLoadingObject());

	}

	IEnumerator AnimateSlowerLoadingObject () {

		yield return new WaitForSeconds(1);

	}

	public void ExitMenu (bool saveSettings) {

		if (saveSettings) {
			//SaveMenuSettings ();
		} else {
			if (GameManager.Instance.UpdateSinceLastSave) {
				//DiscardMenuSettings ();
			}
		}
		GameManager.Instance.AddToSceneBreadCrumb ("Challenge"+PlayerState.Current.SelectedLevel.LevelId, GameState.Constants.CHALLENGE_MENU);
		MoveMenuOut (ChangeScene);

	}

	private void MoveMenuOut (bool changeScene) {		

		MainContentRegion.transform.DOKill (false);
		Tween navTween = MainContentRegion.transform.DOScaleX (0.0f, 1.0f).SetEase(Ease.InExpo);

		if (changeScene) {		
			navTween.OnComplete (()=>LoadNextScene());
		} else {
			GameManager.Instance.MoveInNextMenu (navTween);
		}

	}

	public void LoadNextScene () {

		Debug.Log ("Done moving nav region out.  Let's move onto the next scene!");
		GameManager.Instance.LoadNextScene (true);

	}

}




