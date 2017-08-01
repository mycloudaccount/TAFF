using UnityEngine;
using System.Collections;
using MarkLight;
using MarkLight.Views.UI;
using DG.Tweening;
using System.Collections.Generic;

public class ChallengeHUD : UIView
{

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

		Canvas canvas = gameObject.transform.parent.GetComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		GameManager.SetLayerForAllChildren(canvas.gameObject, "FrontStage");

	}

	void InitializeView () {

		// I need to wait on some position values to get set
		// accurately
		StartCoroutine(AnimateSlowerLoadingObject());

	}

	IEnumerator AnimateSlowerLoadingObject () {

		yield return new WaitForSeconds(1);

	}

}




