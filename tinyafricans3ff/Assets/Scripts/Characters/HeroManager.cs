using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using PlayFab.Json;
using System.Collections.ObjectModel;
using System;

public class HeroManager
{

	private HeroManager() {
		GameManager.Instance.LoadingHeroData = true;
	} //make default constructor private to prevent instantiations 		

	private static HeroManager instance;
	public static HeroManager Instance {
		get { 
			if(instance == null) instance = new HeroManager();
			return instance;
		}
		set { instance = value; }
	}

	private Hero selectedHero = null;
	public Hero SelectedHero {
		get { return selectedHero; }
		set { selectedHero = value; }
	}

	private Dictionary<string,Hero> heros = new Dictionary<string,Hero>();
	public Dictionary<string,Hero> Heros {
		get { return heros; }
		set { heros = value; }
	}

	private Dictionary<string,Hero> littleHeros = new Dictionary<string,Hero>();
	public Dictionary<string,Hero> LittleHeros {
		get { return littleHeros; }
		set { littleHeros = value; }
	}

	public void LoadLittleHeros() {

		PlayerManager.Instance.MakingPlayFabRequest = true;
		// load little hero characters from playfab (find all characters with class = LittleHero)
		Debug.Log("Loading all PlayFab Characters of class LittleHero...");
		PlayerManager.Instance.MakingPlayFabRequest = false;


	}

	public void LoadHeros() {

		PlayerManager.Instance.MakingPlayFabRequest = true;

		Debug.Log("Loading PlayFab Heros...");
		GetTitleDataRequest request = new GetTitleDataRequest ();

		System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
		keys.Add ("GameHeros");
		request.Keys = keys;

		PlayFabClientAPI.GetTitleData(request,(result) => {

			foreach (var item in result.Data)
			{
				Debug.Log ("    " + item.Key + " = " + item.Value);
				if (item.Key == "GameHeros") {

					// convert the item's value into a Dictionary<string, string>
					Dictionary<string, object> values = 
						PlayFab.SimpleJson.DeserializeObject<Dictionary<string, object>>(item.Value);
					
					Debug.Log ("Instantiating the list of GameHeros... " + values.Count);
					foreach (var subItem in values)
					{
						Debug.Log ("    " + subItem.Key + " = " + subItem.Value);
						string jsonString = Convert.ToString(subItem.Value);
						Debug.Log ("Instantiating a Hero JSON String: " + jsonString);
						selectedHero = JsonUtility.FromJson<Hero>(jsonString);
						// add hero to hero map
						heros.Add (selectedHero.Id, selectedHero);
					}
				}
			}

			if (selectedHero != null) {
				Debug.Log ("Name of Selected Hero: " + selectedHero.FirstName);
			} 

			PlayerManager.Instance.MakingPlayFabRequest = false;
			GameManager.Instance.LoadingHeroData = false;
			Debug.Log("PlayFab Hero data Loaded Successfully.");

		}, (error) => {
			Debug.Log("Got error retrieving PlayFab Game Hero data: ");
			Debug.Log(error.ErrorMessage);
		});

	}

}

