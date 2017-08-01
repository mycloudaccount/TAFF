using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
 

namespace AxlPlay{
	[Node("Start",false)]
	public class StartAINode: Node {
 
  		#if UNITY_EDITOR
  	public override void Init()
	{
		base.Init();
  		
		headerColor = Color.red;
		for (int i=0; i< 1 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 100f;
		rect.height = 80f;
		// set node name
		title = "Start";

		connectors[0].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.output , Color.red);
			 
	 
		
	 
  
	}
	public override void DrawNode()
	{
		
		var centeredStyle = GUI.skin.GetStyle("Label");
		centeredStyle.alignment = TextAnchor.LowerCenter;
		
		GUI.color =  headerColor;
		GUILayout.BeginHorizontal (EditorStyles.helpBox);	
		EditorGUILayout.LabelField(title );   
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(10f);
	 
	}
	 #endif
	
	
	 
}
}

