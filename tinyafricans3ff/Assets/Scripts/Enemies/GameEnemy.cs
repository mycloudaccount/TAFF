using System;
using UnityEngine;

[Serializable]
public abstract class GameEnemy
{

	protected GameEnemy (string characterId) {

		// set the avatar id
		AvatarId = characterId;

	} //make default constructor private to prevent instantiation		

	public string AvatarId;

	public int Health;

	public int Score;

	public int Lives;

	public GameObject GetAvatar () {

		return GameManager.Instance.LoadEnemyAvatar (AvatarId);

	}

	public static class Constants
	{

	}

}

