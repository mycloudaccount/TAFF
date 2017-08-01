using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WorldMapManager : MonoBehaviour
{

	// Use this for initialization
	void Awake ()
	{
		GameManager.Instance.CurrentWorldMap = gameObject;
	}
	
	// Update is called once per frame
	void Start ()
	{
	
	}



}

