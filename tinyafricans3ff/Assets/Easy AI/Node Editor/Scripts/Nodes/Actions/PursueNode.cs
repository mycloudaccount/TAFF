using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

namespace AxlPlay{
[Node("Actions/Pursue",true)]
public class PursueNode : Node {
	[Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
	public float targetDistPrediction = 20;
	[Tooltip("Multiplier for predicting the look ahead distance")]
	public float targetDistPredictionMult = 20;
	 
	[Tooltip("The agent has arrived when they are less than the specified distance")]
	public float arrivedDistance = 0.1f;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	
	public GameObject pursueGameObject;
	
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
		rect.height = 180f;
		// set node name
		title = "Pursue";
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
		
		targetDistPrediction = EditorGUILayout.FloatField("Target Distance Prediction:",targetDistPrediction, EditorStyles.label);
		targetDistPredictionMult = EditorGUILayout.FloatField("Predicting the look ahead distance:",targetDistPredictionMult, EditorStyles.label);
		EditorGUILayout.BeginHorizontal();
		if (!useInternalVar[0]){
		    pursueGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject to Pursue:", pursueGameObject, typeof(GameObject), true);
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
			pursueGameObject = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		arrivedDistance = EditorGUILayout.FloatField("Arrived Distance:",arrivedDistance, EditorStyles.label);
		
		AgentSpeed = EditorGUILayout.FloatField("Agent Speed:",AgentSpeed, EditorStyles.label);
		
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
					pursueGameObject = temp.Value;
			}

            if (pursueGameObject == null)
            {

                Debug.Log("Please set the target to Pursue");
            }
            else {
                _agent.SetDestination(Target());
            }
	 
		
	}
	public override Node.Task OnUpdate()
	{
		
		// prevent if target == null 
		if(pursueGameObject == null) return Task.Failure;
		
		_agent.SetDestination(Target());
		if (HasArrived())
		{
			hasFinished = true;
			
			return Task.Success;
		}
		
		return Task.Running;
	}
	public bool HasArrived()
	{
		
		var direction = (_agent.destination - self.transform.position);
        // Do not account for the y difference if it is close to zero.
		if (Mathf.Abs(direction.y) < 0.1f)
		{
			direction.y = 0;
		}
		return !_agent.pathPending && direction.magnitude <= arrivedDistance;
	}
    // Predict the position of the target
	private Vector3 Target()
	{
        // Calculate the current distance to the target and the current speed
		var distance = (pursueGameObject.transform.position - self.transform.position).magnitude;
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
		targetPosition = pursueGameObject.transform.position;
		return targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
	}
	 
}
}
