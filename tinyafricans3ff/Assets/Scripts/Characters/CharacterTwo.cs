using System;

[Serializable]
public class CharacterTwo : GameCharacter {

	private CharacterTwo() : base(ID) {

	} //make default constructor private to prevent instantiation 		

	private static CharacterTwo instance;
	public static CharacterTwo Instance {
		get { 
			if(instance == null) instance = new CharacterTwo();
			return instance;
		}
		set { instance = value; }
	}

	public const string ID = "Ama";
	public const string FIRST_NAME = "Ama";
	public const string LAST_NAME = "Ayotunde";
	public const string DESCRIPTION = "Ama is short for Amadi and her name means seemed destined to die at birth.  Ama was is the second of the triplets to be born, and was out of the womb 3 minutes before she took her first breadth. While all of the girls are born fighters she is the true Warrior of the family.";

	// This is just the initial strength, and by the end of taking a character 
	// through level 1 this should go up
	public const float DEFAULT_STRENGTH = 50;
	public const float DEFAULT_MAX_STRENGTH = 50;

}

