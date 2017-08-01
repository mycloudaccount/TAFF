using UnityEngine;
using System.Collections;
using MarkLight.Views.UI;
using MarkLight.Views;
using DG.Tweening;
using Facebook.Unity;
using System.Collections.Generic;
using MarkLight;

public class StatusDialog : UIView {

	public Image SisterOneFaceOpened;
	public Image EnemyOneFaceOpened;

	public _string CharacterName;
	public Region CharacterLabelRegionMount;
	public Label CharacterLabel;

	public _float HealthValue;
	public _float StrengthValue;

	void Awake(){

	}

	void Start () {
		InitializeView ();
	}

	public override void Initialize () {
		
		base.Initialize ();

		Debug.Log ("Status Dialog ID: " + this.Id);

		//Sprite sprite = new Sprite ();
		//Texture texture;
		if (this.Id == "HeroDialog") {
			HealthValue.Value = 85.0f;
			StrengthValue.Value = 68.0f;
			CharacterName.Value = "Benti";
			//texture = GameManager.LoadImageAsTexture("Assets/Resources/Characters/Omo/images/Sister1Opened.png") as Texture2D;
			//CharacterIconImageOne.Sprite.Value = Resources.Load("Characters/Omo/images/Sister1Opened.png", typeof(Sprite)) as Sprite;
			SisterOneFaceOpened.IsActive.Value = true;
		} else if (this.Id == "EnemyDialog") {
			HealthValue.Value = 45.0f;
			StrengthValue.Value = 78.0f;
			CharacterName.Value = "Grabber";
			//CharacterIconImageOne.Sprite.Value.texture = GameManager.LoadImageAsTexture("Assets/Resources/Characters/Omo/images/Sister1Opened.png") as Texture2D;
			//CharacterIconImageOne.Sprite.Value = Resources.Load("Characters/Omo/images/Sister1Closed.png", typeof(Sprite)) as Sprite;
			EnemyOneFaceOpened.IsActive.Value = true;
		}

	}

	void InitializeView () {

		// I need to wait on some position values to get set
		// accurately
		StartCoroutine(AnimateSlowerLoadingObject());

	}

	IEnumerator AnimateSlowerLoadingObject () {

		yield return new WaitForSeconds(1);

		CharacterLabel.transform.DOMove (CharacterLabelRegionMount.transform.position, 0.5f, true);

	}

}