------------------------------------------------------------------
GUI Animator for Unity UI 1.0.0
------------------------------------------------------------------

	Quickly and easily add professional animations for Unity UI elements. This package will save your time.

	Features:

		ï 9 Demo scenes with sample scripts for C# developers.
		ï 9 Demo scenes with sample scripts for JavaScript developers.
		ï Animate position, rotation, scale, fade using tweeners.
		ï Able to play sounds along with UI animations.
		ï Categorizes into In/Idle/Out animations into tabs.
		ï Ignorable time scale.
		ï Callbacks capability.
		
		ï DOTween, HOTween, iTween and LeanTween compatible.
		ï You can add more tweeners.
		ï Use preprocessor directives to examines Tweener before actual compilation.

		ï Unity UI compatible (http://unity3d.com/learn/tutorials/modules/beginner/ui).
		ï All Unity Canvas Render Modes and Unity Canvas Scale Modes compatible.

		ï Support all build player platforms.

	Compatible:

		ï Unity 4.7.1 or higher.
		ï Unity 5.3.4 or higher.

	Note:
	
		ï Old version users have to delete GSuiEditor script (found in Scripts/Editor folder).
		ï Unity Editor does not generate Texture Atlas for Unity UI. Please visit http://docs.unity3d.com/Manual/SpritePacker.html.
		ï This package makes a discount price for UI Pack: Toony PRO (https://www.assetstore.unity3d.com/#!/content/44103).

	Product page:

		https://www.assetstore.unity3d.com/en/#!/content/28709

	Please direct any bugs/comments/suggestions to support e-mail (geteamdev@gmail.com).
		
	Thank you for your support,

	Gold Experience Team
	E-mail: geteamdev@gmail.com
	Website: http://www.ge-team.com

------------------------------------------------------------------
Release notes
------------------------------------------------------------------

	Version 1.0.0
	
		ï Fixed GSui_Object always disappear from Hierarchy tab.
		ï Able to check the UI element; no animate yet, animating or animated. (See "Checking GUI Animator status" section in "!How to.txt" file)
		ï No longer need GSuiEditor script file, old version users have to delete GSuiEditor script that found in Scripts/Editor.
		ï Unity 4.7.1 and higher compatible.
		ï Unity 5.3.4 and higher compatible.

	Version 0.9.95 (Releasd on Mar 29, 2016)
	
		ï Fixed GUID conflict with other packages.
		ï Update Demo scenes and sample scripts.
		ï Change refactor some classes and variables.
		ï Change rearrange folders.
		ï Unity 4.6.9 and higher compatible.
		ï Unity 5.3.2 and higher compatible.

	Version 0.9.93 (Releasd on Oct 29, 2015)

		ï Fixed "Coroutine couldn't be started"ù error occurs when user sets the GameObject to active/inactive while GEAnim is animating.

	Version 0.9.92c (Releasd on Oct 14, 2015)

		ï Change Canvas UI Scale Mode in all demo scenes to Scale With Screen Size.
		ï Change Some parameters of GEAnim in Callback demo scene (only in Unity 5.2.0).
		ï Fixed wrong folder names.
		ï Supports multiple version of Unity; Unity 4.6.0 or higher, Unity 5.0.0 or higher, Unity 5.2.0 or higher.

	Version 0.9.92 (Released on Sep 28, 2015)

		ï Fixed GETween has memory leak.
		ï Fixed Friendly Inspector has Texture2D memory leak when user saves the scene.
		ï Fixed Wrong ease type convertion when use with LeanTween.
		ï Update smaller of GETween.dll file size.
		ï Update speed up Friendly Inspector.
		ï Unity 5.2.0 and higher compatible.

	Version 0.9.91 (Released on Sep 21, 2015)

		ï Add Show/Hide icon in Hierarchy tab (it can be set in Friendly Inspector).
		ï Add Icon alignment in Hierarchy tab.
		ï Change Camera clear flags to solid color in all demo scenes.
		ï Fixed GUIAnim has protection errors when it works with DOTween, HOTween, LeanTween.
		ï Fixed User can not drop GameObject to callbacks in Friendly Inspector.
		ï Update DOTween 1.0.750 and higher compatible.
		ï Update LeanTween 2.28 and higher compatible.
		ï Unity 5.1.3 and higher compatible.

	Version 0.9.9 (Released on Sep 9, 2015)

		ï Add "960x600px"ù to all demo scene names.
		ï Add 9 demo scenes for Javascript developers.
		ï Add 11 sample scripts for Javascript developers.
		ï Add Friendly Inspector for GUIAnimSystem and GEAnimSystem elements.
		ï Add GEAnim has new 10 override functions; Anim_In_MoveComplete(), Anim_In_RotateComplete(), Anim_In_ScaleComplete(), Anim_In_FadeComplete(), Anim_In_AllComplete(), Anim_Out_MoveComplete(), Anim_Out_RotateComplete(), Anim_Out_ScaleComplete(), Anim_Out_FadeComplete(), Anim_Out_AllComplete().
		ï Add Hide animation parameters in Friendly Inspector unless it has been enabled.
		ï Add GUIAnim and GEAnim can play In-Animations on Start() function ("On Start"ù parameter in Inspector tab).
		ï Add An option to disable/destroy GameObject after In-Animations completed ("On In-anims Complete" parameter in Inspector tab).
		ï Add An option to disable/destroy GameObject after Out-Animations completed ("On Out-anims Complete" parameter in Inspector tab).
		ï Add After Delay Sound into each animation, this will let user play AudioClip at right time after the delay.
		ï Add GUIAnimSystem and GEAnimSystem has MoveInAll() and MoveOutAll() functions to play all GUI Animator elements in the scenes.
		ï Add GUI Animator item shows mini-icon at the right edge of row in Inspector tab.
		ï Add GUIAnimSystem will be add into the scene automatically when GUIAnim or GEAnim component is added into a GameObject.
		ï Change Rename "Demo"ù folder to "Demo (CSharp)".
		ï Fixed Remove rotation and scale issues of In-Animation.
		ï Fixed Sometimes error happens when select GUIAnimSystem or GEAnimSystem object in Hierarchy tab.
		ï Fixed Remove minor known issues.
		ï Fixed GETween is more smooth.
		ï Update Sample Callback scene.

	Version 0.9.8 (Released on Aug 14, 2015)

		ï Add Callback system for Move, Rotate, Scale, Fade.
		ï Add Callback demo scene.
		ï Fixed GUIAnimaEditor and GEAnimEditor components have duplicated parameters.
		ï Fixed The same field name is serialized multiple times in the class.
		ï Fixed Inspector tab, sometimes focus on wrong control when animation tab is changed in Friendly Inspector mode.
		ï Update UI layout of Friendly Inspector.

	Version 0.9.6 (Released on Jun 23, 2015)

		ï Update Unity 5.0.1 and higher compatible.
		ï Update Support latest version of LeanTween and DOTween.
		ï Update Friendly Inspector.
		ï Fixed Wrong Move and Rotate animations.
		ï Fixed Known issues in version 0.8.4.
		ï Unity 5.0.1 and higher compatible.

	Version 0.8.4 (Released on Apr 6, 2015)

		ï Add Unity 5.x.x compatible.
		ï Add DOTween (HOTween v2) compatible.
		ï Add Works with all Unity Canvas Render Modes.
		ï Add Works with all Unity Canvas UI Scale Modes.
		ï Add User can separately test MoveIn/Idle/MoveOut animations.
		ï Add User can set Idle time for Auto animation.
		ï Add Rotation In/Out.
		ï Add Begin/End Sounds to animations.
		ï Add Friendly inspector, all animation are categorised into tabs.
		ï Add Friendly inspector, show/hide Easing graphs.
		ï Add Friendly inspector, show/hide help boxes.
		ï Fixed Bugs and known issues in 0.8.3.
		ï Update Demo scenes and scripts.
		ï Unity 4.6.0 and higher compatible.

	Version 0.8.3 (Initial version, released on Jan 31, 2015)

		ï Unity 4.5.0 and higher compatible.


------------------------------------------------------------------
URLs
------------------------------------------------------------------

	GUI Animator for Unity UI web demo:
		http://ge-team.com/pages/unity-3d/gui-animator-for-unity-ui/

	GE-Team Products page:
		http://ge-team.com/pages/unity-3d/

------------------------------------------------------------------
Compatible Tweeners
------------------------------------------------------------------

	DOTween
		Unity Asset Store: https://www.assetstore.unity3d.com/en/#!/content/27676
		Documentation: http://dotween.demigiant.com/documentation.php

	HOTween
		Unity Asset Store: https://www.assetstore.unity3d.com/#/content/3311
		Documentation: http://hotween.demigiant.com/documentation.html
	
	iTween
		Unity Asset Store: https://www.assetstore.unity3d.com/#/content/84
		Documentation: http://itween.pixelplacement.com/documentation.php

	LeanTween
		Unity Asset Store: https://www.assetstore.unity3d.com/#/content/3595
		Documentation: http://dentedpixel.com/LeanTweenDocumentation/classes/LeanTween.html

------------------------------------------------------------------
Easing type references
------------------------------------------------------------------
	
	ï Easings.net
		http://easings.net

	ï RobertPenner.com
		http://www.robertpenner.com/easing/easing_demo.html
