using System;

[Serializable]
public class CharacterThree : GameCharacter {

	private CharacterThree() : base(ID) {

	} //make default constructor private to prevent instantiation 		

	private static CharacterThree instance;
	public static CharacterThree Instance {
		get { 
			if(instance == null) instance = new CharacterThree();
			return instance;
		}
		set { instance = value; }
	}

	public const string ID = "Demi";
	public const string FIRST_NAME = "Demi";
	public const string LAST_NAME = "Ayotunde";
	public const string DESCRIPTION = "Demi is short for Abidemi and her name means during her fathers absence.  Thinking Ama was the last Demi's Father had left the room just before she jumped out.  Demi is the third of the triplets to be birthed and is the wisest of the 3.";

	// This is just the initial strength, and by the end of taking a character 
	// through level 1 this should go up
	public const float DEFAULT_STRENGTH = 50;
	public const float DEFAULT_MAX_STRENGTH = 50;

}

