using System;
using UnityEngine;
using System.Collections.Generic;

public class HeadGearManager : GearManager
{

	private string pathToParent = "LittleHeroBase/RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigNeck/RigHead/Dummy Prop Head";
	private string pathToGear = "LittleHeros/Prefabs/03 Attach Accessories";
	private string pathToHairStyles = "LittleHeros/Prefabs/02 Attach Hair";

	private List<string> HeadGear;

	public HeadGearManager () {
		HeadGear = new List<string>();
		HeadGear.Add (Constants.HEADBAND);
	}

	public override void AddComponent (GameObject gameObject, GameCharacter gc, string componentType) {

		switch(componentType) {
		case Constants.HAIR:
			if (gc.HairPath != null && gc.HairPath != "") {
				RemoveComponent (gameObject, gc, componentType);
				Debug.Log ("Adding HEro Component: " + Utils.getObjectNameFromPath (gc.HairPath));
				Utils.attachComponent (
					gameObject,
					pathToParent,
					pathToHairStyles + "/" + gc.HairPath
				);
				// add color to hair
				Transform hairParent = gameObject.transform.Find (pathToParent);
				Transform hair = hairParent.FindChild (Utils.getObjectNameFromPath (gc.HairPath));
				Utils.addColor (
					hair,
					gc.HairColor
				);			
				Utils.addToonShader (hair);			
			}
			break;
		case Constants.HEADBAND:
			if (gc.HeadBandPath != null && gc.HeadBandPath != "" && gc.OwnsHeadBand) {
				removeAllGear (gameObject, gc);
				Debug.Log ("Adding HEro Component: " + Utils.getObjectNameFromPath (gc.HeadBandPath));
				GameObject attachment = Utils.attachComponent (
					gameObject,
					pathToParent,
					pathToGear + "/" + gc.HeadBandPath
				);
				if (attachment != null) {
					attachment.transform.localScale = new Vector3 (1.3f, 1.3f, 1.13f);
					attachment.transform.localPosition = new Vector3 (0.0f, -0.03f, 0.02f);
				}
			}
			break;
		default:
			break;
		}

	}

	public override Quaternion RemoveComponent (GameObject gameObject, GameCharacter gc, string componentType) {
		
		string componentName;
		switch(componentType) {
		case Constants.HAIR:
			componentName = Utils.getObjectNameFromPath (gc.HairPath);
			Debug.Log ("Removing HEro Component: " + componentName);
			return Utils.removeComponent (
				gameObject,
				pathToParent,
				componentName
			);
		case Constants.HEADBAND:
			componentName = Utils.getObjectNameFromPath (gc.HeadBandPath);
			Debug.Log ("Removing HEro Component: " + componentName);
			return Utils.removeComponent (
				gameObject,
				pathToParent,
				componentName
			);
		default:
			return Quaternion.identity;
		}

	}

	private void removeAllGear (GameObject gameObject, GameCharacter gc) {

		// removing all gear from head (except for hair)
		foreach (string gear in HeadGear) {
			RemoveComponent (gameObject, gc, gear);
		}

	}

	public static class Constants
	{
		public const string FACE_OPENED = "FACE_OPENED";
		public const string FACE_CLOSED = "FACE_CLOSED";
		public const string HAIR = "HAIR";
		public const string HEADBAND = "HEADBAND";
	}

}
