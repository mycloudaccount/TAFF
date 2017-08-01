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

#endregion // Namespaces

// ######################################################################
// UIPT_PRO_SoundController class
//
// Simple Play/stop BG music and sounds.
// ######################################################################

public class UIPT_PRO_SoundController : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Private reference which can be accessed by this class only
	private static UIPT_PRO_SoundController instance;

	// Public static reference that can be accesd from anywhere
	public static UIPT_PRO_SoundController Instance
	{
		get
		{
			// Check if instance has not been set yet and set it it is not set already
			// This takes place only on the first time usage of this reference
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<UIPT_PRO_SoundController>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

	// Max number of AudioSource components
	public int m_MaxAudioSource = 8;

	// AudioClip component for music
	public AudioClip m_Music = null;

	// AudioClip component for buttons
	public AudioClip m_ButtonBack = null;
	public AudioClip m_ButtonClick = null;
	public AudioClip m_ButtonDisable = null;
	public AudioClip m_ButtonNo = null;
	public AudioClip m_ButtonPause = null;
	public AudioClip m_ButtonPlay = null;
	public AudioClip m_ButtonTab = null;
	public AudioClip m_ButtonYes = null;

	// Sound volume
	public float m_SoundVolume = 1.0f;

	// Music volume
	public float m_MusicVolume = 1.0f;

	#endregion Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		if (instance == null)
		{
			// Make the current instance as the singleton
			instance = this;

			// Make it persistent  
			DontDestroyOnLoad(this);
		}
		else
		{
			// If more than one singleton exists in the scene find the existing reference from the scene and destroy it
			if (this != instance)
			{
				InitAudioListener();
				Destroy(this.gameObject);
			}
		}
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// Initial AudioListener
		InitAudioListener();

		// Automatically play music if it is not playing
		if (IsMusicPlaying() == false)
		{
			// Play music
			Play_Music();
		}
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{

	}

	#endregion // MonoBehaviour

	// ########################################
	// Utilitie functions
	// ########################################

	#region Functions

	// Initial AudioListener
	// This function remove all AudioListener in other objects then it adds new one this object.
	void InitAudioListener()
	{
		// Destroy other's AudioListener components
		AudioListener[] pAudioListenerToDestroy = GameObject.FindObjectsOfType<AudioListener>();
		foreach (AudioListener child in pAudioListenerToDestroy)
		{
			if (child.gameObject.GetComponent<UIPT_PRO_SoundController>() == null)
			{
				Destroy(child);
			}
		}

		// Adds new AudioListener to this object
		AudioListener pAudioListener = gameObject.GetComponent<AudioListener>();
		if (pAudioListener == null)
		{
			pAudioListener = gameObject.AddComponent<AudioListener>();
		}
	}

	// Play music
	void PlayMusic(AudioClip pAudioClip)
	{
		// Return if the given AudioClip is null
		if (pAudioClip == null)
			return;

		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
			// Look for an AudioListener component that is not playing background music or sounds.
			bool IsPlaySuccess = false;
			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
					// Play music
					if (pAudioSourceList[i].isPlaying == false)
					{
						pAudioSourceList[i].loop = true;
						pAudioSourceList[i].clip = pAudioClip;
						pAudioSourceList[i].ignoreListenerVolume = true;
						pAudioSourceList[i].playOnAwake = false;
						pAudioSourceList[i].Play();
						break;
					}
				}
			}

			// If there is not enough AudioListener to play AudioClip then add new one and play it
			if (IsPlaySuccess == false && pAudioSourceList.Length < 16)
			{
				AudioSource pAudioSource = pAudioListener.gameObject.AddComponent<AudioSource>();
				pAudioSource.rolloffMode = AudioRolloffMode.Linear;
				pAudioSource.loop = true;
				pAudioSource.clip = pAudioClip;
				pAudioSource.ignoreListenerVolume = true;
				pAudioSource.playOnAwake = false;
				pAudioSource.Play();
			}
		}
	}

	// Play sound one shot
	void PlaySoundOneShot(AudioClip pAudioClip)
	{

		// Return if the given AudioClip is null
		if (pAudioClip == null)
			return;

		// We wait for a while after scene loaded
		if (Time.timeSinceLevelLoad < 1.5f)
			return;

		// Look for an AudioListener component that is not playing background music or sounds.
		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
			bool IsPlaySuccess = false;
			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
					if (pAudioSourceList[i].isPlaying == false)
					{
						// Play sound
						pAudioSourceList[i].PlayOneShot(pAudioClip);
						break;
					}
				}
			}

			// If there is not enough AudioListener to play AudioClip then add new one and play it
			if (IsPlaySuccess == false && pAudioSourceList.Length < 16)
			{
				// Play sound
				AudioSource pAudioSource = pAudioListener.gameObject.AddComponent<AudioSource>();
				pAudioSource.rolloffMode = AudioRolloffMode.Linear;
				pAudioSource.playOnAwake = false;
				pAudioSource.PlayOneShot(pAudioClip);
			}
		}
	}

	// Set music volume between 0.0 to 1.0
	public void SetMusicVolume(float volume)
	{
		m_MusicVolume = volume;

		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
					if (pAudioSourceList[i].ignoreListenerVolume)
					{
						pAudioSourceList[i].volume = volume;
					}
				}
			}
		}
	}

	// If music is playing, return true.
	public bool IsMusicPlaying()
	{
		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
					if (pAudioSourceList[i].ignoreListenerVolume == true)
					{
						if (pAudioSourceList[i].isPlaying == true)
						{
							return true;
						}
					}
				}
			}
		}

		return false;
	}

	// Set sound volume between 0.0 to 1.0
	public void SetSoundVolume(float volume)
	{
		m_SoundVolume = volume;
		AudioListener.volume = volume;
	}

	// Play music
	public void Play_Music()
	{
		PlayMusic(m_Music);
	}

	// Play Back button sound
	public void Play_SoundBack()
	{
		PlaySoundOneShot(m_ButtonBack);
	}

	// Play Click sound
	public void Play_SoundClick()
	{
		PlaySoundOneShot(m_ButtonClick);
	}

	// Play Disabled button sound
	public void Play_SoundDisable()
	{
		PlaySoundOneShot(m_ButtonDisable);
	}

	// Play No button sound
	public void Play_SoundNo()
	{
		PlaySoundOneShot(m_ButtonNo);
	}

	// Play Pause button sound
	public void Play_SoundPause()
	{
		PlaySoundOneShot(m_ButtonPause);
	}

	// Play Play button sound
	public void Play_SoundPlay()
	{
		PlaySoundOneShot(m_ButtonPlay);
	}

	// Play Tap sound
	public void Play_SoundTap()
	{
		PlaySoundOneShot(m_ButtonTab);
	}

	// Play Yes sound
	public void Play_SoundYes()
	{
		PlaySoundOneShot(m_ButtonYes);
	}

	#endregion // Functions
}
