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
// UIPT_PRO_Demo_Settings class
//
// Show/hide Settings Canvas and respond to user inputs.
//
// Settings Canvas is used in "Demo 02 - Landscape - Home", "Demo 02 - Portrait - Home" demo scenes, "Demo 04 - Landscape - Gameplay" and "Demo 04 - Portrait - Gameplay"
// ######################################################################

public class UIPT_PRO_Demo_Settings : UIPT_PRO_Demo_GUIPanel
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Sliders
	public Slider m_Music = null;
	public Slider m_Sound = null;
	public Slider m_Vibration = null;

	// Toggles
	public Toggle m_AutoUpdate = null;
	public Toggle m_Notifications = null;
	public Toggle m_GraphicQualityHigh = null;
	public Toggle m_GraphicQualityMed = null;
	public Toggle m_GraphicQualityLow = null;

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
		// Load configs from PlayerPrefs
		UIPT_PRO_Demo_Config.Instance.Load();

		// Update UIs using variables in UIPT_PRO_Demo_Config
		UpdateUIs();
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

	public void Slider_Music()
	{
		UIPT_PRO_Demo_Config.Instance.m_Music = m_Music.value;

		UIPT_PRO_SoundController.Instance.SetMusicVolume(m_Music.value);
	}

	public void Slider_Sound()
	{
		UIPT_PRO_Demo_Config.Instance.m_Sound = m_Sound.value;

		UIPT_PRO_SoundController.Instance.SetSoundVolume(m_Sound.value);

		// Play Yes sound
		UIPT_PRO_SoundController.Instance.Play_SoundYes();
	}

	public void Button_DecreaseMusic()
	{
		// Minus 0.1 to Music volume
		m_Music.value -= 0.1f;
		if (m_Music.value < 0)
			m_Music.value = 0.0f;
	}

	public void Button_IncreaseMusic()
	{
		// Plus 0.1 to Music volume
		m_Music.value += 0.1f;
		if (m_Music.value > 1.0f)
			m_Music.value = 1.0f;
	}

	public void Button_DecreaseSound()
	{
		// Minus 0.1 to Sound volume
		m_Sound.value -= 0.1f;
		if (m_Sound.value < 0)
			m_Sound.value = 0.0f;
	}

	public void Button_IncreaseSound()
	{
		// Plus 0.1 to Sound volume
		m_Sound.value += 0.1f;
		if (m_Sound.value > 1.0f)
			m_Sound.value = 1.0f;
	}

	public void Slider_Vibration()
	{
		if (m_Vibration.value > 0)
		{
			UIPT_PRO_Demo_Config.Instance.m_Vibration = true;
		}
		else
		{
			UIPT_PRO_Demo_Config.Instance.m_Vibration = false;
		}

		// Play Tap button sound
		UIPT_PRO_SoundController.Instance.Play_SoundTap();
	}

	public void Toggle_AutoUpdate()
	{
		UIPT_PRO_Demo_Config.Instance.m_AutoUpdate = m_AutoUpdate.isOn;

		if (UIPT_PRO_Demo_Config.Instance.m_AutoUpdate)
		{
			// Play Yes button sound
			UIPT_PRO_SoundController.Instance.Play_SoundYes();
		}
		else
		{
			// Play No button sound
			UIPT_PRO_SoundController.Instance.Play_SoundNo();
		}
	}

	public void Toggle_Notification()
	{
		UIPT_PRO_Demo_Config.Instance.m_Notifications = m_Notifications.isOn;

		if (UIPT_PRO_Demo_Config.Instance.m_Notifications)
		{
			// Play Yes button sound
			UIPT_PRO_SoundController.Instance.Play_SoundYes();
		}
		else
		{
			// Play No button sound
			UIPT_PRO_SoundController.Instance.Play_SoundNo();
		}
	}

	public void Toggle_GraphicHigh()
	{
		if (m_GraphicQualityHigh.isOn == true)
		{
			UIPT_PRO_Demo_Config.Instance.m_Quality = UIPT_PRO_Demo_Config.eGraphicQuality.High;
			UIPT_PRO_SoundController.Instance.Play_SoundYes();
		}
	}

	public void Toggle_GraphicMed()
	{
		if (m_GraphicQualityMed.isOn == true)
		{
			UIPT_PRO_Demo_Config.Instance.m_Quality = UIPT_PRO_Demo_Config.eGraphicQuality.Medium;
			UIPT_PRO_SoundController.Instance.Play_SoundYes();
		}
	}

	public void Toggle_GraphicLow()
	{
		if (m_GraphicQualityLow.isOn == true)
		{
			UIPT_PRO_Demo_Config.Instance.m_Quality = UIPT_PRO_Demo_Config.eGraphicQuality.Low;
			UIPT_PRO_SoundController.Instance.Play_SoundYes();
		}
	}

	public void Button_ResetToDefault()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		UIPT_PRO_Demo_Config.Instance.LoadDefault();

		// Update UIs using variables in UIPT_PRO_Demo_Config
		UpdateUIs();
	}

	public void Button_FacebookLogin()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();
	}

	public void Button_Close()
	{
		// Play Back button sound
		UIPT_PRO_SoundController.Instance.Play_SoundBack();

		// Save configs from PlayerPrefs
		UIPT_PRO_Demo_Config.Instance.Save();

		// Hide this panel
		Hide();
	}

	#endregion // UI Responder

	// ########################################
	// Functions
	// ########################################

	#region UI functions

	// Update UIs using variables in UIPT_PRO_Demo_Config
	void UpdateUIs()
	{
		m_Music.value = UIPT_PRO_Demo_Config.Instance.m_Music;

		m_Sound.value = UIPT_PRO_Demo_Config.Instance.m_Sound;

		if (UIPT_PRO_Demo_Config.Instance.m_Vibration == true)
		{
			m_Vibration.value = 1;
		}
		else
		{
			m_Vibration.value = 0;
		}

		m_AutoUpdate.isOn = UIPT_PRO_Demo_Config.Instance.m_AutoUpdate;

		m_Notifications.isOn = UIPT_PRO_Demo_Config.Instance.m_Notifications;

		if (UIPT_PRO_Demo_Config.Instance.m_Quality == UIPT_PRO_Demo_Config.eGraphicQuality.High)
			m_GraphicQualityHigh.isOn = true;
		else if (UIPT_PRO_Demo_Config.Instance.m_Quality == UIPT_PRO_Demo_Config.eGraphicQuality.Medium)
			m_GraphicQualityMed.isOn = true;
		else if (UIPT_PRO_Demo_Config.Instance.m_Quality == UIPT_PRO_Demo_Config.eGraphicQuality.Low)
			m_GraphicQualityLow.isOn = true;
	}

	#endregion // Functions
}
