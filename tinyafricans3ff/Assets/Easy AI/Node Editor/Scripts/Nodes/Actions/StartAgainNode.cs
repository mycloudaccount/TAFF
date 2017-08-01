using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AxlPlay{
	[Node("Actions/Start Again",true)]
	public class StartAgainNode : Node {
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
			title = "Start Again";
			
			myType = type.action;
			
			connectors[0].Init(ConCorner.left, new Rect(0f, 35f, 20f, 20f), this,Connector.knobType.input , Color.red);
			connectors[1].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this,Connector.knobType.output , Color.cyan);
 
			
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
			isRunning = true;
			
			var temp = self.GetComponent<EasyAIFSM>();
			
 			for (int i = 0; i < temp.nodes.Count; i++) {
				
	 			if (temp.nodes[i] is StartAINode ) {
		 			
				    // get next node 
					if ( temp.nodes[i].connectors[0].connections.Count > 0 ){
						
						temp.NextState(temp.nodes[i].connectors[0].connections[0].homeNode);
						isRunning = false;
						
						break;
					}
				}
 			}// end for
 
				
		}
		public override Node.Task OnUpdate()
		{
			
			return Task.Success;
		}
	}
}
