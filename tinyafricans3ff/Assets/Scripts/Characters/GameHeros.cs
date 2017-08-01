using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameHeros
{
	public int HeroCount;
	public MList ListOfHeros;

	[Serializable]
	public class MList
	{
		public WorldDetail WorldOne;
	}

}

