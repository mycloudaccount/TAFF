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
// UIPT_PRO_Demo08_MoreSamples class
//
// Handles "Demo 08 - Landscape - More Samples" and "Demo 08 - Portrait - More Samples" scenes.
// ######################################################################

public class UIPT_PRO_Demo08_MoreSamples : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// GameObject
	public GameObject m_PanelBottom = null;

	// Text
	public Text m_TextIndex = null;

	// UIs
	public GAui[] m_Samples = null;

	// Buttons
	public Button m_ArrowLeftButton = null;
	public Button m_ArrowRightButton = null;

	// Status
	public int m_SampleUI_Index = 0;
	public int m_SampleUI_IndexOld = 0;

	// Button wait time
	float m_ButtonWaitTimeCount = 0;
	float m_ButtonWaitTime = 2.0f;

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

	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// Play MoveIn animation
		GSui.Instance.MoveIn(m_PanelBottom.transform, true);

		// Stop particles in the hierarchy of given transfrom
		GSui.Instance.StopParticle(this.transform);

		StartCoroutine(ShowNextSample());
	}

	// Update is called once per frame.
	void Update()
	{
		// Count down m_ButtonWaitTimeCount
		if (m_ButtonWaitTimeCount > 0)
		{
			m_ButtonWaitTimeCount -= Time.deltaTime;

			// Enable Interact of Left/Right Arrow buttons
			if (m_ButtonWaitTimeCount <= 0)
			{
				GSui.Instance.EnableButton(m_ArrowLeftButton.transform, true);
				m_ArrowLeftButton.interactable = true;

				GSui.Instance.EnableButton(m_ArrowRightButton.transform, true);
				m_ArrowRightButton.interactable = true;
			}
			return;
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


		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(m_Samples[m_SampleUI_Index].transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(m_Samples[m_SampleUI_Index].transform, true);

		// Play Move Out animation
		GSui.Instance.MoveOut(m_PanelBottom.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 02 - " + OrientationName + " - Home", 1.0f);
	}

	public void Button_LeftArrow()
	{
		if (m_ButtonWaitTimeCount > 0)
		{
			return;
		}

		m_ButtonWaitTimeCount = m_ButtonWaitTime;
		GSui.Instance.EnableButton(m_ArrowLeftButton.transform, false);
		m_ArrowLeftButton.interactable = false;
		GSui.Instance.EnableButton(m_ArrowRightButton.transform, false);
		m_ArrowRightButton.interactable = false;

		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		m_SampleUI_Index--;
		if (m_SampleUI_Index < 0)
			m_SampleUI_Index = m_Samples.Length - 1;

		if (m_SampleUI_Index != m_SampleUI_IndexOld)
		{
			StartCoroutine(ShowNextSample());
		}

		m_TextIndex.text = (m_SampleUI_Index + 1).ToString() + "/" + m_Samples.Length.ToString();
	}

	public void Button_RightArrow()
	{
		if (m_ButtonWaitTimeCount > 0)
		{
			return;
		}

		m_ButtonWaitTimeCount = m_ButtonWaitTime;
		GSui.Instance.EnableButton(m_ArrowLeftButton.transform, false);
		m_ArrowLeftButton.interactable = false;
		GSui.Instance.EnableButton(m_ArrowRightButton.transform, false);
		m_ArrowRightButton.interactable = false;

		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		m_SampleUI_Index++;
		if (m_SampleUI_Index > m_Samples.Length - 1)
			m_SampleUI_Index = 0;

		if (m_SampleUI_Index != m_SampleUI_IndexOld)
		{
			StartCoroutine(ShowNextSample());
		}

		m_TextIndex.text = (m_SampleUI_Index + 1).ToString() + "/" + m_Samples.Length.ToString();
	}

	public void Button_PlaySoundClick()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();
	}

	public void Button_AssetStore_GUIAnimatorForUnityUI()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// http://docs.unity3d.com/ScriptReference/Application.ExternalEval.html
		//Application.ExternalEval("window.open('https://www.assetstore.unity3d.com/en/#!/content/28709','GUI Animator for Unity UI')");

        // http://docs.unity3d.com/ScriptReference/Application.OpenURL.html
        Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/28709");
    }

	public void Button_AssetStore_FirstFantasyForMobile()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// http://docs.unity3d.com/ScriptReference/Application.ExternalEval.html
		//Application.ExternalEval("window.open('https://www.assetstore.unity3d.com/#!/content/10822','First Fantasy for Mobile')");

        // http://docs.unity3d.com/ScriptReference/Application.OpenURL.html
        Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/10822");
    }

	public void Button_AssetStore_FXQuest()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// http://docs.unity3d.com/ScriptReference/Application.ExternalEval.html
		//Application.ExternalEval("window.open('https://www.assetstore.unity3d.com/#!/content/21073','FX Quest')");

        // http://docs.unity3d.com/ScriptReference/Application.OpenURL.html
        Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/21073");
    }

	#endregion // UI Responder


	// ########################################
	// Functions
	// ########################################

	#region Functions

	IEnumerator ShowFirstUI()
	{
		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.5f);

		// Reset all animations' information of before replay
		m_Samples[m_SampleUI_Index].Reset();

		// Play MoveIn animation
		m_Samples[m_SampleUI_Index].MoveIn();
	}

	IEnumerator ShowNextSample()
	{
		if (m_SampleUI_IndexOld >= 0 && m_SampleUI_IndexOld <= m_Samples.Length)
		{
			m_Samples[m_SampleUI_IndexOld].MoveOut();

			// Stop particles in the hierarchy of given transfrom
			GSui.Instance.StopParticle(m_Samples[m_SampleUI_IndexOld].transform);
		}

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.5f);

		if (m_SampleUI_Index >= 0 && m_SampleUI_Index <= m_Samples.Length)
		{
			// Reset all animations' information of before replay
			m_Samples[m_SampleUI_Index].Reset();

			// Play MoveIn animation
			m_Samples[m_SampleUI_Index].MoveIn();

			// Play particles in the hierarchy of given transfrom
			GSui.Instance.PlayParticle(m_Samples[m_SampleUI_Index].transform);
		}

		m_SampleUI_IndexOld = m_SampleUI_Index;
	}

	#endregion // Functions	
}
