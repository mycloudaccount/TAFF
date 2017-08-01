using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using DG.Tweening;
using Facebook.Unity;
using System.Collections.Generic;

public class LoginDialog : UIView {

	const float MOVE_DIALOG_OUT_DURATION = 0.4f;

	public Region MainRegionMount;
	public Region MainRegion;
	public Button LoginButton;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Login () {
		HideDialog();
	}

	public void ShowDialog () {

		MoveDialogIn ();
		GameManager.Instance.AddBackgroundToScene ("Login");

	}

	public void HideDialog () {
		Tween tween = LoginButton.transform.DOPunchScale(new Vector3(0.8f,0.8f,0), MOVE_DIALOG_OUT_DURATION, 5, 0.5f);
		GameManager.Instance.RemoveBackgroundFromScene ("Login"); 
		tween.OnComplete (()=>InitializeFBLoginPrompt());
	}

	private void MoveDialogIn () {
		MainRegion.transform.position = MainRegionMount.transform.position;
		MainRegion.transform.DOPunchScale (new Vector3 (0.2f, 0.2f, 0.2f), 1.0f, 6, 1);
	}

	private void InitializeFBLoginPrompt () {
		Tween tween = MainRegion.transform.DOMoveY (-480, 0.1f, true);
		tween.OnComplete (()=>LoginViaFaceBook());
	}

	private void LoginViaFaceBook () {
		List<string> permissions = new List<string> ();
		permissions.Add ("public_profile");
		// pause scene
		Time.timeScale = 1;
		FB.LogInWithReadPermissions (permissions, PostLogin);
	}

	private void PostLogin (IResult result) {

		if (result.Error != null) {

			Debug.Log ("ERROR - FB Auth ERROR: " + result.Error);
			Debug.Log ("Try FB Login Again...");

			// unpause scene
			Time.timeScale = 1;

			ShowDialog ();

		} else {
			if (FB.IsLoggedIn) {

				/*
				Raw Result Example:
				{
					"permissions": "user_friends,public_profile",
					"expiration_timestamp": "1472210945",
					"access_token": "EAAZAXZCXZBJxScBAKAht50ZCr7LHuO6O48wihv5jkI6ZCZCRZAR1ZCQ0L2ushnCcIQTOZAqN9ZB7Rjo4GsNRbHLpyCi2UrasWYM9spCyZBHGaRCnCqru8erBqD3qwOgCHt1mXtgydbDEJ841hWvIIEI0YaVWGx8BYmp9UKXlubwpZBjFHQZDZD",
					"user_id": "10208634047641544",
					"last_refresh": "1467026945",
					"granted_permissions": ["user_friends", "public_profile"],
					"declined_permissions": [],
					"callback_id": "2"
				}				 
				*/
				Debug.Log ("FB is logged in with the following RAW RESULT: " + result.RawResult);
				Debug.Log ("Contents of result dictionary: ");
				if (result.ResultDictionary != null) {
					foreach (string key in result.ResultDictionary.Keys) {
						Debug.Log(key + " : " + result.ResultDictionary[key].ToString());
					}
				}

				// store the user access token
				GameState.Instance.FBUserAccessToken = result.ResultDictionary["access_token"].ToString();

				// unpause scene
				Time.timeScale = 1;

				// log into playfab using users fb access token
				GameManager.Instance.LogIntoPlayfab ();

			} else {
				Debug.Log ("Try FB Login Again...");

				// unpause scene
				Time.timeScale = 1;

				ShowDialog ();
			}
		}

	}
		
}

