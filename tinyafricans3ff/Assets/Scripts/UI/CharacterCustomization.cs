using UnityEngine;
using System.Collections;
using MarkLight;
using MarkLight.Views.UI;
using DG.Tweening;
using System.Collections.Generic;


public class CharacterCustomization : UIView
{

	private GameCharacter savedCharacterSettings;

	public ObservableList<ObservableList<ColorSwitch>> HairColorSwitchList;
	public Image HairColorMarkerImage;
	public Image HairColorIndicatorImage;

	public Region NavigationRegion;
	public Image NavigationRegionMountImage;

	public ElementSize OptionsWidth;
	public ElementSize OptionsHeight;
	public Material SunRaysMaterialTypeA;
	public Image SunRaysTypeA00;
	public Image SunRaysTypeA01;

	public TabPanel CustomOptionsTabPanel;
	public Region CustomizationRegion;

	public Region HairTabRegion;
	public Region ClothesTabRegion;
	public Region HeadBandTabRegion;

	public TabHeader HairTabHeader;
	public TabHeader ClothesTabHeader;
	public TabHeader HeadBandTabHeader;

	public Image CogMount0Image;
	public Image CogMount1Image;
	public Image CogMount2Image;
	public Image CogMount3Image;
	public Image CogTypeAImageBkg;
	public Image CogTypeAImage0;
	public Image CogTypeAImage1;
	public Image CogTypeAImage2;
	public Image CogTypeAImage3;

	public Image CogTypeAImage4;
	public Image CogTypeAImage5;
	public Image CogTypeAImage6;
	public Image CogTypeAImage7;

	public Image CogTypeAImage8;
	public Image CogTypeAImage9;
	public Image CogTypeAImage10;
	public Image CogTypeAImage11;

	private bool hairTabPanelLoaded = false;
	private bool clothesTabPanelLoaded = false;
	private bool headBandTabPanelLoaded = false;

	// store switches
	public Button HairColorSwitch0;
	public Button HairColorSwitch1;
	public Button HairColorSwitch2;
	public Button HairColorSwitch3;
	public Button HairColorSwitch4;
	public Button HairColorSwitch5;
	public Button HairColorSwitch6;
	public Button HairColorSwitch7;
	public Button HairColorSwitch8;
	public Button HairColorSwitch9;
	public Button HairColorSwitch10;
	private List<Button> HairColorSwitches;

	// whether or not exiting this view results in a scene change
	private bool changeScene = true;

	private string currentHairColor = GameCharacter.Constants.DEFAULT_HAIR_COLOR;

	private Hero savedGameCharacter;

	void Awake(){
	}

	void Start () {
		InitializeView ();
	}

	public override void Initialize () {
		
		base.Initialize ();
		OptionsWidth = new ElementSize(260, ElementSizeUnit.Pixels);
		OptionsHeight = new ElementSize(350, ElementSizeUnit.Pixels);

		SunRaysMaterialTypeA = Resources.Load("Particles/Materials/Light_02", typeof(Material)) as Material;
		SunRaysMaterialTypeA.SetColor ("_TintColor", Color.magenta);

		HairColorSwitchList = new ObservableList<ObservableList<ColorSwitch>>();

		ObservableList<ColorSwitch> HairColorSwitches0 = new ObservableList<ColorSwitch>();
		HairColorSwitches0.Add(new ColorSwitch { ColorString = "#FF4E00", Id = "HairColorSwitch"+0 });
		HairColorSwitches0.Add(new ColorSwitch { ColorString = "#8EA604", Id = "HairColorSwitch"+1 });
		HairColorSwitches0.Add(new ColorSwitch { ColorString = "#F5BB00", Id = "HairColorSwitch"+2 });
		HairColorSwitchList.Add (HairColorSwitches0);

		ObservableList<ColorSwitch> HairColorSwitches1 = new ObservableList<ColorSwitch>();
		HairColorSwitches1.Add(new ColorSwitch { ColorString = "#FFA69E", Id = "HairColorSwitch"+3 });
		HairColorSwitches1.Add(new ColorSwitch { ColorString = "#BF3100", Id = "HairColorSwitch"+4 });
		HairColorSwitches1.Add(new ColorSwitch { ColorString = "#3772FF", Id = "HairColorSwitch"+5 });
		HairColorSwitchList.Add (HairColorSwitches1);

		ObservableList<ColorSwitch> HairColorSwitches2 = new ObservableList<ColorSwitch>();
		HairColorSwitches2.Add(new ColorSwitch { ColorString = "#F038FF", Id = "HairColorSwitch"+6 });
		HairColorSwitches2.Add(new ColorSwitch { ColorString = "#70E4EF", Id = "HairColorSwitch"+7 });
		HairColorSwitches2.Add(new ColorSwitch { ColorString = "#FEEC65", Id = "HairColorSwitch"+8 });
		HairColorSwitchList.Add (HairColorSwitches2);

		ObservableList<ColorSwitch> HairColorSwitches3 = new ObservableList<ColorSwitch>();
		HairColorSwitches3.Add(new ColorSwitch { ColorString = "#000000", Id = "HairColorSwitch"+9 });
		HairColorSwitches3.Add(new ColorSwitch { ColorString = "#773344", Id = "HairColorSwitch"+10 });
		HairColorSwitchList.Add (HairColorSwitches3);

		Canvas canvas = gameObject.transform.parent.GetComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		GameManager.SetLayerForAllChildren (gameObject.transform.parent.gameObject, "FrontStage");

	}

