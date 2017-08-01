using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelDetail
{
	public string WorldId;
	public int LevelId;
	public string LevelTitle;
	public string LevelDescription;
	public bool Enabled = false;

	public float AngleOnMap = -999.0f;
}

