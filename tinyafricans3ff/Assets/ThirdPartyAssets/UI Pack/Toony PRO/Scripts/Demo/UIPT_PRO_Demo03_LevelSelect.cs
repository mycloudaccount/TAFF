// UI Pack : Toony PRO
// Version: 1.1.1
// Compatilble: Unity 4.7.1 and Unity 5.3.4 or higher, more info in Readme.txt file.
//
// Author:	Gold Experience Team (http://www.ge-team.com)
// Details:	https://www.assetstore.unity3d.com/en/#!/content/44103
// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

#endregion // Namespaces

// ######################################################################
// UIPT_PRO_Demo03_LevelSelect class
//
// Handles "Demo 03 - Landscape - Level Select" and "Demo 03 - Portrait - Level Select" scenes.
// ######################################################################

public class UIPT_PRO_Demo03_LevelSelect : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// UIs
	public GAui m_Banner = null;
	public GAui m_Page0 = null;
	public GAui m_Page1 = null;
	public GAui m_ArrowLeft = null;
	public GAui m_ArrowRight = null;
	public GAui m_Home = null;
	public GAui m_Play = null;
	public GAui m_Shop = null;
	public GAui m_Page_0_Dot = null;
	public GAui m_Page_1_Dot = null;

	// Buttons
	public Button m_ArrowLeftButton = null;
	public Button m_ArrowRightButton = null;

	// Pages
	private int m_CurrentPage = 0;
	//private int				m_PageCount				= 4;

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Awake is called when the script instance is being loaded.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
	void Awake()
	{
		// Set GSui.Instance.m_AutoAnimation to false, 
		// this will let you control all GUI Animator elements in the scene via scripts.
		if (enabled)
		{
			GSui.Instance.m_GUISpeed = 4.0f;
			GSui.Instance.m_AutoAnimation = false;
		}

		// If this class is not running on Unity Editor, the resolution will be change to 960x600px for Lanscape demo scene or 540x960px for Portrait demo scene
		if (Application.isEditor == false)
		{
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
			{
				string CurrentLevel = Application.loadedLevelName;
				if (CurrentLevel.Contains("Landscape") == true)
					Screen.SetResolution(960, 600, false);
				else
					Screen.SetResolution(540, 960, false);
			}
		}

	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// Update Loading progress bar.
		StartCoroutine(ShowLevelSelect());

		if (m_Page0.gameObject.activeSelf == false)
			m_Page0.gameObject.SetActive(true);
		if (m_Page1.gameObject.activeSelf == false)
			m_Page1.gameObject.SetActive(true);


	}

	// Update is called once per frame.
	void Update()
	{
	}

	#endregion // MonoBehaviour

	// ########################################
	// UI Responder functions
	// ########################################

	#region UI Responder

	public void Button_Home()
	{
		// Play Back button sound
		UIPT_PRO_SoundController.Instance.Play_SoundBack();

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 02 - " + OrientationName + " - Home", 1.5f);
	}

	public void Button_Play(GameObject goButton)
	{
		// Play Play button sound
		UIPT_PRO_SoundController.Instance.Play_SoundPlay();

		int ParticleIndex = Random.Range(0, 2);
		if (ParticleIndex == 0)
			UIPT_PRO_ParticleController.Instance.CreateParticle(goButton, UIPT_PRO_ParticleController.Instance.m_PrefabButton_01);
		else
			UIPT_PRO_ParticleController.Instance.CreateParticle(goButton, UIPT_PRO_ParticleController.Instance.m_PrefabButton_02);

		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 04 - " + OrientationName + " - Gameplay", 1.5f);
	}

	public void Button_Shop()
	{
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 10 - " + OrientationName + " - Shop", 1.5f);
	}

	public void Button_PreviousPage()
	{
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		if (m_CurrentPage != 0)
		{
			m_Page0.m_MoveOut.Enable = true;
			m_Page0.m_MoveIn.Enable = true;
			m_Page0.m_MoveIn.MoveFrom = GAui.ePosMove.LeftScreenEdge;
			m_Page0.m_MoveIn.Time = 1.5f;
			m_Page0.m_MoveIn.Delay = 0;
			m_Page0.Reset();                    // Reset all animations' information of before replay
			m_Page0.MoveIn(GUIAnimSystem.eGUIMove.Self);          // Play MoveIn animation
			m_Page_0_Dot.gameObject.GetComponent<Image>().color = new Color(1, 1, 0, 1);

			m_Page1.m_MoveIn.Enable = true;
			m_Page1.m_MoveOut.Enable = true;
			m_Page1.m_MoveOut.MoveTo = GAui.ePosMove.RightScreenEdge;
			m_Page1.m_MoveOut.Time = 1.5f;
			m_Page1.m_MoveOut.Delay = 0;
			m_Page1.Reset();                    // Reset all animations' information of before replay
			m_Page1.MoveOut(GUIAnimSystem.eGUIMove.Self);         // Play Move Out animation
			m_Page_1_Dot.gameObject.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
		}

		m_CurrentPage--;
		UpdateArrowButtons();

	}

	public void Button_NextPage()
	{
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		if (m_CurrentPage != 1)
		{
			m_Page1.m_MoveOut.Enable = false;
			m_Page1.m_MoveIn.Enable = true;
			m_Page1.m_MoveIn.MoveFrom = GAui.ePosMove.RightScreenEdge;
			m_Page1.m_MoveIn.Time = 1.5f;
			m_Page1.m_MoveIn.Delay = 0;
			m_Page1.Reset();                    // Reset all animations' information of before replay
			m_Page1.MoveIn(GUIAnimSystem.eGUIMove.Self);          // Play MoveIn animation
			m_Page_0_Dot.gameObject.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 0.4f);

			m_Page0.m_MoveIn.Enable = false;
			m_Page0.m_MoveOut.Enable = true;
			m_Page0.m_MoveOut.MoveTo = GAui.ePosMove.LeftScreenEdge;
			m_Page0.m_MoveOut.Time = 1.5f;
			m_Page0.m_MoveOut.Delay = 0;
			m_Page0.Reset();                    // Reset all animations' information of before replay
			m_Page0.MoveOut(GUIAnimSystem.eGUIMove.Self);         // Play Move Out animation
			m_Page_1_Dot.gameObject.GetComponent<Image>().color = new Color(1, 1, 0, 1);
		}

		m_CurrentPage++;
		UpdateArrowButtons();
	}

	public void Button_ClearedLevel()
	{
		// Play No button sound
		UIPT_PRO_SoundController.Instance.Play_SoundNo();
	}

	public void Button_CurrentLevel(GameObject goButton)
	{
		// Play Play button sound
		UIPT_PRO_SoundController.Instance.Play_SoundPlay();

		int ParticleIndex = Random.Range(0, 2);
		if (ParticleIndex == 0)
			UIPT_PRO_ParticleController.Instance.CreateParticle(goButton, UIPT_PRO_ParticleController.Instance.m_PrefabButton_01);
		else
			UIPT_PRO_ParticleController.Instance.CreateParticle(goButton, UIPT_PRO_ParticleController.Instance.m_PrefabButton_02);

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 04 - " + OrientationName + " - Gameplay", 1.5f);
	}

	public void Button_LockedLevel()
	{
		// Play Disable button sound
		UIPT_PRO_SoundController.Instance.Play_SoundDisable();
	}

	#endregion // UI Responder

	// ########################################
	// Functions
	// ########################################

	#region Functions

	IEnumerator ShowLevelSelect()
	{
		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.25f);

		// Play MoveIn animation
		m_Banner.MoveIn();

		// Update Loading progress bar.
		StartCoroutine(ShowPage0());
	}

	IEnumerator ShowPage0()
	{
		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.5f);

		foreach (Transform child in m_Page0.transform)
		{
			GAui pGAui = child.gameObject.GetComponent<GAui>();
			if (pGAui != null)
			{
				pGAui.m_ScaleIn.Delay = Random.Range(0.0f, 1.0f);
				pGAui.m_ScaleIn.Time = Random.Range(0.5f, 1.5f);

				// Play MoveIn animation
				pGAui.MoveIn();
			}
		}

		// Play MoveIn animation
		m_Page1.MoveIn(GUIAnimSystem.eGUIMove.Children);

		// Play particles in the hierarchy of given transfrom
		GSui.Instance.PlayParticle(m_Page0.transform);

		// Update Loading progress bar.
		StartCoroutine(ShowBottomScreenButtons());
	}

	IEnumerator ShowBottomScreenButtons()
	{
		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.25f);

		m_Play.m_ScaleIn.Delay = Random.Range(0.0f, 0.5f);
		m_Play.m_ScaleIn.Time = Random.Range(0.5f, 1.0f);

		// Play MoveIn animation
		m_Play.MoveIn(GUIAnimSystem.eGUIMove.Self);

		float WaitForPlayButton = (m_Play.m_ScaleIn.Delay + m_Play.m_ScaleIn.Time) / 2.0f;

		m_Home.m_MoveIn.Delay = WaitForPlayButton + Random.Range(0.5f, 1.0f);
		m_Home.m_MoveIn.Time = WaitForPlayButton + Random.Range(0.5f, 1.0f);
		m_Home.m_FadeIn.Delay = m_Home.m_MoveIn.Delay;
		m_Home.m_FadeIn.Time = m_Home.m_MoveIn.Time;

		// Play MoveIn animation
		m_Home.MoveIn();

		m_Shop.m_MoveIn.Delay = WaitForPlayButton + Random.Range(0.5f, 1.0f);
		m_Shop.m_MoveIn.Time = WaitForPlayButton + Random.Range(0.5f, 1.0f);
		m_Shop.m_FadeIn.Delay = m_Shop.m_MoveIn.Delay;
		m_Shop.m_FadeIn.Time = m_Shop.m_MoveIn.Time;

		// Play MoveIn animation
		m_Shop.MoveIn();

		// Update Loading progress bar.
		StartCoroutine(ShowArrows());
	}

	IEnumerator ShowArrows()
	{
		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.5f);

		m_ArrowLeft.m_ScaleIn.Delay = Random.Range(0.5f, 1.0f);
		m_ArrowLeft.m_ScaleIn.Time = Random.Range(0.5f, 1.0f);

		// Play MoveIn animation
		m_ArrowLeft.MoveIn();

		m_ArrowRight.m_ScaleIn.Delay = Random.Range(0.5f, 1.0f);
		m_ArrowRight.m_ScaleIn.Time = Random.Range(0.5f, 1.0f);

		// Play MoveIn animation
		m_ArrowRight.MoveIn();

		// Play MoveIn animations
		m_Page_0_Dot.MoveIn();
		m_Page_1_Dot.MoveIn();

		UpdateArrowButtons();
	}

	void UpdateArrowButtons()
	{
		// Limit page to 0 and 1
		if (m_CurrentPage <= 0)
		{
			m_CurrentPage = 0;

			// Disable Left Arrow button
			m_ArrowLeftButton.enabled = false;
			m_ArrowLeftButton.interactable = false;
			SetImageColor(m_ArrowLeftButton.gameObject, new Color(0.5f, 0.5f, 0.5f, 0.5f));

			// Enable Right Arrow button
			m_ArrowRightButton.enabled = true;
			m_ArrowRightButton.interactable = true;
			SetImageColor(m_ArrowRightButton.gameObject, new Color(1.0f, 1.0f, 1.0f, 1.0f));
		}
		else if (m_CurrentPage >= 1)
		{
			m_CurrentPage = 1;

			// Enable Left Arrow button
			m_ArrowLeftButton.enabled = true;
			m_ArrowLeftButton.interactable = true;
			SetImageColor(m_ArrowLeftButton.gameObject, new Color(1.0f, 1.0f, 1.0f, 1.0f));

			// Disable Right Arrow button
			m_ArrowRightButton.enabled = false;
			m_ArrowRightButton.interactable = false;
			SetImageColor(m_ArrowRightButton.gameObject, new Color(0.5f, 0.5f, 0.5f, 0.5f));
		}
	}

	// Change color of Image UI
	void SetImageColor(GameObject gObj, Color Color)
	{
		Image pImage = gObj.GetComponent<Image>();
		if (pImage != null)
		{
			pImage.color = Color;
		}

		foreach (Transform child in gObj.transform)
		{
			SetImageColor(child.gameObject, Color);
		}
	}

	#endregion // Functions
}