	void InitializeView () {

		// I need to wait on some position values to get set
		// accurately
		StartCoroutine(AnimateSlowerLoadingObject());

		StartSunRayAnimations ();

		// animate the tab buttons
		HairTabHeader.Scale.Value = new Vector3 (0.0f,0.0f,0.0f);
		ClothesTabHeader.Scale.Value = new Vector3 (0.0f,0.0f,0.0f);
		HeadBandTabHeader.Scale.Value = new Vector3 (0.0f,0.0f,0.0f);
		StartTabHeaderAnimation ();

		// store all the non-updated setting for the current character
		savedGameCharacter = new Hero();
		savedGameCharacter.CloneCharacter(GameManager.Instance.GetSelectedCharacter ());

	}

	private void SetCurrentHairColor (string cColor) {
		
		currentHairColor = cColor;
		HairColorIndicatorImage.BackgroundColor.Value = GameManager.Instance.HexToColor (cColor);

		// also update the current characters hair color
		GameCharacter gC = GameManager.Instance.GetSelectedCharacter ();
		gC.HairColor = cColor;
		gC.ReconfigureHero ();
	
	}

	private Vector3 GetSelectedColorsPosition (ObservableList<ObservableList<ColorSwitch>> switchList) {

		foreach (ObservableList<ColorSwitch> row in switchList) {

			foreach (ColorSwitch cSwitch in row) {
				if (currentHairColor.CompareTo(cSwitch.ColorString) != -1) {
					// get associated switch button given the switch id
					Button btn = GetButtonFromSelectedColor(cSwitch.ColorString, HairColorSwitches);
					if (btn != null) {
						return btn.transform.position;
					} else {
						Debug.Log ("WARNING: Could not find Button matching currently selected hair color: " + currentHairColor);
					}
				}
			}

		}

		return HairColorMarkerImage.transform.position;

	}

	private Button GetButtonFromSelectedColor (string cString, List<Button> switches) {

		string btnColorString;

		foreach (Button btn in switches) {

			if (btn != null) {
				btnColorString = GameManager.Instance.ColorToHex (btn.BackgroundColor.Value);
				if (btnColorString == cString) {
					return btn;
				}
			} else {
				Debug.Log ("WARNING: button in array of switches is null");
			}

		}

		return null;

	}

