using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AxlPlay{
	
[Node("Actions/Running",true)]
	public class RunningNode : Node {
		#if UNITY_EDITOR
	public override void Init()
	{
		base.Init();
		headerColor = Color.red;
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 100f;
		rect.height = 80f;
		// set node name
		title = "Running";
		
		connectors[0].Init(ConCorner.left, new Rect(0f, 35f, 20f, 20f), this,Connector.knobType.input , Color.red);
		connectors[1].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.output , Color.cyan);
		
		
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
	public override void OnStart()
	{
		
	}
	public override Node.Task OnUpdate()
	{
		return Task.Running;
	}
}
}
