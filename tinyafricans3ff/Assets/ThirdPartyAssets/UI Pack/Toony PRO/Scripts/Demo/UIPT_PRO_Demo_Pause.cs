// UI Pack : Toony PRO
// Version: 1.1.1
// Compatilble: Unity 4.7.1 and Unity 5.3.4 or higher, more info in Readme.txt file.
//
// Author:	Gold Experience Team (http://www.ge-team.com)
// Details:	https://www.assetstore.unity3d.com/en/#!/content/44103
// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.

using UnityEngine;

using System.Collections;

#region Namespaces
using UnityEngine.UI;

#endregion // Namespaces

// ######################################################################
// UIPT_PRO_Demo_Pause class
//
// Show/hide Pause Canvas and respond to user inputs.
//
// Pause Canvas is used in "Demo 02 - Landscape - Home", "Demo 02 - Portrait - Home" demo scenes, "Demo 04 - Landscape - Gameplay" and "Demo 04 - Portrait - Gameplay" scenes.
// ######################################################################

public class UIPT_PRO_Demo_Pause : UIPT_PRO_Demo_GUIPanel
{

	#region Variables

	public UIPT_PRO_Demo_Settings m_Settings = null;            // Keep UIPT_PRO_Demo_Settings class to show it when user presses Settings button

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

		// Activate all UI Canves GameObjects.
		if (m_Settings.gameObject.activeSelf == false)
			m_Settings.gameObject.SetActive(true);
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{
	}

	#endregion // MonoBehaviour

	// ########################################
	// UI Responder functions
	// ########################################

	#region UI Responder

	public void Button_Resume()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Hide this panel
		Hide();
	}

	public void Button_Settings()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Show this panel
		m_Settings.Show();
	}

	public void Button_Levels()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Resume everything
		Time.timeScale = 1.0f;

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 03 - " + OrientationName + " - Level Select", 1.0f);
	}

	#endregion // UI Responder

	// ########################################
	// Functions
	// ########################################

	#region Functions

	// Show this panel
	public void ShowMe()
	{
		// Pause everything
		Time.timeScale = 0.0f;

		// Show this panel
		this.gameObject.GetComponent<UIPT_PRO_Demo_GUIPanel>().Show();
	}

	// Hide this panel
	public void HideMe()
	{
		// Resume everything
		Time.timeScale = 1.0f;

		// Hide this panel
		this.gameObject.GetComponent<UIPT_PRO_Demo_GUIPanel>().Hide();
	}

	#endregion //UI functions
}
