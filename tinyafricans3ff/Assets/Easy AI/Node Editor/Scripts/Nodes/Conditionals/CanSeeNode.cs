using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System.Collections.Generic;

namespace AxlPlay{
	[Node("Conditionals/Can See",true)]
	public class CanSeeNode : Node {
		
	public bool usePhysics2D;
	//[Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
	//public GameObject targetObject;
	[Tooltip("The LayerMask of the objects that we are searching for")]
	public LayerMask objectLayerMask;
	[Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
	public LayerMask ignoreLayerMask;
	[Tooltip("The field of view angle of the agent (in degrees)")]
	public float fieldOfViewAngle = 90;
	[Tooltip("The distance that the agent can see")]
	public float viewDistance = 25;
	[Tooltip("The offset relative to the pivot position")]
	public Vector3 offset;
	[Tooltip("The target offset relative to the pivot position")]
	public Vector3 targetOffset;
 
	[Tooltip("The angle offset relative to the pivot position 2D")]
	public float angleOffset2D;
		
	public GameObject targetObject;	
		
	public GameObject returnedObject;
		
	public LayerMask _objectLayerMask;
	public LayerMask _ignoreLayerMask;
		
	public bool useInverted;
 		#if UNITY_EDITOR
	public override void Init()
	{
		base.Init();
		
		headerColor = Color.cyan;
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 280f;
		rect.height = 310f;
		// set node name
		title = "Can See Object";
		
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
		
		EditorGUILayout.BeginVertical("box");
		usePhysics2D = EditorGUILayout.Toggle("Use 2D Physics:" , usePhysics2D);
		
		angleOffset2D = EditorGUILayout.FloatField("The angle offset 2D: ",angleOffset2D, EditorStyles.label);
	 
		
	 EditorGUILayout.BeginHorizontal();
		 
		varGameObjects = fsmVariables.Where(q=> q.VariableType  == typeof(GameObject)).Select(x=>x.Name).ToArray();
		
	 if (!useInternalVar[0]){
	 	targetObject = (GameObject)EditorGUILayout.ObjectField("We are searching for:", targetObject, typeof(GameObject), true);
		 
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
						
						var temp = fsmVariables[i] as FsmGameObject;
						indexVarSelected[0] = i;
						targetObject = temp.Value;
 						
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
		
		_objectLayerMask = EditorGUILayout.LayerField("The Layer searching for", _objectLayerMask);
		
		_ignoreLayerMask =  EditorGUILayout.LayerField("The Layer to ignore", _ignoreLayerMask);

		fieldOfViewAngle = EditorGUILayout.FloatField("The field of view angle: ",fieldOfViewAngle, EditorStyles.label);
		
		viewDistance = EditorGUILayout.FloatField("The distance that the agent can see: ",viewDistance, EditorStyles.label);
		
		
		offset = EditorGUILayout.Vector3Field("The offset relative to the pivot position: ",offset);
		
		targetOffset = EditorGUILayout.Vector3Field("The target offset relative to the pivot position: ",targetOffset);
		
		EditorGUILayout.BeginHorizontal();
		
	
		if (!useInternalVar[1])
		returnedObject = (GameObject)EditorGUILayout.ObjectField("Returned Object:", returnedObject, typeof(GameObject), true);
		else{
		 
		 // to prevent is delete the variable from inspector
			if (choiceIndex[1] >= varGameObjects.Count()) {
				
				choiceIndex[1]  =0 ;
			}
		 
			EditorGUILayout.LabelField("Returned Object:",GUILayout.Width(120));
			if (varGameObjects.Count() > 0){
				choiceIndex[1]  = EditorGUILayout.Popup(choiceIndex[1],varGameObjects,GUILayout.Height(10),GUILayout.Width(110) );
				
				for (int i = 0; i < fsmVariables.Count; i++) {
					if (varGameObjects.Count() > 0 &&   fsmVariables[i].Name == varGameObjects[choiceIndex[1]] ){
						
						indexVarSelected[1] = i;
 					break;
					}
				}
			}
		} // end else
		 	
 
		if (useInternalVar[1]){
			GUI.color = Color.green;
		}else{
			GUI.color = Color.white;
		}
		
		if (GUILayout.Button ("S", GUILayout.Height(18), GUILayout.Width(18))){
			targetObject = null;
			if (useInternalVar[1]){
				
				useInternalVar[1] = false;
			}else{
				useInternalVar[1] = true;
			}
			
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		GUILayout.Label("Use Inverted => NOT See Object:");
		GUILayout.BeginArea (new Rect(140, 290, 20, 20));
		useInverted = EditorGUILayout.Toggle(useInverted);
		GUILayout.EndArea ();
		EditorGUILayout.EndVertical();

	}
		#endif
	public override void OnStart()
	{
		isRunning = true;
		objectLayerMask = 1 <<_objectLayerMask;
		ignoreLayerMask = 1 <<_ignoreLayerMask;
			if (fsmVariables.Count > indexVarSelected [1]) {
				var temp = (FsmGameObject)fsmVariables [indexVarSelected [1]];
				if (temp != null)
					targetObject = temp.Value;
			}
	}
	public override Node.Task OnUpdate()
	{
		GameObject _target;
		_target = CanSee();

		if (_target == null) {
				if (useInverted) {
					return Task.Success;
				} else {

				return Task.Running;
			}
			
		}else{
			
			if (useInverted){
				return Task.Running;
				
			}else{
				if (useInternalVar[1]){
					fsmVariables[indexVarSelected[1]].SetValue(_target);
			 
				}
				return Task.Success;
				
			}
			
			
		}
		
		
	}
		public GameObject CanSee()
		{
			if (usePhysics2D)
			{
            // If the target object is null then determine if there are any objects within sight based on the layer mask
				if (targetObject == null)
				{
					returnedObject = MovementUtility.WithinSight2D(self.transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask, targetOffset, angleOffset2D, ignoreLayerMask);
				}
				else { // If the target is not null then determine if that object is within sight
					returnedObject = MovementUtility.WithinSight2D(self.transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, angleOffset2D, ignoreLayerMask);
				}
			}
			else {
            // If the target object is null then determine if there are any objects within sight based on the layer mask
				if (targetObject == null)
				{
					returnedObject = MovementUtility.WithinSight(self.transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask, targetOffset, ignoreLayerMask);
				}
				else { // If the target is not null then determine if that object is within sight
					returnedObject = MovementUtility.WithinSight(self.transform, offset, fieldOfViewAngle, viewDistance, targetObject, targetOffset, ignoreLayerMask);
				}
			}
			if (returnedObject != null)
			{
            // Return success if an object was found
				return returnedObject;
			}
        // An object is not within sight so return failure
			return null;
			
			
		}
		public override void OnDrawGizmos()
		{
			DrawLineOfSight(self.transform, offset, fieldOfViewAngle, viewDistance, usePhysics2D);
		}
		public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, bool usePhysics2D)
		{
#if UNITY_EDITOR
			var oldColor = UnityEditor.Handles.color;
			var color = Color.yellow;
			color.a = 0.1f;
			UnityEditor.Handles.color = color;
			
			var halfFOV = fieldOfViewAngle * 0.5f;
			var beginDirection = Quaternion.AngleAxis(-halfFOV, (usePhysics2D ? Vector3.forward : Vector3.up)) * (usePhysics2D ? transform.up : transform.forward);
			UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(positionOffset), (usePhysics2D ? transform.forward : transform.up), beginDirection, fieldOfViewAngle, viewDistance);
			
			UnityEditor.Handles.color = oldColor;
#endif
		}
}
}