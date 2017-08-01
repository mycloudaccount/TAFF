using UnityEngine;
using System.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AxlPlay{
	
	[Node("Conditionals/Can Hear",true)]
	public class CanHearNode : Node {
		[Tooltip("Should the 2D version be used?")]
		public bool usePhysics2D;
 
		[Tooltip("The tag of the object that we are searching for")]
 		public string targetTag;
		
		[Tooltip("The LayerMask of the objects that we are searching for")]
		public LayerMask objectLayerMask;
		
		[Tooltip("How far away the unit can hear")]
		public float hearingRadius = 5f;
		
		[Tooltip("The further away a sound source is the less likely the agent will be able to hear it. " +
		"Set a threshold for the the minimum audibility level that the agent can hear")]
		public float audibilityThreshold = 0.05f;
		
		[Tooltip("The offset relative to the pivot position")]
		public Vector3 offset;
		
		[Tooltip("The returned object that is heard")]
		public GameObject returnedObject;
		
		
		public LayerMask _objectLayerMask;
		public bool useInverted;
		
		public GameObject targetObject;	
		
		
		#if UNITY_EDITOR
		public override void Init()
		{
			base.Init();
			
			headerColor = Color.cyan;
			
			for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
			
			////  set the node side
			rect.width = 280f;
			rect.height = 245f;
			// set node name
			title = "Can Hear Object";
			
			myType = type.conditional;
			
			connectors[0].Init(ConCorner.left, new Rect(0f, 65f, 20f, 20f), this,Connector.knobType.input , Color.cyan);
			connectors[1].Init(ConCorner.right, new Rect(0f, 65f, 20f, 20f), this, Connector.knobType.output , Color.red);
			
			// dropdown index choice
			choiceIndex = new int[2];
			
			// index from fsmVariables
			indexVarSelected = new int[2];
			
			// bool for button selected
			useInternalVar = new bool[2];
			
		}
		public override void DrawNode()
		{
			base.DrawNode();
			varGameObjects = fsmVariables.Where(q=> q.VariableType  == typeof(GameObject)).Select(x=>x.Name).ToArray();
			
			EditorGUILayout.BeginVertical("box");
			usePhysics2D = EditorGUILayout.Toggle("Use 2D Physics:" , usePhysics2D);
			
		   EditorGUILayout.BeginHorizontal();
			if (!useInternalVar[0]){
			targetObject = (GameObject)EditorGUILayout.ObjectField("we are searching for:", targetObject, typeof(GameObject), true);
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
				targetObject = null;
				if (useInternalVar[0]){
					
					useInternalVar[0] = false;
				}else{
					useInternalVar[0] = true;
				}
				
			}
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();
			targetTag = EditorGUILayout.TagField("Tag of the objects", targetTag);
			
			_objectLayerMask = EditorGUILayout.LayerField("The Layer searching for", _objectLayerMask);
 
			hearingRadius = EditorGUILayout.FloatField("How far away the unit hear: ",hearingRadius, EditorStyles.label);
			
			audibilityThreshold = EditorGUILayout.FloatField("Less likely the agent will be able to hear it: ",audibilityThreshold, EditorStyles.label);
			
			
			offset = EditorGUILayout.Vector3Field("The offset relative to the pivot position: ",offset);
			
 			
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical();
			var centeredStyle = GUI.skin.GetStyle("Box");
			centeredStyle.alignment = TextAnchor.UpperCenter;
			
			
			GUILayout.Label("Use Inverted => NOT Hear Object:");
			GUILayout.BeginArea (new Rect(140, 210, 20, 20));
			useInverted = EditorGUILayout.Toggle(useInverted);
			GUILayout.EndArea ();
			EditorGUILayout.EndVertical();
			
		}
		#endif
		public override void OnStart()
		{
			isRunning = true;
			objectLayerMask = 1 << _objectLayerMask;

			if (fsmVariables.Count > indexVarSelected [0]) {
				var temp = (FsmGameObject)fsmVariables [indexVarSelected [0]];
				if (temp != null)
					targetObject = temp.Value;
			}

			
		}
		public override Node.Task OnUpdate()
		{
			GameObject _target;
			_target = Hear();
			
			if (_target == null) {
				if (useInverted){
					return Task.Success;
				}else{
					return Task.Running;
				}
				
			}else{
				
				if (useInverted){
					return Task.Running;
					
				}else{
					
					if (useInternalVar[0]){
						fsmVariables[indexVarSelected[0]].SetValue(_target);
						
					}
					return Task.Success;
					
				}
				
				
			}
			
			
		}
		public GameObject Hear()
		{
        // If the target object is null then determine if there are any objects within hearing range based on the layer mask
			if (targetObject == null)
			{
				if (usePhysics2D)
				{
					returnedObject = MovementUtility.WithinHearingRange2D(self.transform, offset, audibilityThreshold, hearingRadius, objectLayerMask);
				}
				else
				{
					
					returnedObject = MovementUtility.WithinHearingRange(self.transform, offset, audibilityThreshold, hearingRadius, objectLayerMask);
				}
				
			}
			else
			{
				GameObject _target;
				if (!targetTag.Contains("Untagged"))
				{
					_target = GameObject.FindGameObjectWithTag(targetTag);
				}
				else
				{
					_target = targetObject;
				}
				if (_target == null) {
					_target = targetObject;
				}
				if (Vector3.Distance(targetObject.transform.position, self.transform.position) < hearingRadius)
				{
					returnedObject = MovementUtility.WithinHearingRange(self.transform, offset, audibilityThreshold, targetObject);
				}
			}
			if (returnedObject != null)
			{
				
				return returnedObject;
				
			}
			else {
				
				return null;
			}
			
			
		}
		 // Draw the hearing radius
		public override void OnDrawGizmos()
		{
#if UNITY_EDITOR
			float fieldOfViewAngle = 360f;
			var oldColor = UnityEditor.Handles.color;
			var color = Color.green;
			color.a = 0.1f;
			UnityEditor.Handles.color = color;
			
			var halfFOV = fieldOfViewAngle * 0.5f;
			var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? Vector3.forward : Vector3.up)) * (usePhysics2D ? self.transform.up : self.transform.forward);
			UnityEditor.Handles.DrawSolidArc(self.transform.TransformPoint(offset), (usePhysics2D ? self.transform.forward : self.transform.up), beginDirection, fieldOfViewAngle, hearingRadius);
			
			UnityEditor.Handles.color = oldColor;
#endif
			
		}
	}
	
}
