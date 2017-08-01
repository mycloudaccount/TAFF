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
// UIPT_PRO_Demo05_Ranks class
//
// Handles "Demo 05 - Landscape - Ranks" and "Demo 05 - Portrait - Ranks" scenes.
// ######################################################################

public class UIPT_PRO_Demo05_Ranks : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Tabs
	public Image m_ImageButtonFriends = null;
	public Image m_ImageButtonAllPlayers = null;

	// Texts
	public Text m_TextHeader = null;
	public Text m_TextButtonFriends = null;
	public Text m_TextButtonAllPlayers = null;

	// Scroll
	public Scrollbar m_Scrollbar = null;

	// Content
	public Image m_PanelContent = null;

	// Prefab Sprite
	public Sprite[] m_PrefabPortraits = null;

	// Infos
	public UIPT_PRO_Demo_RankInfo[] m_RankItems = null;

	// Ranks
	[System.Serializable]           // Embed this class with sub properties in the inspector. http://docs.unity3d.com/ScriptReference/Serializable.html
	public class Rank
	{
		public string m_Name;
		public int m_Score;
		[HideInInspector]
		public Sprite m_PortraitSprite;
	}
	public Rank[] m_RankOfFriends = null;
	public Rank[] m_RankOfAllPlayers = null;

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
		// Random Players' portrait
		int RandNumOld = -1;
		int RandNumNew = -1;
		for (int i = 0; i < m_RankOfFriends.Length; i++)
		{
			while (RandNumOld == RandNumNew)
			{
				RandNumNew = Random.Range(0, m_PrefabPortraits.Length - 1);
			}
			RandNumOld = RandNumNew;
			m_RankOfFriends[i].m_PortraitSprite = m_PrefabPortraits[RandNumNew];
		}
		for (int i = 0; i < m_RankOfAllPlayers.Length; i++)
		{
			while (RandNumOld == RandNumNew)
			{
				RandNumNew = Random.Range(0, m_PrefabPortraits.Length - 1);
			}
			RandNumOld = RandNumNew;
			m_RankOfAllPlayers[i].m_PortraitSprite = m_PrefabPortraits[RandNumNew];
		}

		SwitchTab(0);

		// Play MoveIn animation
		GSui.Instance.MoveIn(this.transform, true);
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

	public void Button_Friends()
	{
		// Play Tap button sound
		UIPT_PRO_SoundController.Instance.Play_SoundTap();

		SwitchTab(0);
	}

	public void Button_AllPlayers()
	{
		// Play Tap button sound
		UIPT_PRO_SoundController.Instance.Play_SoundTap();

		SwitchTab(1);
	}

	public void Scrollbar_ValueChanged()
	{
		string CurrentLevel = Application.loadedLevelName;

		// Lanscape demo scene
		if (CurrentLevel.Contains("Landscape") == true)
		{
			RectTransform pRectTransform = m_PanelContent.transform.GetComponent<RectTransform>();
			if (pRectTransform != null)
				pRectTransform.anchoredPosition = new Vector3(0, 2240.0f * m_Scrollbar.value, 0);
		}
		// Portrait demo scene
		else
		{
			RectTransform pRectTransform = m_PanelContent.transform.GetComponent<RectTransform>();
			if (pRectTransform != null)
				pRectTransform.anchoredPosition = new Vector3(0, 1680.0f * m_Scrollbar.value, 0);
		}
	}

	#endregion // UI Responder

	// ########################################
	// Functions
	// ########################################

	#region Functions

	void SwitchTab(int index)
	{
		if (index == 0)
		{
			m_ImageButtonFriends.color = new Color(1, 1, 1, 1);
			m_TextButtonFriends.color = new Color(1, 1, 1, 1);

			m_ImageButtonAllPlayers.color = new Color(0.75f, 0.75f, 0.75f, 1);
			m_TextButtonAllPlayers.color = new Color(0.0f, 0.0f, 0.0f, 1);

			m_TextHeader.text = "Top 10 of 256 friends";

			UpdateItemList(0);
		}
		if (index == 1)
		{
			m_ImageButtonFriends.color = new Color(0.75f, 0.75f, 0.75f, 1);
			m_TextButtonFriends.color = new Color(0.0f, 0.0f, 0.0f, 1);

			m_ImageButtonAllPlayers.color = new Color(1, 1, 1, 1);
			m_TextButtonAllPlayers.color = new Color(1, 1, 1, 1);

			m_TextHeader.text = "12,525,980 players";

			UpdateItemList(1);
		}
	}

	void UpdateItemList(int index)
	{
		if (index == 0)
		{
			for (int i = 0; i < m_RankItems.Length; i++)
			{
				if (i < m_RankOfFriends.Length)
				{
					// Set information to current rank.
					// Note this function have to be called anytime after BindGameObjects is called.
					m_RankItems[i].SetInfo((i + 1).ToString(),
						m_RankOfFriends[i].m_Name,
						string.Format("{0:n0}", m_RankOfFriends[i].m_Score),
						m_RankOfFriends[i].m_PortraitSprite);
				}
			}
		}
		else if (index == 1)
		{
			for (int i = 0; i < m_RankItems.Length; i++)
			{
				if (i < m_RankOfAllPlayers.Length)
				{
					// Set information to current rank.
					// Note this function have to be called anytime after BindGameObjects is called.
					m_RankItems[i].SetInfo((i + 1).ToString(),
						m_RankOfAllPlayers[i].m_Name,
						string.Format("{0:n0}", m_RankOfAllPlayers[i].m_Score),
						m_RankOfAllPlayers[i].m_PortraitSprite);
				}
			}
		}
	}

	#endregion // Functions
}
