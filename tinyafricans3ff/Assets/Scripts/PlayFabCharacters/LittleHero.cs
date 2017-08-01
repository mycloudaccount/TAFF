using System;
using UnityEngine;

[Serializable]
public abstract class LittleHero
{

	protected LittleHero (string characterId) {

		// set the avatar id
		AvatarId = characterId;

	} //make default constructor protected so only descendents can get at it	

	public bool PlayFabItemLoaded = false;

	public bool PresentFromStart = false;

	public string HairColor = Constants.DEFAULT_HAIR_COLOR;

	public string AvatarId;

	public int Health;

	public int Score;

	public int Lives;

	public string Id;

	public string FirstName;

	public string LastName;

	public string Description;

	public string HairPath;

	public string HeadBandPath;

	public string ClothesPath;

	public string AnimatorController;

	public void CloneCharacter (LittleHero gC) {

		this.HairColor = gC.HairColor;

		this.AvatarId = gC.AvatarId;

		this.Health = gC.Health;

		this.Score = gC.Score;

		this.Lives = gC.Lives;

		this.Id = gC.Id;

		this.FirstName = gC.FirstName;

		this.LastName = gC.LastName;

		this.Description = gC.Description;

		this.HairPath = gC.HairPath;

		this.HeadBandPath = gC.HeadBandPath;

		this.ClothesPath = gC.ClothesPath;

		this.AnimatorController = gC.AnimatorController;

	}

	public static class Constants
	{

		// default hair color
		public const string DEFAULT_HAIR_COLOR = "#FF0000";

	}

}

