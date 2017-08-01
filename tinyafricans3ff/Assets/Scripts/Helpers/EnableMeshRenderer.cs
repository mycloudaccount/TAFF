using UnityEngine;
using System.Collections;

public class EnableMeshRenderer : MonoBehaviour
{
	
	private Renderer rend;

	// Use this for initialization
	void Start ()
	{
		rend = GetComponent<Renderer>();
		rend.enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

}

