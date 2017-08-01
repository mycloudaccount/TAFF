using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using DG.Tweening;
using MarkLight;

public class LevelManager : MonoBehaviour
{

	float TIME_TO_COMPLETE = 10.0f;

	// Use this for initialization
	void Start ()
	{
	
		// start the clock ticking for how long the player lasts in this level
		MonitorGame ();

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void MonitorGame () {

		// track time in level
		StartCoroutine(TrackTimeInLevel());

	}

	// if the user lasts TIME_TO_COMPLETE seconds then they have concured this level
	IEnumerator TrackTimeInLevel () {

		Debug.Log ("Start tracking time in Level: " + PlayerState.Current.SelectedLevel);
		yield return new WaitForSeconds(TIME_TO_COMPLETE);

		Debug.Log ("Done tracking. The Player Has Conquered Level: " + PlayerState.Current.SelectedLevel);
		MoveHUDOut ();

	}

	private void MoveHUDOut () {

		GameManager.Instance.ExitMenu (false);

	}


}

