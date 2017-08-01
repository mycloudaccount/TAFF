using UnityEngine;
using System.Collections;
using Facebook.Unity;
using MarkLight.Views.UI;
using MarkLight.Views;
using DG.Tweening;

public class SplashMenu : UIView {

	public Mask SkyMask;

	public Label CameroonLabel;
	public Label GhanaLabel;
	public Label NigeriaLabel;

	public Image CountryNameMarkerOneLeft;
	public Image CountryNameMarkerOneRight;
	public Image CountryNameMarkerTwoLeft;
	public Image CountryNameMarkerTwoRight;
	public Image CountryNameMarkerThreeLeft;
	public Image CountryNameMarkerThreeRight;

	private Tween africansLogoTween;
	public Tween copyrightTween;
	public Tween closingPanelTween;
	public Tween loadingMessageTween;

	void Awake () {
	}

	// Use this for initialization
	void Start () {
		InitializeView ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void InitializeView () {
		africansLogoTween = DOTween.TweensById ("AfricansLogoTween", false)[0];
		if (africansLogoTween != null) {
			africansLogoTween.OnComplete (InitializeLogin);
		}
		StartCoroutine(AnimateSlowerLoadingObject());
	}

	IEnumerator AnimateSlowerLoadingObject () {
		
		yield return new WaitForSeconds (1);
		AnimateCountryName (
			GhanaLabel,
			CountryNameMarkerOneRight.transform.position,
			CountryNameMarkerOneLeft.transform.position,
			1.5f
		);
		AnimateCountryName (
			NigeriaLabel,
			CountryNameMarkerTwoRight.transform.position,
			CountryNameMarkerTwoLeft.transform.position,
			1.4f
		);
		AnimateCountryName (
			CameroonLabel,
			CountryNameMarkerThreeRight.transform.position,
			CountryNameMarkerThreeLeft.transform.position,
			1.3f
		);
			
	}
		
	private void AnimateCountryName (Label cName, Vector3 initPos, Vector3 endPos, float duration) {
		
		cName.transform.position = initPos;
		Tween tween = cName.transform.DOMove (endPos, duration, true);
		tween.SetEase (Ease.Linear);
		tween.SetLoops (-1);

	}

	private void InitializeLogin () {

		copyrightTween = DOTween.TweensById ("CopyrightTween", false)[0];
		closingPanelTween = DOTween.TweensById ("ClosingPanelTween", false)[0];
		//loadingMessageTween = DOTween.TweensById ("LoadingMessageTween", false)[0];

		// show copy right after a bit of a delay
		Sequence cleanupSequence = DOTween.Sequence ();
		cleanupSequence.Append (copyrightTween);
		cleanupSequence.Append (closingPanelTween);
		//cleanupSequence.Append (loadingMessageTween);
		cleanupSequence.OnComplete (VerifyFBAuthentication);

	}

	// verify the user is logged in
	private void VerifyFBAuthentication () {

		// lets get the splash items off the screen
		SkyMask.Deactivate();
		GameObject logo = GameObject.Find ("TitleCanvas");
		Destroy (logo);

		if (!FB.IsLoggedIn) {
			FB.Init (CheckLoggedIn, OnHideUnity);
		} else {
			// TODO: Add the story scene and bulliten message scene (info message is something I might need all users to be aware ASAP) 
			//       to the list of possible sceens shown right after splash
			GameManager.Instance.LoadNextScene(false);
		}

	}

	private void CheckLoggedIn () {

		if (FB.IsLoggedIn) {
			Debug.Log ("FB is logged in");
		} else {

			Debug.Log ("FB is not logged in");

			// show login dialog
			GameManager.LoginDialogInScene.ShowDialog ();

		}

	}

	private void OnHideUnity (bool isGameShown) {

		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

	}

}

