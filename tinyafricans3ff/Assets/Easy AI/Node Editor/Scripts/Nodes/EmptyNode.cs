using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AxlPlay{
	[Node("Empty",false)]
	public class EmptyNode : Node {
		

 	
#if UNITY_EDITOR
 	public override void Init()
	{
		base.Init();
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 220f;
		rect.height = 100f;
		// set node name
		title = "Send Event";
		
		myType = type.action;
		
		connectors[0].Init(ConCorner.left, new Rect(0f, 65f, 20f, 20f), this,Connector.knobType.input , Color.red);
		connectors[1].Init(ConCorner.right, new Rect(0f, 65f, 20f, 20f), this, Connector.knobType.output , Color.red);
		
		 
		
	
	}
		 
	public override void DrawNode()
	{
		
	 
		
		GUI.color =  Color.red;
		GUILayout.BeginHorizontal (EditorStyles.helpBox);	
		
		
		EditorGUILayout.LabelField(title,EditorStyles.label );  //customSkin.label 
		GUI.color = Color.white;
		
		
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(10f);
 
		
	}
		#endif
		public override void OnStart()
		{
			isRunning = true;
			
			Actions.Invoke();
			
			
			
		}
		public override Node.Task OnUpdate()
		{
			
			return Task.Running;
		}
}
}
