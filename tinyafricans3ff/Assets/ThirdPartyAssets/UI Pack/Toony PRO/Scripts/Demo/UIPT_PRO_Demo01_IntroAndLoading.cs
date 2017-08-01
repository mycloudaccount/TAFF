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

#endregion

// ######################################################################
// UIPT_PRO_Demo01_IntroAndLoading class
//
// Handles "Demo 01 - Landscape - Intro & Loading" and "Demo 01 - Portrait - Intro & Loading" scenes.
// ######################################################################

public class UIPT_PRO_Demo01_IntroAndLoading : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Intro
	public GameObject m_Intro = null;

	// Loading
	public GameObject m_Loading = null;
	public Slider m_Slider = null;

	// Time
	public float m_ShowLogoTime = 2.0f;
	public float m_ShowLoadingTime = 1.0f;
	public float m_IdleLoadingTime = 1.0f;

	// Loading Progress
	private AsyncOperation m_Async = null;

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

		// Deactivates m_Intro and m_Loading GameObjects.
		m_Intro.SetActive(false);
		m_Loading.SetActive(false);
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// Update Loading progress bar.
		StartCoroutine(ShowLogo());

		// Reset Loading progress bar to zero.
		m_Slider.value = 0.0f;
	}

	// Update is called once per frame.
	void Update()
	{
		// Update Loading progress bar.
		if (m_Async != null)
		{
			// Update Loading progress bar.
			if (m_Async.progress < 0.9f)
				m_Slider.value = m_Async.progress;
			else
				m_Slider.value = 1.0f;
		}
	}

	#endregion // MonoBehaviour

	// ########################################
	// Functions
	// ########################################

	#region Functions

	IEnumerator ShowLogo()
	{
		// Activate m_Intro GameObject then animate MoveIn animation.
		m_Intro.gameObject.SetActive(true);

		// Play MoveIn animation
		GSui.Instance.MoveIn(m_Intro.transform, true);

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(m_ShowLogoTime);

		// Start ShowLoading() coroutine.
		StartCoroutine(ShowLoading());
	}

	IEnumerator ShowLoading()
	{
		// Play Move Out animation
		GSui.Instance.MoveOut(m_Intro.transform, true);

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(m_ShowLoadingTime);

		// Start IdleLoading() coroutine.
		StartCoroutine(IdleLoading());
	}

	IEnumerator IdleLoading()
	{
		// Activate m_Loading GameObject then animate MoveIn animation.
		m_Loading.SetActive(true);

		// Play MoveIn animation
		GSui.Instance.MoveIn(m_Loading.transform, true);

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(m_IdleLoadingTime);

		// Start LoadNextScene() coroutine.
		if (Application.HasProLicense())
		{
			// NOTE: Asynchronous Background loading is only supported in Unity Pro.
			StartCoroutine(LoadNextSceneAsync());
		}
		else
		{
			// NOTE: Asynchronous Background loading is only supported in Unity Pro.
			LoadNextScene();
		}
	}

	IEnumerator LoadNextSceneAsync()
	{
		// Reset Loading progress bar to zero.
		m_Slider.value = 0.0f;

		// Load next scene asynchronously in the background.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";

		// NOTE: Asynchronous Background loading is only supported in Unity Pro.
		m_Async = Application.LoadLevelAsync("ToonyPRO Demo 02 - " + OrientationName + " - Home");

		yield return m_Async;
	}

	void LoadNextScene()
	{
		// Reset Loading progress bar to zero.
		m_Slider.value = 0.0f;

		// Load next scene asynchronously in the background.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";

		// NOTE: Asynchronous Background loading is only supported in Unity Pro.
		// Use Application.LoadLevel or Application.LoadLevelAdditive instead of LoadLevelAsync.
		Application.LoadLevel("ToonyPRO Demo 02 - " + OrientationName + " - Home");
	}

	#endregion // Functions
}