	IEnumerator AnimateSlowerLoadingObject () {
		yield return new WaitForSeconds(1);

		// add bnts to array
		HairColorSwitches = new List<Button>();
		HairColorSwitches.Add (HairColorSwitch0);
		HairColorSwitches.Add (HairColorSwitch1);
		HairColorSwitches.Add (HairColorSwitch2);
		HairColorSwitches.Add (HairColorSwitch3);
		HairColorSwitches.Add (HairColorSwitch4);
		HairColorSwitches.Add (HairColorSwitch5);
		HairColorSwitches.Add (HairColorSwitch6);
		HairColorSwitches.Add (HairColorSwitch7);
		HairColorSwitches.Add (HairColorSwitch8);
		HairColorSwitches.Add (HairColorSwitch9);
		HairColorSwitches.Add (HairColorSwitch10);

		LoadMenu ();

		StartHairCogAnimation ();
		StartClothesCogAnimation ();
		StartHeadBandCogAnimation ();

		// add charcter to the scene
		MoveInCharacter ();

	}

	public void MoveNavigationRegionIn () {

		NavigationRegion.transform.DORotate (new Vector3(0,180,0), 1, RotateMode.Fast).SetEase(Ease.OutBounce).SetDelay(0.8f);
		NavigationRegion.transform.DOMove (NavigationRegionMountImage.transform.position, 1, true).SetEase(Ease.OutBounce).SetDelay(0.8f);

	}

	public void MoveNavigationRegionOut (bool changeScene) {

		NavigationRegion.transform.DOKill (false);
		Tween navTween = NavigationRegion.transform.DOMove (new Vector3 (NavigationRegion.transform.position.x+40, NavigationRegion.transform.position.y+40, NavigationRegion.transform.position.z) , 0.5f, true).SetEase(Ease.InExpo);

		if (changeScene) {
			// go back to the previous scene
			GameManager.Instance.RemoveFromSceneBreadCrumb (); 
			navTween.OnComplete (()=>GameManager.Instance.LoadNextScene(true));
		} else {
			GameManager.Instance.MoveInNextMenu (navTween);
		}
	
	}

	private void LoadMenuSettings () {

		GameCharacter gC = GameManager.Instance.GetSelectedCharacter ();
		SetCurrentHairColor (gC.HairColor);

	}

	public void  LoadMenu () {

		LoadMenuSettings ();
		MoveMenuIn ();
	
	}

	private void MoveMenuIn () {

		MoveNavigationRegionIn ();
		MoveCustomizationTabIn ();

	}

	private void MoveCustomizationTabIn () {
	}

	private void MoveCustomizationTabOut () {
	
		CustomizationRegion.transform.DOMove (new Vector3 (CustomizationRegion.transform.position.x - 100, CustomizationRegion.transform.position.y,CustomizationRegion.transform.position.z), 0.5f, true).SetEase(Ease.InExpo);
	
	}

	public void Exit (Button btn) {
		

		// TODO: check and any setting has changed like hair color. etc...
		if (currentHairColor != savedGameCharacter.HairColor) {
			GameManager.Instance.UpdateSinceLastSave = true;
		}

		// if we have changed any settings since the last save then we better warn the user
		if (GameManager.Instance.UpdateSinceLastSave) {
			GameManager.SaveGameStateDialogInScene.ShowDialog ();
		} else {
			ExitMenu (false);
		}

	}

	public void ExitMenu (bool saveSettings) {

		if (saveSettings) {
			SaveMenuSettings ();
		} else {
			if (GameManager.Instance.UpdateSinceLastSave) {
				DiscardMenuSettings ();
			}
		}
		GameManager.Instance.RemoveFromMenuBreadCrumb ();
		MoveMenuOut (changeScene);

	}

	private void MoveMenuOut (bool changeScene) {
		
		MoveNavigationRegionOut (changeScene);
		MoveCustomizationTabOut ();
	
	}

	private void SaveMenuSettings () {
		
		// save the char cust settings
		//GameCharacter gC = GameManager.Instance.GetSelectedCharacter();
		//gC.HairColor = currentHairColor;

		// save the uber gamestate object
		GameManager.Instance.SaveGameState();

	}

	private void DiscardMenuSettings () {

		// discard the char cust settings
		//GameCharacter gC = GameManager.Instance.GetSelectedCharacter();
		//SetCurrentHairColor(gC.HairColor);
		SetCurrentHairColor(savedGameCharacter.HairColor);

		GameManager.Instance.UpdateSinceLastSave = false;

	}

