using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AxlPlay{

	[Node("Conditionals/Wait for Finish",true)]
	public class WaitForFinishNode : Node {
		#if UNITY_EDITOR
		public override void Init()
		{
			base.Init();
			headerColor = Color.cyan;
			for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
			
		////  set the node side
			rect.width = 100f;
			rect.height = 80f;
		// set node name
			title = "Wait for Finish";
			myType = type.conditional;
			connectors[0].Init(ConCorner.left, new Rect(0f, 35f, 20f, 20f), this,Connector.knobType.input , Color.cyan);
			connectors[1].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.output , Color.red);
			
			
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
			if (this.connectors[0].connections.Count > 0){
				var previousNode = this.connectors[0].connections[0].homeNode;
				
				if (previousNode.hasFinished){
					
					return Task.Success;
				}
			}
			return Task.Running;
			
			
		}
 }
}
