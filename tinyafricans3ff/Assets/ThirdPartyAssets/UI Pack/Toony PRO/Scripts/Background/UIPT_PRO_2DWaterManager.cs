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
// UIPT_PRO_2DWaterManager class
//
// This class moves water sprite on background.
// ######################################################################

public class UIPT_PRO_2DWaterManager : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	// This class describes information of each water sprite
	[System.Serializable]           // Embed this class with sub properties in the inspector. http://docs.unity3d.com/ScriptReference/Serializable.html
	public class UIPackToony2DWater
	{
		public float m_MoveSpeed;           // Move speed of water
		public GameObject m_Water;          // Handle of water's gameObject
		public RectTransform m_RectTransform; // Handle of water's gameObject
		public Vector2 m_OriginalLocalPos;          // LocalPosition before first Update() is called
	}

	public GameObject m_Water = null;           // Original water sprite

	//[HideInInspector]									// Remark this line if you want to see each water's details
	public UIPackToony2DWater[] m_WaterList = null;         // Array of sprites

	public float m_Speed = 1.00f;                       // Move Speed

	public bool m_ShowAtbottom = false;                 // if true all sprites will be set at the bottom of screen

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region Monobehavior

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		InitMe();
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{
		// Check if the screen resolution was changed
		if (m_CameraRightEdge != m_ParentCanvasRectTransform.rect.width / 2 || m_CameraTopEdge != m_ParentCanvasRectTransform.rect.height / 2)
		{
			// Re-initial all water sprites
			InitMe();
		}

		// Update all sprite's position
		for (int index = 0; index < m_WaterList.Length; index++)
		{
			if (m_WaterList[index].m_Water.activeSelf == true)
			{
				// Move sprite left/right according to m_MoveSpeed
				m_WaterList[index].m_Water.transform.localPosition = new Vector3(m_WaterList[index].m_Water.transform.localPosition.x + (m_WaterList[index].m_MoveSpeed * Time.deltaTime),
																			m_WaterList[index].m_Water.transform.localPosition.y,
																			m_WaterList[index].m_Water.transform.localPosition.z);
			}
		}

		// Bring off-screen sprite back to screen
		for (int index = 0; index < m_WaterList.Length; index++)
		{
			if (m_WaterList[index].m_Water.activeSelf == true)
			{
				// Moving left to right
				if (m_WaterList[index].m_MoveSpeed > 0)
				{
					// Check if sprite move off-screen of the rigth-edge
					if (m_WaterList[index].m_Water.transform.localPosition.x > m_CameraRightEdge + m_WaterList[index].m_RectTransform.rect.width / 2)
					{
						// Find most-left sprite
						float MostLeftPos = m_CameraRightEdge;
						int MostLeftPosIndex = -1;
						for (int i = 0; i < m_WaterList.Length; i++)
						{
							if (m_WaterList[i].m_Water.transform.localPosition.x < MostLeftPos)
							{
								MostLeftPosIndex = i;
								MostLeftPos = m_WaterList[i].m_Water.transform.localPosition.x;
							}
						}

						// Move off-screen water to next to most-left sprite
						m_WaterList[index].m_Water.transform.localPosition = new Vector3(m_WaterList[MostLeftPosIndex].m_Water.transform.localPosition.x - m_WaterList[index].m_RectTransform.rect.width,
																					m_WaterList[index].m_Water.transform.localPosition.y);
					}
				}
				// Moving right to left
				else
				{
					// Check if sprite move off-screen of the left-edge
					if (m_WaterList[index].m_Water.transform.localPosition.x < m_CameraLeftEdge - m_WaterList[index].m_RectTransform.rect.width / 2)
					{
						// Find most-right sprite
						int MostRightIndex = -1;
						float MostRightPos = m_CameraLeftEdge;
						for (int i = 0; i < m_WaterList.Length; i++)
						{
							if (m_WaterList[i].m_Water.transform.localPosition.x > MostRightPos)
							{
								MostRightIndex = i;
								MostRightPos = m_WaterList[i].m_Water.transform.localPosition.x;
							}
						}

						// Move off-screen water to next to most-right sprite
						m_WaterList[index].m_Water.transform.localPosition = new Vector3(m_WaterList[MostRightIndex].m_Water.transform.localPosition.x + m_WaterList[index].m_RectTransform.rect.width,
																					m_WaterList[index].m_Water.transform.localPosition.y);

						break;
					}
				}

			}
		}
	}

	#endregion // Monobehavior

	// ########################################
	// Utilities
	// ########################################

	#region Utilities

	// This class need Canvas to work properly.
	Canvas m_Parent_Canvas = null;

	// Edge position of camera perspective
	float m_CameraLeftEdge;
	float m_CameraRightEdge;
	float m_CameraTopEdge;
	float m_CameraBottomEdge;

	// If you have error at this line on Unity 5.x, please make sure that you are using Unity 5.x with a valid license.
	RectTransform m_ParentCanvasRectTransform = null;

	// Search for parent Canvas and calculate view size of camera 
	void FindParentCanvasAndCameraArea()
	{
		// Search for the parent Canvas
		if (m_Parent_Canvas == null)
			m_Parent_Canvas = GSui.Instance.GetParent_Canvas(transform);

		// Calculate view size of camera 
		if (m_Parent_Canvas != null)
		{
			m_ParentCanvasRectTransform = m_Parent_Canvas.GetComponent<RectTransform>();

			m_CameraRightEdge = (m_ParentCanvasRectTransform.rect.width / 2);
			m_CameraLeftEdge = -m_CameraRightEdge;
			m_CameraTopEdge = (m_ParentCanvasRectTransform.rect.height / 2);
			m_CameraBottomEdge = -m_CameraTopEdge;
		}
	}

	// Initial sprite and put them to the array
	void InitMe()
	{
		// Search for parent Canvas and calculate camera view size
		FindParentCanvasAndCameraArea();

		// Check if there is child object
		if (transform.childCount > 0)
		{
			// Destroy all children objects
			foreach (Transform child in transform)
			{
				if (child.gameObject != m_Water)
					Destroy(child.gameObject);
			}

			// Enable original water sprite
			m_Water.GetComponent<Image>().enabled = true;

			// if there is no RectTransform then destroy it
			RectTransform pRectTransform = m_Water.GetComponent<RectTransform>();
			if (pRectTransform == null)
			{
				DestroyObject(m_Water);
				return;
			}

			// Keep width of sprite
			float Width = pRectTransform.rect.width;

			if (Width > 0)
			{
				// How many sprites to fill along the horizontal of screen
				int MaxWaterCount = (int)((m_CameraRightEdge - m_CameraLeftEdge) / Width) + 3;

				// Init array of UIPackToony2DWater
				m_WaterList = new UIPackToony2DWater[MaxWaterCount];

				// Put all sprites to m_WaterList
				for (int i = 0; i < MaxWaterCount; i++)
				{
					// Create new UIPackToony2DWater class
					m_WaterList[i] = new UIPackToony2DWater();

					// Instantiate new game object and set its parent
					GameObject pGameObject = (GameObject)GameObject.Instantiate(m_Water);
					pGameObject.name = "Water_" + string.Format("{0:00}", i);
					pGameObject.transform.SetParent(m_Water.transform.parent);

					// Set UIPackToony2DWater's variables
					m_WaterList[i].m_Water = pGameObject;
					m_WaterList[i].m_RectTransform = pGameObject.GetComponent<RectTransform>();
					m_WaterList[i].m_RectTransform.localScale = m_Water.GetComponent<RectTransform>().localScale;
					m_WaterList[i].m_RectTransform.anchoredPosition3D = m_Water.GetComponent<RectTransform>().anchoredPosition3D;

					// Move sprite to bottom of screen
					float BottomPos = m_CameraBottomEdge;
					if (m_ShowAtbottom)
					{
						BottomPos = m_CameraBottomEdge + (m_WaterList[i].m_RectTransform.rect.height / 2.0f);
					}
					m_WaterList[i].m_RectTransform.anchoredPosition = new Vector2((m_CameraLeftEdge - m_WaterList[i].m_RectTransform.rect.width) + (i * m_WaterList[i].m_RectTransform.rect.width), BottomPos);

					// Random speed
					m_WaterList[i].m_MoveSpeed = m_Speed;

					// Keep original anchoredPosition to use later
					m_WaterList[i].m_OriginalLocalPos = m_WaterList[i].m_RectTransform.anchoredPosition;
				}
			}

			// Disable original water sprite, we just finised using it for creating water sprite array
			m_Water.GetComponent<Image>().enabled = false;
		}
	}

	#endregion // Utilities
}