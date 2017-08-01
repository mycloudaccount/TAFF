using MarkLight.Views.UI;
using UnityEngine;
using MarkLight.Views;
using DG.Tweening;
using MarkLight;

public class TemplateMenu : UIView
{

	public Image MainBackgroundImage;
	public ViewAnimation MainBackgroundImageInAnimation;
	public ViewAnimation MainBackgroundImageOutAnimation;
	public ViewAnimation MainContentRegionInAnimation;
	public ViewAnimation MainContentRegionOutAnimation;
	public Region MainContentRegion;
	public Region MainRegionMount;
	public Region MainRegion;

	void Awake(){

	}

	void Start () {
		InitializeView ();
	}

	public override void Initialize () {
	}

	private void InitializeView () {
		MainBackgroundImage.Alpha.Value = 1.0f;
		MainContentRegion.Alpha.Value = 0.0f;
	}

	public void MoveMenuIn () {
		MoveMainRegionIn ();
		MoveBackgroundIn ();
		MoveMainContentIn ();
	}

	public void MoveMenuOut (bool changeScene) {
		MoveMainRegionOut (changeScene);
		MoveBackgroundOut ();
		MoveMainContentOut ();
	}

	private void MoveMainRegionIn () {
		MainRegion.transform.position = MainRegionMount.transform.position;
	}

	private void MoveMainRegionOut (bool changeScene) {
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
		GameManager.Instance.RemoveFromMenuBreadCrumb ();
		MoveMenuOut (false);
	}

}
