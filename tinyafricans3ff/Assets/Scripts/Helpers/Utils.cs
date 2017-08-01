using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;

public static class Utils
{

	public static GameObject attachComponent (GameObject gameObject, string pathToComponentParent, string pathToComponent) {
		Transform componentParent = gameObject.transform.Find(pathToComponentParent);
		if (gameObject == null || componentParent == null) return null;

		GameObject attachment = GameObject.Instantiate(Resources.Load(pathToComponent, typeof(GameObject))) as GameObject;

		Debug.Log ("Attaching the following Component to " + componentParent.name + ": " + attachment.name);

		if ( attachment.name.IndexOf("(clone)", StringComparison.OrdinalIgnoreCase) >= 0 ) {
			attachment.name = Regex.Replace(attachment.name, "(clone)", "", RegexOptions.IgnoreCase);
			attachment.name = Regex.Replace(attachment.name, "[^0-9A-Za-z]+", "", RegexOptions.IgnoreCase);
		}

		attachment.transform.parent = componentParent;
		attachment.transform.localPosition = Vector3.zero;
		attachment.transform.localScale = Vector3.one;
		attachment.transform.localRotation = componentParent.transform.localRotation;
		attachment.transform.Rotate (new Vector3 (0,0,-90));
		return attachment;
	}

	public static Quaternion removeComponent (GameObject gameObject, string pathToComponentParent, string componentName) {
		Transform componentParent = gameObject.transform.Find(pathToComponentParent);
		if (gameObject == null || componentParent == null) return Quaternion.identity;

		Quaternion rotation = new Quaternion();

		if (componentParent != null) {
			Transform component = componentParent.Find (componentName);
			if (component != null) {
				rotation = component.localRotation;
				Debug.Log ("Removing the following Component from " + componentParent.name + ": " + component.name);
				component.parent = null;
				GameObject.Destroy (component.gameObject);
			}
		}

		return rotation;

	}

	public static void addMaterial (Transform theComponent, string pathToTexture, string pathToMaterial = "LittleHeros/Materials/Sisters") {
		if (theComponent != null) {
			Renderer renderer = theComponent.gameObject.GetComponent<Renderer>();
			if (pathToMaterial != null) {
				Material material = GameObject.Instantiate (Resources.Load (pathToMaterial, typeof(Material))) as Material;
				renderer.material = material;
			}

			// update the texture
			if (pathToTexture != null) {
				Debug.Log ("Loading the following texture file: " + pathToTexture);
				Texture2D tex = (Texture2D)Resources.Load(pathToTexture, typeof(Texture2D));
				renderer.material.SetTexture("_MainTex", tex);
			}
		}
	}

	public static void addColor (Transform theComponent, string colorString) {	
		if (theComponent != null) {
			Renderer renderer = theComponent.gameObject.GetComponent<Renderer> ();
			Color colorColor = GameManager.Instance.HexToColor (colorString);
			renderer.material.SetColor("_Color", colorColor);
		}
	}

	public static void addToonShader (Transform theComponent) {	
		if (theComponent != null) {
			Renderer renderer = theComponent.gameObject.GetComponent<Renderer> ();
			Shader shader = Shader.Find( "Toon/Basic" );
			renderer.material.shader = shader;
		}
	}

	public static string getObjectNameFromPath (string type) {

		string name;
		char[] delimiterChars = { '/' };

		string[] nameParts = type.Split (delimiterChars);
		if (nameParts.Length > 0) {
			name = nameParts [nameParts.Length - 1];
		} else {
			name = type;
		}

		return Regex.Replace(name, @"\s+", "");

	}

	public static IEnumerator QueueSystemCoroutine(IEnumerator userCoroutine, Action callback = null)
	{
		// Run the user's coroutine (e.g. DownloadText)
		while (userCoroutine.MoveNext())
		{
			yield return userCoroutine.Current;
		}

		// Notify that the coroutine is finished
		if (callback != null) {
			callback();
		}
	}

}

