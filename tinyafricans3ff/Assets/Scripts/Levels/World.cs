using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class World
{

	public WorldDetails Details;

	// how long the player lasted in this level
	public bool Completed = false;

	// has this level actually been primed
	public bool Primed = false;

}

