// GUI Animator for Unity UI
// Version: 1.0.0
// Compatilble: Unity 4.7.1 and Unity 5.3.4 or higher, more info in Readme.txt file.
//
// Author:	Gold Experience Team (http://www.ge-team.com)
// Details:	https://www.assetstore.unity3d.com/en/#!/content/28709
// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

#endregion // Namespaces

// ######################################################################
// GSui class
// Handles animation speed and auto animation of all GAui elements in the scene.
// ######################################################################

#region GSui

public class GSui : GUIAnimSystem
{

	#region Variables
	
	#endregion // Variables
	
	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################
	
	#region MonoBehaviour
		
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	//void Start ()
	public override void GUIAnimSystemStart()
	{
		// ########################################
		// PERFORM YOUR SCRIPTS


		// ########################################
	}
		
	// Update is called every frame, if the MonoBehaviour is enabled.
	//void Update ()
	public override void GUIAnimSystemUpdate()
	{
		// ########################################
		// PERFORM YOUR SCRIPTS


		// ########################################
	}
	
	#endregion // MonoBehaviour

}

#endregion
