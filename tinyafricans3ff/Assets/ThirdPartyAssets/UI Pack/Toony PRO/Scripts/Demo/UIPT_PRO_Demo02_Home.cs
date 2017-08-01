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
// UIPT_PRO_Demo02_Home class
//
// Handles "Demo 02 - Landscape - Home" and "Demo 02 - Portrait - Home" scenes.
// ######################################################################

public class UIPT_PRO_Demo02_Home : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// UIPT_PRO_Demo_Settings
	public UIPT_PRO_Demo_Settings m_Settings = null;

	// UIPT_PRO_Demo_News
	public UIPT_PRO_Demo_News m_News = null;

	// UIPT_PRO_Demo_Help
	public UIPT_PRO_Demo_Help m_Help = null;

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour functions

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
		if (m_Help.gameObject.activeSelf == false)
			m_Help.gameObject.SetActive(true);

		if (m_News.gameObject.activeSelf == false)
			m_News.gameObject.SetActive(true);

		if (m_Settings.gameObject.activeSelf == false)
			m_Settings.gameObject.SetActive(true);

	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// Update Loading progress bar.
		StartCoroutine(Show());
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

	public void Button_Help()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Show this panel
		m_Help.Show();
	}

	public void Button_Credits()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 09 - " + OrientationName + " - Credits", 1.5f);
	}

	public void Button_Social()
	{
		// Play Click sound
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
		GSui.Instance.LoadLevel("ToonyPRO Demo 07 - " + OrientationName + " - Social", 1.5f);
	}

	public void Button_News()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Show this panel
		m_News.Show();
	}

	public void Button_CheckoutGUIAnimator()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// http://docs.unity3d.com/ScriptReference/Application.ExternalEval.html
		//Application.ExternalEval("window.open('https://www.assetstore.unity3d.com/en/#!/content/28709','GUI Animator for Unity UI')");

        // http://docs.unity3d.com/ScriptReference/Application.OpenURL.html
        Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/28709");
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

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 03 - " + OrientationName + " - Level Select", 1.5f);
	}

	public void Button_Settings()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Show this panel
		m_Settings.Show();
	}

	public void Button_Ranks()
	{
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 05 - " + OrientationName + " - Ranks", 1.5f);
	}

	public void Button_Achievement()
	{
		// Play Click sound
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
		GSui.Instance.LoadLevel("ToonyPRO Demo 06 - " + OrientationName + " - Achievements", 1.5f);
	}

	public void Button_More()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 08 - " + OrientationName + " - More Samples", 1.5f);
	}

	public void Button_Shop()
	{
		// Play Click sound
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

	#endregion // UI Responder

	// ########################################
	// Functions
	// ########################################

	#region Functions

	IEnumerator Show()
	{
		// Play MoveIn animation
		GSui.Instance.MoveIn(this.transform, true);

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(1.0f);

		// Play particles in the hierarchy of given transfrom
		GSui.Instance.PlayParticle(this.transform);
	}

	#endregion // Functions
}
