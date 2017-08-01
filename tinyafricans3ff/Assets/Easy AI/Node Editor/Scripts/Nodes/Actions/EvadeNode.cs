using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;


namespace AxlPlay{
	
[Node("Actions/Evade",true)]
	public class EvadeNode : Node {
		
	[Tooltip("The agent has evaded when the magnitude is greater than this value")]
	public float evadeDistance = 10;
	[Tooltip("The distance to look ahead when evading")]
	public float lookAheadDistance = 5;
	[Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
	public float targetDistPrediction = 20;
	[Tooltip("Multiplier for predicting the look ahead distance")]
	public float targetDistPredictionMult = 20;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	[Tooltip("Stopping Distance.")]
	public float StopDistance = 0.1f;
		
		public GameObject evadeGameObject;
	// The position of the target at the last frame
	private Vector3 targetPosition;
	private UnityEngine.AI.NavMeshAgent _agent;
	
		#if UNITY_EDITOR
	public override void Init()
	{
		base.Init();
		
		headerColor = Color.red;
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 280f;
		rect.height = 220f;
		// set node name
		title = "Evade";
		myType = type.action;
		
		connectors[0].Init(ConCorner.left, new Rect(0f, 65f, 20f, 20f), this,Connector.knobType.input , Color.red);
		connectors[1].Init(ConCorner.right, new Rect(0f, 65f, 20f, 20f), this, Connector.knobType.output , Color.cyan);
		
		
		// dropdown index choice
		choiceIndex = new int[1];
		
		// index from fsmVariables
		indexVarSelected = new int[1];
		
		// bool for button selected
		useInternalVar = new bool[1];
	}
		
	public override void DrawNode()
	{
		base.DrawNode();
		
 
		
		EditorGUILayout.BeginVertical("box");
		
		evadeDistance = EditorGUILayout.FloatField("Evade Distance magnitude:",evadeDistance, EditorStyles.label);
		
		lookAheadDistance = EditorGUILayout.FloatField("Look ahead distance:",lookAheadDistance, EditorStyles.label);
		
		targetDistPrediction = EditorGUILayout.FloatField("target Distance Prediction:",targetDistPrediction, EditorStyles.label);
		
		targetDistPredictionMult = EditorGUILayout.FloatField("Multiplier for predicting:",targetDistPredictionMult, EditorStyles.label);
		EditorGUILayout.BeginHorizontal();
		if (!useInternalVar[0]){
			evadeGameObject = (GameObject)EditorGUILayout.ObjectField("Object to Evade:", evadeGameObject, typeof(GameObject), true);
		}else{
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= varGameObjects.Count()) {
				
				choiceIndex[0]  =0 ;
			}
			EditorGUILayout.LabelField("Object to Evade:",GUILayout.Width(100));
			
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
			evadeGameObject = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		
		EditorGUILayout.EndHorizontal();
		
		StopDistance = EditorGUILayout.FloatField("Stopping Distance:",StopDistance, EditorStyles.label);
		
		AgentSpeed = EditorGUILayout.FloatField("Agent Speed:",AgentSpeed, EditorStyles.label);
		
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical();
		var centeredStyle = GUI.skin.GetStyle("Box");
		centeredStyle.alignment = TextAnchor.UpperCenter;
		
		
		GUILayout.Label("Use Returned GameObject as Target:");
		GUILayout.BeginArea (new Rect(110, 200, 20, 20));
		useReturnedObject = EditorGUILayout.Toggle(useReturnedObject);
		GUILayout.EndArea ();
		EditorGUILayout.EndVertical();
		
		
	}
		#endif
		public override void OnStart()
		{
			isRunning = true;
			hasFinished = false;
			if (!self.GetComponent<UnityEngine.AI.NavMeshAgent>()){
				
				self.AddComponent<UnityEngine.AI.NavMeshAgent>();
			}
			
			_agent = self.GetComponent<UnityEngine.AI.NavMeshAgent>(); 

			if (fsmVariables.Count > indexVarSelected [0]) {
				var temp = (FsmGameObject)fsmVariables [indexVarSelected [0]];
				if (temp != null)
					evadeGameObject = temp.Value;
			}

 
 		}
		public override Node.Task OnUpdate()
		{
			_agent.SetDestination(Target()); 
			
			if (HasArrived()){
				
				hasFinished = true;
				return Task.Success;
			}
			
			return Task.Running;
		}
		  // Evade in the opposite direction
		private Vector3 Target()
		{
        // Calculate the current distance to the target and the current speed
			var distance = (evadeGameObject.transform.position - self.transform.position).magnitude;
			var speed = _agent.velocity.magnitude;
			
			float futurePrediction = 0;
        // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
			if (speed <= distance / targetDistPrediction)
			{
				futurePrediction = targetDistPrediction;
			}
			else
			{
				futurePrediction = (distance / speed) * targetDistPredictionMult; // the prediction should be accurate enough
			}
			
        // Predict the future by taking the velocity of the target and multiply it by the future prediction
			var prevTargetPosition = targetPosition;
			targetPosition = evadeGameObject.transform.position;
			var position = targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
			
			return self.transform.position + (self.transform.position - position).normalized * lookAheadDistance;
		}
		private bool HasArrived()
		{
			return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.001f;
		}
}
}
