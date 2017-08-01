using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

namespace AxlPlay{
	
	[Node("Actions/Flee",true)]
public class FleeNode : Node {
	
	[Tooltip("The agent has fleed when the magnitude is greater than this value")]
	public float fleedDistance = 10;
	[Tooltip("The distance to look ahead when fleeing")]
	public float lookAheadDistance = 5;
	[Tooltip("The agent has arrived when they are less than the specified distance")]
	public float arrivedDistance = 2;
	
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	[Tooltip("Stopping Distance.")]
	public float StoppingDistance = 0.1f;
 	public UnityEngine.AI.NavMeshAgent _agent;
	
	public GameObject fleeGameObject;
	
  	public bool hasArrived;
	#if UNITY_EDITOR
	public override void Init()
	{
		base.Init();
		
		headerColor = Color.red;
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 280f;
		rect.height = 200f;
		// set node name
		title = "Flee";
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
		
		fleedDistance = EditorGUILayout.FloatField("Flee Distance:",fleedDistance, EditorStyles.label);
		
		lookAheadDistance = EditorGUILayout.FloatField("Look ahead distance:",lookAheadDistance, EditorStyles.label);
		
		arrivedDistance = EditorGUILayout.FloatField("Arrived Distance:",arrivedDistance, EditorStyles.label);
	 EditorGUILayout.BeginHorizontal();
		if (!useInternalVar[0]){
			fleeGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject to Evade:", fleeGameObject, typeof(GameObject), true);
		}else{
			
			 // to prevent is delete the variable from inspector
			if (choiceIndex[0] >= varGameObjects.Count()) {
				
				choiceIndex[0]  =0 ;
			}
			EditorGUILayout.LabelField("GameObject to Evade:",GUILayout.Width(120));
			
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
			fleeGameObject = null;
			if (useInternalVar[0]){
				
				useInternalVar[0] = false;
			}else{
				useInternalVar[0] = true;
			}
			
		}
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		StoppingDistance = EditorGUILayout.FloatField("Stopping Distance:",StoppingDistance, EditorStyles.label);
		
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
					fleeGameObject = temp.Value;
			}

		
		
		SetDestination(Target());
	}
	public override Node.Task OnUpdate()
	{
		SetDestination(Target());
		if (Vector3.Magnitude(self.transform.position - fleeGameObject.transform.position) > fleedDistance)
		{
			hasFinished = true;
            // finish event
			return Task.Success;
		}else{
			
			return Task.Running;
			
		}
	
		
	}
	// Flee in the opposite direction
	private Vector3 Target()
	{
		if (fleeGameObject == null) return Vector3.zero;
		return self.transform.position + (self.transform.position - fleeGameObject.transform.position).normalized * lookAheadDistance;
	}
	private bool SetDestination(Vector3 target)
	{
		if (_agent.destination == target)
		{
			return true;
		}
		if (_agent.SetDestination(target))
		{
			return true;
		}
		return false;
	}
	
	private bool HasArrived()
	{
		return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.001f;
	}
	 
}
}