	public void UpdateHairColor(Button ClickedButton) {

		string btnColorString = GameManager.Instance.ColorToHex (ClickedButton.BackgroundColor.Value);

		if (btnColorString != currentHairColor) {
			
			// Let's animate the color switch
			ClickedButton.transform.DOKill(true);
			ClickedButton.transform.DOPunchScale (new Vector3 (0.5f, 0.5f, 0.5f), 0.4f, 9, 1.0f);
			SetCurrentHairColor (btnColorString);

			// Move item marker to selected color switch
			MoveMarkToColorSwitch(ClickedButton.transform.position);

			// Do the actual work here

		} else {
			return;
		}
	
	}

	public void MouseEnterColorSwitch(Button ClickedButton) {

		// Let's animate the color switch
		ClickedButton.transform.DOPunchScale(new Vector3(0.2f,0.2f,0.2f), 0.8f, 4, 0.5f);

	}

	public void MouseExitColorSwitch(Button ClickedButton) {

		// Lets animate the color switch
		ClickedButton.transform.DOKill(true);

	}

	public void MoveMarkToColorSwitch(Vector3 newPosition) {
		Transform transform = HairColorMarkerImage.transform;
		Tween scaleTween;
		Tween transTween = transform.DOMove (newPosition, 0.8f, false);
		if (transform.localScale.x == 0.0f) {
			scaleTween = transform.DOScale (new Vector3 (1, 1, 1), 0.8f);
			scaleTween.SetEase (Ease.InOutCubic);
		} else {
			Sequence mySequence = DOTween.Sequence();
			mySequence.Append(transform.DOScale (new Vector3 (0.4f, 0.4f, 0.4f), 0.4f));
			mySequence.Append(transform.DOScale (new Vector3 (1, 1, 1), 0.4f));
		}
		transTween.SetEase(Ease.InOutCubic);
	}

	public void StartSunRayAnimations() {
	
		SunRaysMaterialTypeA.DOColor(Color.cyan, "_TintColor", 5).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		SunRaysTypeA00.transform.DORotate (new Vector3 (0.0f, 0.0f, 180.0f), 5, RotateMode.LocalAxisAdd).SetEase (Ease.Linear).SetLoops (-1);
		SunRaysTypeA01.transform.DORotate (new Vector3 (0.0f, 0.0f, 180.0f), 5, RotateMode.LocalAxisAdd).SetEase (Ease.Linear).SetLoops (-1);

	}

	public void AnimateSelectedTab() {

		if (CustomOptionsTabPanel.SelectedTab.Id == "HairTab" && hairTabPanelLoaded) {
			RestartHairCogAnimation ();
		} else if (CustomOptionsTabPanel.SelectedTab.Id == "ClothesTab" && clothesTabPanelLoaded) {
			RestartClothesCogAnimation ();
		} else if (CustomOptionsTabPanel.SelectedTab.Id == "HeadBandTab" && headBandTabPanelLoaded) {
			RestartHeadBandCogAnimation ();
		}

	}

