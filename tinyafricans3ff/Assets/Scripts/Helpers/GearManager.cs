using System;
using UnityEngine;

public abstract class GearManager
{
	protected GearManager () {
		
	}

	public abstract void AddComponent (GameObject gameObject, GameCharacter gC, string componentType);

	public abstract Quaternion RemoveComponent (GameObject gameObject, GameCharacter gC, string componentType);


}

