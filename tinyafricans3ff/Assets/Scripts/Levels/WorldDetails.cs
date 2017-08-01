using System;
using UnityEngine;

[Serializable]
public abstract class WorldDetails
{

	public string WorldId;
	public int LevelCount;
	public MList ListOfLevels;

	[Serializable]
	public class MList {

		public LevelDetail LevelOne;
		public LevelDetail LevelTwo;
		public LevelDetail LevelThree;
		public LevelDetail LevelFour;
		public LevelDetail LevelFive;
		public LevelDetail LevelSix;
		public LevelDetail LevelSeven;
		public LevelDetail LevelEight;
		public LevelDetail LevelNine;
		public LevelDetail LevelTen;

	}

	public LevelDetail GetLevelFromId (int levelId) {

		switch (levelId) {
		case 1:
			return ListOfLevels.LevelOne;
		case 2:
			return ListOfLevels.LevelTwo;
		case 3:
			return ListOfLevels.LevelThree;
		case 4:
			return ListOfLevels.LevelFour;
		case 5:
			return ListOfLevels.LevelFive;
		case 6:
			return ListOfLevels.LevelSix;
		case 7:
			return ListOfLevels.LevelSeven;
		case 8:
			return ListOfLevels.LevelEight;
		case 9:
			return ListOfLevels.LevelNine;
		case 10:
			return ListOfLevels.LevelTen;
		default:
			return null;
		}

	}

	public LevelDetail GetPreviousLevel (Level currentLevel) {

		int nextLevelId = currentLevel.LevelId - 1;

		switch (nextLevelId) {
		case 0:
			return null;
		case 1:
			return ListOfLevels.LevelOne;
		case 2:
			return ListOfLevels.LevelTwo;
		case 3:
			return ListOfLevels.LevelThree;
		case 4:
			return ListOfLevels.LevelFour;
		case 5:
			return ListOfLevels.LevelFive;
		case 6:
			return ListOfLevels.LevelSix;
		case 7:
			return ListOfLevels.LevelSeven;
		case 8:
			return ListOfLevels.LevelEight;
		case 9:
			return ListOfLevels.LevelNine;
		case 10:
			return ListOfLevels.LevelTen;
		default:
			return null;
		}

		//return GameManager.Instance.GetPreviousGameLevel (currentLevel, this);

	}

	public LevelDetail GetNextLevel (Level currentLevel) {

		int nextLevelId = currentLevel.LevelId + 1;

		if (nextLevelId > LevelCount) {
			return null;
		}

		switch (nextLevelId) {
		case 10:
			return null;
		case 1:
			return ListOfLevels.LevelOne;
		case 2:
			return ListOfLevels.LevelTwo;
		case 3:
			return ListOfLevels.LevelThree;
		case 4:
			return ListOfLevels.LevelFour;
		case 5:
			return ListOfLevels.LevelFive;
		case 6:
			return ListOfLevels.LevelSix;
		case 7:
			return ListOfLevels.LevelSeven;
		case 8:
			return ListOfLevels.LevelEight;
		case 9:
			return ListOfLevels.LevelNine;
		default:
			return null;
		}

		//return GameManager.Instance.GetNextGameLevel (currentLevel, this);

	}

}




