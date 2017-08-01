using System;
using UnityEngine;

[Serializable]
public class Settings
{

	private Settings () {
		
	}

	private static Settings instance;
	public static Settings Instance {
		get { 
			if(instance == null) instance = new Settings();
			return instance;
		}
		set { instance = value; }
	}

	public bool EffectsMuted = false;

	public float EffectsVolume = GameState.Constants.DEFAULT_EFFECTS_VOLUME;

	public bool MusicMuted = false;

	public float MusicVolume = GameState.Constants.DEFAULT_MUSIC_VOLUME;


}

