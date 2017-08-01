using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class LoadingSceneManager : MonoBehaviour {

	private const string CIRCLE_ONE_ID = "CircleOne";
	private const string CIRCLE_TWO_ID = "CircleTwo";
	private const string CIRCLE_THREE_ID = "CircleThree";
	private const string CIRCLE_FOUR_ID = "CircleFour";
	private const string CIRCLE_FIVE_ID = "CircleFive";

	// indicates that the parent of this manager is actually a splash scene
	// the only object in a splash scene is going to be the LoadSceneManager Object
	public bool InSplashScene = false;

	private int LoadStatus = 0;
	private Text LoadingText;

	public Camera LoadingSceneCamera;
	public Canvas LoadingSceneCanvas;
	public GameObject QuickLoadImage;
	public GameObject LoadingTextGameObject;
	public GameObject StatusBar;

	private Tween LoadingTween;
	private Tween LoadingBallOneTween;
	private Tween LoadingBallTwoTween;
	private Tween LoadingBallThreeTween;
	private List<Tween> loadingBallTweens;

	private bool IsCircleAnimationComplete = false;

	private List<Tween> circleFrameTweens;
	private List<Tween> loadingTweens;
	private List<Tween> treeLineTweens;
	private float CircleDuration = 3.0f;

	private Tween TreeLineTween;

	private GameObject CircleOne;
	private Tween CircleOneTween;
	private Tween CircleOneFrameTween;

	private GameObject CircleTwo;
	private Tween CircleTwoTween;
	private Tween CircleTwoFrameTween;

	private GameObject CircleThree;
	private Tween CircleThreeTween;
	private Tween CircleThreeFrameTween;

	private GameObject CircleFour;
	private Tween CircleFourTween;
	private Tween CircleFourFrameTween;

	private GameObject CircleFive;
	private Tween CircleFiveTween;
	private Tween CircleFiveFrameTween;

	private bool quickLoad;
	public bool QuickLoad {
		get { return quickLoad; }
		set { quickLoad = value; }
	}

	// Use this for initialization
	void Start () {

		LoadStatus = 0;
		LoadingText = LoadingTextGameObject.GetComponent<Text> ();

		loadingTweens = DOTween.TweensById ("Loading", false);
		LoadingTween = loadingTweens [0];

		loadingBallTweens = DOTween.TweensById ("LoadingBallOne", false);
		LoadingBallOneTween = loadingBallTweens [0];
		loadingBallTweens = DOTween.TweensById ("LoadingBallTwo", false);
		LoadingBallTwoTween = loadingBallTweens [0];
		loadingBallTweens = DOTween.TweensById ("LoadingBallThree", false);
		LoadingBallThreeTween = loadingBallTweens [0];

		treeLineTweens = DOTween.TweensById ("TreeLine", false);
		TreeLineTween = treeLineTweens [0];

		CircleOne = GameObject.Find(CIRCLE_ONE_ID);
		CircleOne.SetActive (false);
		CircleOne.transform.localScale = new Vector3 (0.0f,0.0f,0.0f);
		circleFrameTweens = DOTween.TweensById (CIRCLE_ONE_ID + "FrameTween", false);
		CircleOneFrameTween = circleFrameTweens [0];

		CircleTwo = GameObject.Find(CIRCLE_TWO_ID);
		CircleTwo.SetActive (false);
		CircleTwo.transform.localScale = new Vector3 (0.0f,0.0f,0.0f);
		circleFrameTweens = DOTween.TweensById (CIRCLE_TWO_ID + "FrameTween", false);
		CircleTwoFrameTween = circleFrameTweens [0];

		CircleThree = GameObject.Find(CIRCLE_THREE_ID);
		CircleThree.SetActive (false);
		CircleThree.transform.localScale = new Vector3 (0.0f,0.0f,0.0f);
		circleFrameTweens = DOTween.TweensById (CIRCLE_THREE_ID + "FrameTween", false);
		CircleThreeFrameTween = circleFrameTweens [0];

		CircleFour = GameObject.Find(CIRCLE_FOUR_ID);
		CircleFour.SetActive (false);
		CircleFour.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		circleFrameTweens = DOTween.TweensById (CIRCLE_FOUR_ID + "FrameTween", false);
		CircleFourFrameTween = circleFrameTweens [0];

		CircleFive = GameObject.Find(CIRCLE_FIVE_ID);
		CircleFive.SetActive (false);
		CircleFive.transform.localScale = new Vector3 (0.0f,0.0f,0.0f);
		circleFrameTweens = DOTween.TweensById (CIRCLE_FIVE_ID + "FrameTween", false);
		CircleFiveFrameTween = circleFrameTweens [0];

		if (InSplashScene) {

			// TODO: Add the story scene and bulliten message scene (info message is something I might need all users to be aware ASAP) 
			//       to the list of possible sceens shown right after splash
			GameState.Instance.CurrentScene = (string)GameManager.Instance.GetLastSceneInBreadCrumb ();
			Debug.Log ("Loading the following Scene: " + GameState.Instance.CurrentScene);
			LoadNextScene (GameState.Instance.CurrentScene, false);
			
		} else {
			if (LoadingSceneCamera != null) {
				LoadingSceneCamera.enabled = false;
			}
			if (LoadingSceneCanvas != null) {
				LoadingSceneCanvas.enabled = false;
			}
			if (QuickLoadImage != null) {
				QuickLoadImage.SetActive(false);
			}
		}

	}

	// Update is called once per frame
	void Update () {

	}

	public void LoadNextScene (string newScene, bool quickLoad) {

		// update quickload
		QuickLoad = quickLoad;

		if (!QuickLoad) {
			// display the loading message
			LoadingTween.Play ();
			LoadingBallOneTween.Play ();
			LoadingBallTwoTween.Play ();
			LoadingBallThreeTween.Play ();
			TreeLineTween.Play ();
			// setup Circle One's Frame Tween
			StartCircleFrameAnimation(CIRCLE_ONE_ID);
		}

		StartCoroutine (DisplayNextScene(newScene));

	}

	IEnumerator DisplayNextScene (string newScene) {

		if (LoadingSceneCamera != null) {
			LoadingSceneCamera.enabled = true;
		}
		if (LoadingSceneCanvas != null) {
			LoadingSceneCanvas.enabled = true;
		}
		if (QuickLoad && QuickLoadImage != null) {
			QuickLoadImage.SetActive(true);
		}

		LoadingText.text = "PERCENT LOADED " + LoadStatus + "%";
		StatusBar.transform.localScale = new Vector3 (0.0f,1.0f,1.0f);

		AsyncOperation loadScreenAsync = SceneManager.LoadSceneAsync (newScene);
		loadScreenAsync.allowSceneActivation = false;
		while (!loadScreenAsync.isDone) {

			LoadStatus = (int)(loadScreenAsync.progress * 100);
			LoadingText.text = "PERCENT LOADED " + LoadStatus + "%";
			StatusBar.transform.localScale = new Vector3 (loadScreenAsync.progress,1.0f,1.0f);

			// let's allow a minimum amount of time for the scene load to take (this might be useful
			// for example when I am trying to provide a message for the user to read during screen loading)
			if (
				loadScreenAsync.progress == 0.9f 
				&& !GameManager.Instance.LoadingPlayerSettings 
				&& !GameManager.Instance.LoadingWorldData
				//&& !GameManager.Instance.LoadingHeroData
			) {
				LoadStatus = (int)(loadScreenAsync.progress * 100);
				LoadingText.text = "PERCENT LOADED " + "95" + "%";
				StatusBar.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);

				// ok we are done so lets make the circles complete really fast
				CircleDuration = 0.3f;

				// add a wait here
				//yield return new WaitForSeconds(0.1f);
				if (IsCircleAnimationComplete || QuickLoad) {
					loadScreenAsync.allowSceneActivation = true;
				}
			}

			yield return null;

		}

	}

	public void StartCircleFrameAnimation (string circleId) {
	
		switch (circleId) {

		case CIRCLE_ONE_ID:
			CircleOneFrameTween.Play ();
			break;

		case CIRCLE_TWO_ID:
			CircleTwoFrameTween.Play ();
			break;

		case CIRCLE_THREE_ID:
			CircleThreeFrameTween.Play ();
			break;

		case CIRCLE_FOUR_ID:
			CircleFourFrameTween.Play ();
			break;

		case CIRCLE_FIVE_ID:
			CircleFiveFrameTween.Play ();
			break;

		}

	}

	public void StartCircleAnimation (string circleId) {
	
		switch (circleId) {

		case CIRCLE_ONE_ID:
			CircleOne.SetActive (true);
			CircleOneTween = CircleOne.transform.DOScale (new Vector3 (1, 1, 1), CircleDuration);
			CircleOneTween.SetEase (Ease.Linear);
			CircleOneTween.OnComplete (()=>StartCircleFrameAnimation(CIRCLE_TWO_ID));
			CircleOneTween.Play ();
			break;

		case CIRCLE_TWO_ID:
			CircleTwo.SetActive (true);
			CircleTwoTween = CircleTwo.transform.DOScale (new Vector3 (1, 1, 1), CircleDuration);
			CircleTwoTween.SetEase (Ease.Linear);
			CircleTwoTween.OnComplete (()=>StartCircleFrameAnimation(CIRCLE_THREE_ID));
			CircleTwoTween.Play ();
			break;

		case CIRCLE_THREE_ID:
			CircleThree.SetActive (true);
			CircleThreeTween = CircleThree.transform.DOScale (new Vector3 (1, 1, 1), CircleDuration);
			CircleThreeTween.SetEase (Ease.Linear);
			CircleThreeTween.OnComplete (()=>StartCircleFrameAnimation(CIRCLE_FOUR_ID));
			CircleThreeTween.Play ();
			break;

		case CIRCLE_FOUR_ID:
			CircleFour.SetActive (true);
			CircleFourTween = CircleFour.transform.DOScale (new Vector3 (1, 1, 1), CircleDuration);
			CircleFourTween.SetEase (Ease.Linear);
			CircleFourTween.OnComplete (()=>StartCircleFrameAnimation(CIRCLE_FIVE_ID));
			CircleFourTween.Play ();
			break;

		case CIRCLE_FIVE_ID:
			CircleFive.SetActive (true);
			CircleFiveTween = CircleFive.transform.DOScale (new Vector3 (1, 1, 1), CircleDuration);
			CircleFiveTween.SetEase (Ease.Linear);
			CircleFiveTween.OnComplete (SetCircleAnimationComplete);
			CircleFiveTween.Play ();
			break;

		}

	}

	private void SetCircleAnimationComplete () {
		IsCircleAnimationComplete = true;
	}

}
