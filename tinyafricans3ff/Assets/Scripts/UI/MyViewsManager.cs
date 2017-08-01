using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using MarkLight;

public class MyViewsManager : MonoBehaviour {

	GameTitle gameTitle;
	GameObject gA;

	// Use this for initialization
	void Start () {
		gA = GameObject.Find("GameTitle");
		if (gA != null) {
			//gameTitle = gA.GetComponent<GameTitle> ();

			//ViewActionEntry vAE = new ViewActionEntry ();
			//gameTitle.ShowTitle3For.AnimationCompleted.AddEntry (new ViewActionEntry("MethodToBeCalled", gameObject));
		} 
	}

	void Update () {
	
	}

}

