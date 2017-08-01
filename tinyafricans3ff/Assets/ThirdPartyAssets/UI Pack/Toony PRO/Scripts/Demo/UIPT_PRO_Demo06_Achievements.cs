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
// UIPT_PRO_Demo06_Achievements class
//
// Handles "Demo 06 - Landscape - Achievements" and "Demo 06 - Portrait - Achievements" scenes.
// ######################################################################

public class UIPT_PRO_Demo06_Achievements : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Slider
	public Slider m_Slider = null;

	// Texts
	public Text m_TextSlider = null;
	public Text m_TextSummary = null;

	// Scrollbar
	public Scrollbar m_Scrollbar = null;

	// Content
	public Image m_PanelContent = null;

	// Info
	public UIPT_PRO_Demo_Achievement[] m_AchievementItems = null;

	// Archivement
	[System.Serializable]           // Embed this class with sub properties in the inspector. http://docs.unity3d.com/ScriptReference/Serializable.html
	public class Achievement
	{
		public bool m_Completed;
		public string m_Mission;
		public int m_Point;
	}
	public Achievement[] m_Achievements = null;

	// Status
	private bool m_AnimateSlider = false;
	private int m_CompletedMission = 0;

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
		m_Slider.value = 0;
		UpdateItemList();

		m_TextSlider.text = ((int)(m_Slider.value * 1000)).ToString() + "/1000";

		// Play MoveIn animation
		GSui.Instance.MoveIn(this.transform, true);
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{
		if (m_AnimateSlider == true)
		{
			m_Slider.value += Time.deltaTime / 10.0f;

			float ArchievementsProgress = (float)m_CompletedMission / (float)m_AchievementItems.Length;
			if (m_Slider.value >= ArchievementsProgress)
			{
				m_Slider.value = ArchievementsProgress;
				m_AnimateSlider = false;
			}

			m_TextSlider.text = ((int)(m_Slider.value * 1000)).ToString() + "/1000";

		}
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

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 02 - " + OrientationName + " - Home", 1.5f);
	}

	public void Scrollbar_ValueChanged()
	{
		string CurrentLevel = Application.loadedLevelName;

		// Lanscape demo scene
		if (CurrentLevel.Contains("Landscape") == true)
		{
			RectTransform pRectTransform = m_PanelContent.transform.GetComponent<RectTransform>();
			if (pRectTransform != null)
				pRectTransform.anchoredPosition = new Vector3(0, 1450.0f * m_Scrollbar.value, 0);
		}
		// Portrait demo scene
		else
		{
			RectTransform pRectTransform = m_PanelContent.transform.GetComponent<RectTransform>();
			if (pRectTransform != null)
				pRectTransform.anchoredPosition = new Vector3(0, 1000.0f * m_Scrollbar.value, 0);
		}
	}

	#endregion  //UI Responder

	// ########################################
	// Functions
	// ########################################

	#region Functions

	void UpdateItemList()
	{
		m_CompletedMission = 0;
		for (int i = 0; i < m_AchievementItems.Length; i++)
		{
			if (i < m_Achievements.Length)
			{
				// Set information to current Achievement.
				// Note this function have to be called anytime after BindGameObjects is called.
				m_AchievementItems[i].SetInfo(
					m_Achievements[i].m_Completed,
					m_Achievements[i].m_Mission,
					m_Achievements[i].m_Point);

				if (m_Achievements[i].m_Completed)
					m_CompletedMission++;
			}
		}

		m_TextSummary.text = "Completed " + m_CompletedMission.ToString() + "/" + m_AchievementItems.Length.ToString() + " Missions";

		StartCoroutine(AnimateSlider());
	}

	IEnumerator AnimateSlider()
	{

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(1.5f);

		m_AnimateSlider = true;
	}

	#endregion // Functions
}
