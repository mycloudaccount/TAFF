using System;

[Serializable]
public class EnemyOne : GameEnemy {

	private EnemyOne() : base(ID) {

	} //make default constructor private to prevent instantiation 		

	private static EnemyOne instance;
	public static EnemyOne Instance {
		get { 
			if(instance == null) instance = new EnemyOne();
			return instance;
		}
		set { instance = value; }
	}

	public const string ID = "PurpleGrabber";
	public const string FIRST_NAME = "Grabber";
	public const string LAST_NAME = "Monster";
	public const string DESCRIPTION = "Purple Plant Monster.";
	public const float DEFAULT_STRENGTH = 50;
	public const float DEFAULT_MAX_STRENGTH = 50;

}

