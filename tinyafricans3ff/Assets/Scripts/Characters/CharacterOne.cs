using System;

[Serializable]
public class CharacterOne : GameCharacter {

	private CharacterOne() : base(ID) {

	} //make default constructor private to prevent instantiation 		

	private static CharacterOne instance;
	public static CharacterOne Instance {
		get { 
			if(instance == null) instance = new CharacterOne();
			return instance;
		}
		set { instance = value; }
	}

	public const string ID = "Omo";
	public const string FIRST_NAME = "Omo";
	public const string LAST_NAME = "Ayotunde";
	public const string DESCRIPTION = "Omo is short for Omobolanle, and means a child who met wealth at home. Omo is the first of the triplets to be born, and she is always thinking of ways to gain more control.";

	// This is just the initial strength, and by the end of taking a character 
	// through level 1 this should go up
	public const float DEFAULT_STRENGTH = 50;
	public const float DEFAULT_MAX_STRENGTH = 50;

}

