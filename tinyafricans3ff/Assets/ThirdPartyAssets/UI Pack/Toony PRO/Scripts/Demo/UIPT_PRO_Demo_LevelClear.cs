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
// UIPT_PRO_Demo_GUIPanel class
//
// Handles Level Clear canvas.
// It is used in "Demo 04 - Landscape - Gameplay" and "Demo 04 - Portrait - Gameplay"
// ######################################################################


public class UIPT_PRO_Demo_LevelClear : UIPT_PRO_Demo_GUIPanel
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Texts
	public Text m_Text_Score = null;
	public Text m_Text_BestScore = null;
	public Text m_Text_Time = null;
	public Text m_Text_Coin = null;

	// GAuis	
	public GAui m_BestScore = null;
	public GAui m_Star1 = null;
	public GAui m_Star2 = null;
	public GAui m_Star3 = null;

	// Toggles
	bool m_ReportScore = false;
	bool m_ReportCoin = false;

	// Info
	private int m_Score = 0;
	private float m_RemainingTime = 0;
	private int m_Coin = 0;

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
		// Reset all Texts
		m_Text_Score.text = "0";
		m_Text_BestScore.text = "0";
		m_Text_Time.text = "0";
		m_Text_Coin.text = "0";

		// Stop particles in the hierarchy of given transfrom
		GSui.Instance.StopParticle(this.transform);
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{
		// Count-up Score Text
		if (m_ReportScore == true)
		{
			int Score = int.Parse(m_Text_Score.text);
			if (Score < m_Score)
			{
				int AddScore = (m_Score - Score) / 4;
				if (AddScore <= 0) AddScore = 1;
				Score += AddScore;
				if (Score > m_Score)
					Score = m_Score;
				m_Text_Score.text = Score.ToString();
			}
			else if (Score > m_Score)
			{
				Score -= 1;
				if (Score < m_Score)
					Score = m_Score;
				m_Text_Score.text = Score.ToString();
			}
		}

		// Count-up Coin Text
		if (m_ReportCoin == true)
		{
			int Coin = int.Parse(m_Text_Coin.text.Replace(",", ""));
			if (Coin < m_Coin)
			{
				int AddCoin = (m_Coin - Coin) / 4;
				if (AddCoin <= 0) AddCoin = 1;
				Coin += AddCoin;
				if (Coin > m_Coin)
					Coin = m_Coin;
				m_Text_Coin.text = string.Format("{0:n0}", Coin);
			}
		}
	}

	#endregion // MonoBehaviour

	// ########################################
	// UI Responder functions
	// ########################################

	#region UI Responder

	public void Button_Home()
	{
		// Hide this panel
		Hide();

		// Play Back button sound
		UIPT_PRO_SoundController.Instance.Play_SoundBack();

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 02 - " + OrientationName + " - Home", 1.5f);
	}

	public void Button_Play()
	{
		// Hide this panel
		Hide();

		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 03 - " + OrientationName + " - Level Select", 1.5f);
	}

	public void Button_Replay()
	{
		// Play Click sound
		UIPT_PRO_SoundController.Instance.Play_SoundClick();

		// Keep particles stay alive until it finished playing.
		GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = Application.loadedLevelName;
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GSui.Instance.LoadLevel("ToonyPRO Demo 04 - " + OrientationName + " - Gameplay", 1.5f);
	}

	#endregion // UI Responder

	// ########################################
	// Functions
	// ########################################

	#region Functions

	public void Show(int Score, float RemainingTime, int Coin)
	{
		// Update some Text objects
		m_Score = Score;
		m_RemainingTime = RemainingTime;
		m_Coin = Coin;

		// Begin report Text animations
		StartCoroutine(Report());

		// Show this panel
		this.gameObject.GetComponent<UIPT_PRO_Demo_GUIPanel>().Show();
	}

	// Hide this panel
	public void HideMe()
	{
		// Hide this panel
		this.gameObject.GetComponent<UIPT_PRO_Demo_GUIPanel>().Hide();
	}

	IEnumerator Report()
	{
		// Random a best score
		int BestScore = m_Score - (Random.Range(1, 10) * 100);
		if (BestScore < 0)
			BestScore = 0;

		// Set Texts
		m_Text_BestScore.text = BestScore.ToString();
		m_Text_Time.text = string.Format("{00:00}", (int)m_RemainingTime / 60) + ":" + string.Format("{00:00}", (int)m_RemainingTime % 60);

		// Disable all Star animation
		m_Star1.enabled = false;
		m_Star2.enabled = false;
		m_Star3.enabled = false;
		m_BestScore.enabled = false;

		// Play particles in the hierarchy of given transfrom
		GSui.Instance.PlayParticle(this.transform);

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(1.0f);

		// Start count up number for m_Text_Score
		m_ReportScore = true;

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(1.0f);

		// Start count up number for m_Text_Coin
		m_ReportCoin = true;

		// Start m_Star1 MoveIn animation
		m_Star1.enabled = true;
		m_Star1.MoveIn();           // Play MoveIn animation

		// Start m_Star2 MoveIn animation
		m_Star2.enabled = true;
		m_Star2.MoveIn();           // Play MoveIn animation

		// Start m_Star3 MoveIn animation
		m_Star3.enabled = true;
		m_Star3.MoveIn();           // Play MoveIn animation

		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.5f);

		// Start m_BestScore MoveIn animation
		m_BestScore.enabled = true;
		m_BestScore.MoveIn();           // Play MoveIn animation
	}

	#endregion // Functions
}
