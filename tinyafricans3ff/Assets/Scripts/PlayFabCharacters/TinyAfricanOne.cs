using System;


public class TinyAfricanOne : LittleHero
{

	private TinyAfricanOne() : base(ID) {

		PresentFromStart = PRESENT_FROM_START;

	} //make default constructor private to prevent instantiation 		

	private static TinyAfricanOne instance;
	public static TinyAfricanOne Instance {
		get { 
			if(instance == null) instance = new TinyAfricanOne();
			return instance;
		}
		set { instance = value; }
	}

	public const string ID = "Omo";
	public const bool PRESENT_FROM_START = true;

}


