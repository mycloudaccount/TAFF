using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameWorlds
{
	public int WorldCount;
	public MList ListOfWorlds;

	[Serializable]
	public class MList
	{
		public WorldDetail WorldOne;
	}

}

