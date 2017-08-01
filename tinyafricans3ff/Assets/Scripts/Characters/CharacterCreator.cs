using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterCreator : MonoBehaviour {

	PlayMakerFSM[] playMakerFSMs;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	// Set the CharacterAvatarPrefab
	public void SetCharacterAvatar() {

		GameCharacter selectedChar = GameManager.Instance.GetSelectedCharacter ();
		GameObject selectCharAvatar = selectedChar.GetAvatar();

		// we want the object name to be the actual char Id
		selectCharAvatar.name = selectedChar.AvatarId;

		playMakerFSMs = gameObject.GetComponents<PlayMakerFSM>();
		playMakerFSMs [0].FsmVariables.GetFsmGameObject ("CharacterAvatarPrefab").Value = selectCharAvatar.gameObject;
		Destroy (selectCharAvatar);

	}

}

