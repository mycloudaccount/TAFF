using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

namespace AxlPlay{
	
	[Node("Conditionals/Smooth Look At",true)]
	public class SmoothLookAtNode : Node {
		
		public GameObject _target;
		#if UNITY_EDITOR
		public override void Init()
		{
			base.Init();
			headerColor = Color.cyan;
			for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
			
			////  set the node side
			rect.width = 280f;
			rect.height = 80f;
			// set node name
			title = "Smoth Look At";
			myType = type.conditional;

			connectors[0].Init(ConCorner.left, new Rect(0f, 35f, 20f, 20f), this,Connector.knobType.input , Color.cyan);
			connectors[1].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.output , Color.red);
			
			// dropdown index choice
			choiceIndex = new int[1];
			
			// index from fsmVariables
			indexVarSelected = new int[1];
			
			// bool for button selected
			useInternalVar = new bool[1];
			
		}
		public override void DrawNode()
		{
			varGameObjects = fsmVariables.Where(q=> q.VariableType  == typeof(GameObject)).Select(x=>x.Name).ToArray();
			
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.alignment = TextAnchor.LowerCenter;
			
			GUI.color =  headerColor;
			GUILayout.BeginHorizontal (EditorStyles.helpBox);	
			EditorGUILayout.LabelField(title );   
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal ();
			GUILayout.Space(10f);
			EditorGUILayout.BeginHorizontal();
			if (!useInternalVar[0]){
				_target = (GameObject)EditorGUILayout.ObjectField("GameObject to Rotate:", _target, typeof(GameObject), true);
			}else{
				 // to prevent is delete the variable from inspector
				if (choiceIndex[0] >= varGameObjects.Count()) {
					
					choiceIndex[0]  =0 ;
				}
				EditorGUILayout.LabelField("We are searching for:",GUILayout.Width(120));
				
				if (varGameObjects.Count() > 0){
					
					choiceIndex[0]  = EditorGUILayout.Popup(choiceIndex[0],varGameObjects,GUILayout.Height(10),GUILayout.Width(110) );
					
					for (int i = 0; i < fsmVariables.Count; i++) {
						if (varGameObjects.Count() > 0 &&   fsmVariables[i].Name == varGameObjects[choiceIndex[0]] ){
							
							indexVarSelected[0] = i;
 						
							break;
						}
					}
				}
			}
			if (useInternalVar[0]){
				GUI.color = Color.green;
			}else{
				GUI.color = Color.white;
			}
			
			if (GUILayout.Button ("S", GUILayout.Height(18), GUILayout.Width(18))){
				_target = null;
				if (useInternalVar[0]){
					
					useInternalVar[0] = false;
				}else{
					useInternalVar[0] = true;
				}
				
			}
			GUI.color = Color.white;
			
			EditorGUILayout.EndHorizontal();
		}
		#endif
		public override void OnStart()
		{
			isRunning = true;
		}
		public override Node.Task OnUpdate()
		{
			if (_target != null)
				RotateTowards(_target.transform);
			return Task.Running;
			
		}
		private void RotateTowards(Transform target)
		{
			Vector3 direction = (target.position - self.transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			self.transform.rotation = Quaternion.Slerp(self.transform.rotation, lookRotation, Time.deltaTime * 5f);
		}
	}
}
