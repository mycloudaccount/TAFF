// GearDriver.cs (C)2015 by Alexander Schlottau, Hamburg, Germany
//   simulates procedural gear and worm gear objects at runtime.


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GearDriver : MonoBehaviour {

	[Serializable]
	public class Settings {

		public bool isMotor, isShaft, isWorm = false, invWormOut = false;
		public bool updatePowerchainOnce = false;
		public bool updatePowerchainLive = false;
		public float motorSpeed = 0.0f;
		public List<GearDriver> outputTo;
	}

	public  Settings settings;
	public  float actualSpeed = 0.0f;
	private float wantedMotorSpeed = 0.0f;
	private int error = 0;


	void Start () {
		
		if (GetComponent<ProceduralWormGear> () != null) 
			settings.isWorm = true;
		else
			if (GetComponent<ProceduralGear> () == null)
				settings.isShaft = true;
			else
				settings.isShaft = false;
		
		if (settings.isMotor) {
			error++;
			UpdateConnections ( settings.isShaft? 0 : GetTeethCountFromGearScript(), settings.motorSpeed, error);
			wantedMotorSpeed = actualSpeed;
		}
	}
	
	private int GetTeethCountFromGearScript() {
		
		if (GetComponent<ProceduralWormGear> () != null)
			if (GetComponent<ProceduralWormGear> ().prefs.lr)
				return settings.invWormOut?1:-1;
			else
				return settings.invWormOut?-1:1;
		else
			if (GetComponent<ProceduralGear> () != null)
				return GetComponent<ProceduralGear> ().prefs.teethCount;
			else
				return 0;
	}

	void Update () {
	
		if (settings.isMotor)
			DriveMotor ();

		gameObject.transform.Rotate (Vector3.up * Time.deltaTime * -actualSpeed);
	}

	private void DriveMotor() {

		if (settings.updatePowerchainOnce || settings.updatePowerchainLive)
			UpdatePowerchain ();
		else
			actualSpeed = wantedMotorSpeed;
	}
	
	public void UpdateConnections(int _otherTeethCount, float _speed, int _error) {

		if (!settings.isMotor) {
			if (error == _error) {
				Debug.LogWarning ("GearDriver.cs : Get two inputs on " + gameObject.name + " . Check connections for loop.");
				this.enabled = false;
				return;
			}
		}
		error = _error;

		int tc = 0;
		if (_otherTeethCount == 0) {
			if (!settings.isShaft)
				_otherTeethCount = -GetTeethCountFromGearScript(); 
			else
				_otherTeethCount = 1;
		}
		if (!settings.isShaft) {
			tc = GetTeethCountFromGearScript(); 
			actualSpeed = (float)_otherTeethCount / (float)tc * -_speed;
		}
		else
			actualSpeed = _speed;

		for (int i = 0; i < settings.outputTo.Count; i++) {
			if (settings.outputTo[i] != null) {
				settings.outputTo[i].UpdateConnections (tc, actualSpeed, error);
			} else {
				settings.outputTo.RemoveAt(i);
			}
		}
	}
	
	public void UpdatePowerchain() {

		error = error>8?0:error+=2;
		UpdateConnections (settings.isShaft? 0 : GetTeethCountFromGearScript(), settings.motorSpeed, error);
		settings.updatePowerchainOnce = false;
		wantedMotorSpeed = -settings.motorSpeed;
	}
}
