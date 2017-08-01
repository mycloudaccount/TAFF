using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager
{

	private int virtualCurrency=0;
	private bool makingPlayFabRequest=true;
	public bool MakingPlayFabRequest {
		get { return makingPlayFabRequest; }
		set { makingPlayFabRequest = value; }
	}

	private static bool donePrimingPlayer=false;
	private static bool doneLoadingUserData=false;

	private PlayerManager () {

	}

	private static PlayerManager instance;
	public static PlayerManager Instance {
		get { 
			if(instance == null) instance = new PlayerManager();
			return instance;
		}
		set { instance = value; }
	}
		
	public static bool LoadStatus () {

		return (
			donePrimingPlayer &&
			doneLoadingUserData
		);

	}

	public IEnumerator LoadPlayerSettings (bool primePlayer) {
	
		if (!primePlayer) {
			donePrimingPlayer = true;
		}

		yield return LoadAllPlayerSettings ();

	}

	private IEnumerator LoadAllPlayerSettings () {

		Debug.Log("Loading All PlayFab Player Settings...");

		//////////// LOAD ALL PLAYER'S USER DATA ////////////////////////////////////
		/// 
		/// 
		GetPlayerUserData ();
		while( makingPlayFabRequest ){yield return null;}
		doneLoadingUserData = true;

		/////////// PRIME PLAYER (should only happen once) //////////////////////////
		/// 
		/// 1. Give the Player Some initial Currency
		/// 2. Add some initial characters to the players character list
		if (!donePrimingPlayer) {

			// give some initial money to player
			GetCurrencyOfPlayer ();
			while( makingPlayFabRequest ){yield return null;}
			RemoveCurrencyFromPlayer (this.virtualCurrency);
			while( makingPlayFabRequest ){yield return null;}
			GiveCurrencyToPlayer (GameState.Constants.DEFAULT_COIN_COUNT);
			while( makingPlayFabRequest ){yield return null;}

			// add some initial characters to players inventory
			PurchaseInitialHero ("SisterOne", 100);
			while( makingPlayFabRequest ){yield return null;}

			// pull characters from inventory and add them to players charcter list (TODO: also update the characters to reflect custom data)
			GrantHeroToUser ("SisterOne", "Omo");

			donePrimingPlayer = true;
		}

		/////////// LOADING PLAYER CHARACTERS ////////////////////////////////////////
		/// 
		/// 
		HeroManager.Instance.LoadLittleHeros ();
		while( makingPlayFabRequest ){yield return null;}
		HeroManager.Instance.LoadHeros ();
		while( makingPlayFabRequest ){yield return null;}

		GameManager.Instance.LoadingPlayerSettings = false;
		Debug.Log("PlayFab Player Settings Loaded Successfully.");

	}

	private void PurchaseInitialHero (string heroId, int cash) {
		makingPlayFabRequest = true;
		PurchaseItemRequest purchaseItemRequest = new PurchaseItemRequest()
		{
			VirtualCurrency = "GC",
			Price = cash,
			ItemId = heroId
		};
		PlayFabClientAPI.PurchaseItem(purchaseItemRequest,(result) => {
			if (result.Items.Count > 0) {
				Debug.Log("It looks like player just purchased (" + result.Items[0].ItemId + ") from the catalog");
			} else {
				Debug.Log("Something went wrong and no items were purchased");
			}
			makingPlayFabRequest = false;
		}, (error) => {
			Debug.Log("Error purchasing " + heroId + ": " + error.ErrorMessage);
			makingPlayFabRequest = false;
		});
	}

	private void GrantHeroToUser (string heroId, string heroName) {
		makingPlayFabRequest = true;
		GrantCharacterToUserRequest grantCharacterToUserRequest = new GrantCharacterToUserRequest()
		{
			CharacterName = heroName,
			ItemId = heroId
		};
		PlayFabClientAPI.GrantCharacterToUser (grantCharacterToUserRequest,(result) => {
			Debug.Log("Granted Character (" + heroId + ") to player with the folloing details: ");
			Debug.Log("result.CharacterId: " + result.CharacterId);
			Debug.Log("result.CharacterType: " + result.CharacterType);
			if (result.CustomData != null) {
				Debug.Log("result.CustomData.ToString: " + result.CustomData.ToString());
			}
			makingPlayFabRequest = false;
		}, (error) => {
			Debug.Log("Error adding character to player's charaxcter list (" + heroId + "): " + error.ErrorMessage);
			makingPlayFabRequest = false;
		});
	}

	private void GetPlayerUserData () {
		makingPlayFabRequest = true;

		List<string> playerStates = new List<string>();

		GetUserDataRequest userDataRequest = new GetUserDataRequest()
		{
			PlayFabId = GameState.Instance.PlayFabUserId,
			Keys = null
		};
		PlayFabClientAPI.GetUserData(userDataRequest,(result) => {

			if ((result.Data == null) || (result.Data.Count == 0))
			{
				Debug.Log("No playfab user data available");
			}
			else
			{
				foreach (var item in result.Data)
				{
					Debug.Log("    " + item.Key + " = " + item.Value.Value);
					if (item.Key.Contains("PlayerGame_")) {
						Debug.Log("Loading Player Game State...");
						playerStates.Add(item.Value.Value);
					}
				}
			}

			// load the player states from user data
			if (playerStates.Count != 0) {

				foreach (var playerState in playerStates) {
					PlayerState ps = JsonUtility.FromJson<PlayerState>(playerState);
					GameManager.Instance.PlayerGames.Add (ps);
				}

				Debug.Log("All Saved Player Games Deserialized Successfully.");

			} 

			if (GameManager.Instance.PlayerGames.Count > 0) {
				// in production mode rather than hardcoding to 0th element this is something the user will select
				PlayerState.Current = GameManager.Instance.PlayerGames [0];
			} else {
				PlayerState.Current = new PlayerState ();
				GameState.Instance.CurrentPlayer = PlayerState.Current.PlayerGameId;

				// THIS IS THE FIRST TIME USER HAS ACCESSED THE GAME!!
				// let's assign some default values here

			}

			if (GameState.IsDevMode) {
				Debug.Log ("WARNING - IN DEV MODE!!!");

				// in production mode if SelectedCharacterId == null then just force user to assign
				// selected character
				if (PlayerState.Current.SelectedCharacterId == null){
					Debug.Log ("IN DEV MODE: No Set Primary Character Found So setting to a default one");
					PlayerState.Current.SelectedCharacterId = PlayerState.Constants.SISTER_ONE_ID;
				}
			}

			makingPlayFabRequest = false;

		}, (error) => {
			Debug.Log("Got error retrieving playfab user data:");
			Debug.Log(error.ErrorMessage);
			makingPlayFabRequest = false;
		});
	}

	private void GetCurrencyOfPlayer () {
		makingPlayFabRequest = true;
		AddUserVirtualCurrencyRequest virtualCurrencyRequest = new AddUserVirtualCurrencyRequest()
		{
			VirtualCurrency = "GC",
			Amount = 0
		};
		PlayFabClientAPI.AddUserVirtualCurrency(virtualCurrencyRequest,(result) => {
			Debug.Log("It looks like player (" + result.PlayFabId + ") currently has the following Virtual Currency: " + result.Balance);
			this.virtualCurrency = result.Balance;
			makingPlayFabRequest = false;
		}, (error) => {
			Debug.Log("Error Retrieving Virtual Currency: " + error.ErrorMessage);
			makingPlayFabRequest = false;
		});
	}

	private void GiveCurrencyToPlayer (int numberOfCoins) {
		makingPlayFabRequest = true;
		AddUserVirtualCurrencyRequest virtualCurrencyRequest = new AddUserVirtualCurrencyRequest()
		{
			VirtualCurrency = "GC",
			Amount = numberOfCoins
		};
		PlayFabClientAPI.AddUserVirtualCurrency(virtualCurrencyRequest,(result) => {
			Debug.Log("Adding (" + numberOfCoins + ") coins to Player " + result.PlayFabId + " has left them with: " + result.Balance);
			makingPlayFabRequest = false;
		}, (error) => {
			Debug.Log("Error Updating Virtual Currency: " + error.ErrorMessage);
			makingPlayFabRequest = false;
		});
	}

	private void RemoveCurrencyFromPlayer (int numberOfCoins) {
		makingPlayFabRequest = true;
		SubtractUserVirtualCurrencyRequest virtualCurrencyRequest = new SubtractUserVirtualCurrencyRequest()
		{
			VirtualCurrency = "GC",
			Amount = numberOfCoins
		};
		PlayFabClientAPI.SubtractUserVirtualCurrency(virtualCurrencyRequest,(result) => {
			Debug.Log("Removing (" + numberOfCoins + ") coins from Player " + result.PlayFabId + " has left them with: " + result.Balance);
			makingPlayFabRequest = false;
		}, (error) => {
			Debug.Log("Error Updating Virtual Currency: " + error.ErrorMessage);
			makingPlayFabRequest = false;
		});
	}

	private void DeleteCharacterFromUser (string characterId)
	{
		ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
		{
			FunctionName = "deleteCharacterFromUser", // Arbitrary function name (must exist in your uploaded cloud.js file)
			FunctionParameter = new { CharacterId = characterId, SaveCharacterInventory = false }, // The parameter provided to your function
			GeneratePlayStreamEvent = false, // Optional - Shows this event in PlayStream
		};
		PlayFabClientAPI.ExecuteCloudScript(request, OnDeleteCharacterFromUser, OnError);
	}

	// Await the response and process the result
	private void OnDeleteCharacterFromUser(ExecuteCloudScriptResult result)
	{
		//Debug.Log("OnDeleteCharacterFromUser() Result: " + result.ToString());
		makingPlayFabRequest = false;
	}

	private void OnError (PlayFabError error) {
		Debug.Log("Error making PlayFab request: " + error.ErrorMessage);
		makingPlayFabRequest = false;
	}

}