	public void StartTabHeaderAnimation() {
	
		StartHairTabHeaderAnimation ();

	}
	private void StartHairTabHeaderAnimation() {

		Tweener tweener = HairTabHeader.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.5f);
		tweener.SetDelay (1.0f);
		tweener.SetEase (Ease.OutBounce);
		tweener.OnComplete (StartClothesTabHeaderAnimation);

	}
	private void StartClothesTabHeaderAnimation() {

		Tweener tweener = ClothesTabHeader.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.5f);
		tweener.SetEase (Ease.OutBounce);
		tweener.OnComplete (StartHeadBandTabHeaderAnimation);
	}
	private void StartHeadBandTabHeaderAnimation() {

		Tweener tweener = HeadBandTabHeader.transform.DOScale (new Vector3 (1.0f, 1.0f, 1.0f), 0.5f);
		tweener.SetEase (Ease.OutBounce);

	}

	public void StartHairCogAnimation() {

		CogTypeAImage0.transform.DOKill (false);
		CogTypeAImage1.transform.DOKill (false);
		CogTypeAImage2.transform.DOKill (false);
		CogTypeAImage3.transform.DOKill (false);
		 
		CogTypeAImage0.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage1.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage2.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage3.transform.position = CogTypeAImageBkg.transform.position;

		AnimateCog (
			CogTypeAImage0, 
			CogMount0Image.transform.position, 
			new Vector3 (0.5f, 0.5f, 1), 
			90.0f,
			0.8f, 
			0.0f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage1, 
			CogMount1Image.transform.position, 
			new Vector3 (1.7f, 1.7f, 1), 
			-180.0f,
			1.0f, 
			0.5f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage2, 
			CogMount2Image.transform.position, 
			new Vector3 (1.3f, 1.3f, 1), 
			-180.0f,
			1.5f, 
			0.8f,
			4.0f
		);

		AnimateCog (
			CogTypeAImage3, 
			CogMount3Image.transform.position, 
			new Vector3 (1.0f, 1.0f, 1), 
			-270.0f,
			1.5f, 
			1.8f,
			3.0f
		);

		hairTabPanelLoaded = true;

	}

	public void StartClothesCogAnimation() {

		CogTypeAImage4.transform.DOKill (false);
		CogTypeAImage5.transform.DOKill (false);
		CogTypeAImage6.transform.DOKill (false);
		CogTypeAImage7.transform.DOKill (false);

		CogTypeAImage4.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage5.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage6.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage7.transform.position = CogTypeAImageBkg.transform.position;

		AnimateCog (
			CogTypeAImage4, 
			CogMount0Image.transform.position, 
			new Vector3 (0.5f, 0.5f, 1), 
			90.0f,
			0.8f, 
			0.0f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage5, 
			CogMount1Image.transform.position, 
			new Vector3 (1.7f, 1.7f, 1), 
			-180.0f,
			1.0f, 
			0.5f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage6, 
			CogMount2Image.transform.position, 
			new Vector3 (1.3f, 1.3f, 1), 
			-180.0f,
			1.5f, 
			0.8f,
			4.0f
		);

		AnimateCog (
			CogTypeAImage7, 
			CogMount3Image.transform.position, 
			new Vector3 (1.0f, 1.0f, 1), 
			-270.0f,
			1.5f, 
			1.8f,
			3.0f
		);

		clothesTabPanelLoaded = true;

	}

	public void StartHeadBandCogAnimation() {

		CogTypeAImage8.transform.DOKill (false);
		CogTypeAImage9.transform.DOKill (false);
		CogTypeAImage10.transform.DOKill (false);
		CogTypeAImage11.transform.DOKill (false);

		CogTypeAImage8.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage9.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage10.transform.position = CogTypeAImageBkg.transform.position;
		CogTypeAImage11.transform.position = CogTypeAImageBkg.transform.position;

		AnimateCog (
			CogTypeAImage8, 
			CogMount0Image.transform.position, 
			new Vector3 (0.5f, 0.5f, 1), 
			90.0f,
			0.8f, 
			0.0f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage9, 
			CogMount1Image.transform.position, 
			new Vector3 (1.7f, 1.7f, 1), 
			-180.0f,
			1.0f, 
			0.5f,
			2.0f
		);

		AnimateCog (
			CogTypeAImage10, 
			CogMount2Image.transform.position, 
			new Vector3 (1.3f, 1.3f, 1), 
			-180.0f,
			1.5f, 
			0.8f,
			4.0f
		);

		AnimateCog (
			CogTypeAImage11, 
			CogMount3Image.transform.position, 
			new Vector3 (1.0f, 1.0f, 1), 
			-270.0f,
			1.5f, 
			1.8f,
			3.0f
		);

		headBandTabPanelLoaded = true;

	}

	private void AnimateCog(Image cog, Vector3 newPosition, Vector3 newScale, float deltaRotation, float animationDuration, float animationDelay, float loopDuration) {

		Tweener scaleTween = cog.transform.DOScale (newScale, animationDuration);
		scaleTween.SetDelay (animationDelay);
		scaleTween.SetEase (Ease.OutBounce);

		Vector3 newLoc = newPosition;
		Tweener moveTween = cog.transform.DOMove (newLoc, animationDuration, true);
		moveTween.SetDelay (animationDelay);
		moveTween.SetEase (Ease.OutBounce);
		moveTween.OnComplete(()=>LoopCogAnimation(cog, deltaRotation, loopDuration));

	}

	private Vector3 getNewPosition(Vector3 oldPosition, Vector3 deltaPosition) {

		Vector3 newPosition = new Vector3(
			oldPosition.x + deltaPosition.x,
			oldPosition.y + deltaPosition.y,
			oldPosition.z + deltaPosition.z
		);

		return newPosition;

	}

	public void LoopCogAnimation(Image cog, float deltaRotation, float loopDuration) {

		cog.transform.DORotate (new Vector3 (0, 0, deltaRotation), loopDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);

	}

	public void RestartHairCogAnimation() {

		Tweener sunRayBeforeTween = SunRaysTypeA00.transform.DOScale (new Vector3(0.0f,0.0f,0.0f), 0.2f).SetEase(Ease.Linear);
		sunRayBeforeTween.OnComplete (StartHairCogAnimation);

		HairTabRegion.transform.DOKill (false);
		Tweener scaleTween = HairTabRegion.transform.DOPunchScale (new Vector3(0.2f,0.2f,0.2f), 0.5f, 9, 1.0f);
		scaleTween.SetDelay (0.0f);

		Tweener sunRayAfterTween = SunRaysTypeA00.transform.DOScale (new Vector3(1.0f,1.0f,1.0f), 0.8f).SetEase(Ease.Linear);
		sunRayAfterTween.SetDelay (0.5f);

	}

	public void RestartClothesCogAnimation() {

		Tweener sunRayBeforeTween = SunRaysTypeA00.transform.DOScale (new Vector3(0.0f,0.0f,0.0f), 0.2f).SetEase(Ease.Linear);
		sunRayBeforeTween.OnComplete (StartClothesCogAnimation);

		ClothesTabRegion.transform.DOKill (false);
		Tweener scaleTween = ClothesTabRegion.transform.DOPunchScale (new Vector3(0.2f,0.2f,0.2f), 0.5f, 9, 1.0f);
		scaleTween.SetDelay (0.0f);

		Tweener sunRayAfterTween = SunRaysTypeA00.transform.DOScale (new Vector3(1.0f,1.0f,1.0f), 0.8f).SetEase(Ease.Linear);
		sunRayAfterTween.SetDelay (0.5f);

	}

	public void RestartHeadBandCogAnimation() {

		Tweener sunRayBeforeTween = SunRaysTypeA00.transform.DOScale (new Vector3(0.0f,0.0f,0.0f), 0.2f).SetEase(Ease.Linear);
		sunRayBeforeTween.OnComplete (StartHeadBandCogAnimation);

		HeadBandTabRegion.transform.DOKill (false);
		Tweener scaleTween = HeadBandTabRegion.transform.DOPunchScale (new Vector3(0.2f,0.2f,0.2f), 0.5f, 9, 1.0f);
		scaleTween.SetDelay (0.0f);

		Tweener sunRayAfterTween = SunRaysTypeA00.transform.DOScale (new Vector3(1.0f,1.0f,1.0f), 0.8f).SetEase(Ease.Linear);
		sunRayAfterTween.SetDelay (0.5f);

	}

	private void MoveInCharacter () {

		Debug.Log ("Adding a new Character to the Character Customization Scene...");

		// add character to scene from prefab
		string fileLocation = "LittleHeros/LittleHero";
		Debug.Log ("Instantiating New Hero...");
		GameObject characterInView = Instantiate(Resources.Load(fileLocation, typeof(GameObject))) as GameObject;
		if (characterInView == null) {
			Debug.Log ("Could not load a new Hero");
		} else {
			// place our new hero at the proper location
			GameObject heroMarker = GameObject.Find("HeroMarker");
			characterInView.transform.parent = heroMarker.transform;
			characterInView.transform.localPosition = new Vector3(0.0f, 0.52f, 0.0f);
			Debug.Log ("heroMarker.transform.parent.localScale: " + heroMarker.transform.parent.localScale);
			characterInView.transform.localScale = new Vector3 (
				1.5f / heroMarker.transform.parent.localScale.x,
				1.5f / heroMarker.transform.parent.localScale.y,
				1.5f / heroMarker.transform.parent.localScale.z
			);
		}

	}

}
	

