using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

		// Check to see who the selected enemy is
		// and load it up
		GameEnemy selectedEnemy = GameManager.Instance.GetSelectedEnemy ();
		GameObject selectEnemyAvatar = selectedEnemy.GetAvatar();

		// we want the object name to be the actual Enemy Id
		selectEnemyAvatar.name = selectedEnemy.AvatarId;

		Debug.Log ("Moving the following Enemy Into Position: " + selectedEnemy.AvatarId);

		// move avatar position to the position of the parent game object
		selectEnemyAvatar.transform.position = gameObject.transform.position;
		selectEnemyAvatar.transform.localRotation = gameObject.transform.localRotation;
		selectEnemyAvatar.transform.localScale = gameObject.transform.localScale;
		selectEnemyAvatar.transform.SetParent (gameObject.transform);

		// set the layer to enemy stage
		GameManager.SetLayerForAllChildren (gameObject,"VillainStage");
	
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

