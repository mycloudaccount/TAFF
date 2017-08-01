using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AxlPlay{

[Node("Actions/Wander",true)]
public class WanderNode : Node {
	
	[Tooltip("How far ahead of the current position to look ahead for a wander")]
	public float wanderDistance = 20;
	[Tooltip("The amount that the agent rotates direction")]
	public float wanderRate = 2;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	
	[SerializeField]
	private UnityEngine.AI.NavMeshAgent _agent;
	#if UNITY_EDITOR
	public override void Init()
	{
		base.Init();
		
		headerColor = Color.red;
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 200f;
		rect.height = 110f;
		// set node name
		title = "Wander";
		myType = type.action;
		
		connectors[0].Init(ConCorner.left, new Rect(0f, 65f, 20f, 20f), this,Connector.knobType.input , Color.red);
		connectors[1].Init(ConCorner.right, new Rect(0f, 65f, 20f, 20f), this, Connector.knobType.output , Color.cyan);
		
		
	}
	public override void DrawNode()
	{
		base.DrawNode();

		EditorGUILayout.BeginVertical("box");
		 
		wanderDistance = EditorGUILayout.FloatField( new GUIContent("Wander Distance:","How far ahead of the current position to look ahead for a wander"),wanderDistance, EditorStyles.label);
		wanderRate = EditorGUILayout.FloatField("Wander Rate:",wanderRate, EditorStyles.label);
		AgentSpeed = EditorGUILayout.FloatField("Agent Speed:",AgentSpeed, EditorStyles.label);
		
		EditorGUILayout.EndVertical();

		
	}
	#endif
	public override void OnStart()
	{
		isRunning = true;

		if (!self.GetComponent<UnityEngine.AI.NavMeshAgent>()){
			
			self.AddComponent<UnityEngine.AI.NavMeshAgent>();
		}
		
		_agent = self.GetComponent<UnityEngine.AI.NavMeshAgent>();

		SetDestination(Target());
		
		
	}
	public override Node.Task OnUpdate()
	{
		if (HasArrived())
		{
			SetDestination(Target());
		}
 		return Task.Running;
	}
	private Vector3 Target()
	{
        // point in a new random direction and then multiply that by the wander distance
		var direction = self.transform.forward + Random.insideUnitSphere * wanderRate;
		return self.transform.position + direction.normalized * wanderDistance;
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
		if (_agent == null) _agent = self.GetComponent<UnityEngine.AI.NavMeshAgent>();
		return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.001f;
	}
	
}
}
