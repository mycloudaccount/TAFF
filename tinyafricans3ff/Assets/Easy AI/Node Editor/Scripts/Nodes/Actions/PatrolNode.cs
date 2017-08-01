using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;


namespace AxlPlay{
	
	[Node("Actions/Patrol",true)]
	public class PatrolNode : Node {
	
	[Tooltip("Should the agent patrol the waypoints randomly?")]
	public bool randomPatrol = false;
	[Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
	public float waypointPauseDuration = 0f;
	[Tooltip("The Agent speed.")]
	public float AgentSpeed = 3.5f;
	[Tooltip("The waypoints to move to")]
	
	public int numberOfWaypoints;
		
	 // The current index that we are heading towards within the waypoints array
	private int waypointIndex;
	private float waypointReachedTime;
	private float arrivedDistance = 1f;
	private UnityEngine.AI.NavMeshAgent _agent;

 
	public List<GameObject> waypoints;
	
 
	private Rect newRect;
		
		[HideInInspector]
		[SerializeField]
		private int count;
		#if UNITY_EDITOR
	public override void Init()
	{
		base.Init();
		
		waypoints = new List<GameObject>();
		count = 0;
		headerColor = Color.red;
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 240f;
		rect.height = 200f;
		// set node name
		title = "Patrol";
		myType = type.action;
		
		connectors[0].Init(ConCorner.left, new Rect(0f, 65f, 20f, 20f), this,Connector.knobType.input , Color.red);
		connectors[1].Init(ConCorner.right, new Rect(0f, 65f, 20f, 20f), this, Connector.knobType.output , Color.cyan);
		
		
	}
		
		public override void DrawNode()
		{
			base.DrawNode();
			
			EditorGUILayout.BeginVertical("box");

			randomPatrol = EditorGUILayout.Toggle ("Random Patrol", randomPatrol);
		 
			waypointPauseDuration = EditorGUILayout.FloatField("Waypoint Pause Duration:",waypointPauseDuration, EditorStyles.label);
 
			AgentSpeed = EditorGUILayout.FloatField("Agent Speed:",AgentSpeed, EditorStyles.label);

			EditorGUILayout.EndVertical();
	
			GUILayout.Space(10);
   			
			EditorGUILayout.BeginVertical ();
			
			count = EditorGUILayout.IntField("Waypoints: " ,count);
			
  			
			if (count < waypoints.Count){
				
				var temp = waypoints.Count -1;
				waypoints.RemoveAt(temp);
			}
			
			
			for (int i = 0; i < count; i++) {
				if (count > waypoints.Count){
					
					waypoints.Add(null);
				}else if (count == waypoints.Count) {
					
					waypoints[i] = (GameObject)EditorGUILayout.ObjectField("Waypoint : ", waypoints[i], typeof(GameObject), true);
				}
				
			}
	 
 			newRect = EditorGUILayout.GetControlRect();
			
			if (newRect.y  > 0){
				rect.height  =  50f + newRect.y;
				
			}
			EditorGUILayout.EndVertical (); 
 
			
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
			
			_agent.Resume();
        	// initially move towards the closest waypoint
			float distance = Mathf.Infinity;
			float localDistance;
			for (int i = 0; i < waypoints.Count; ++i)
			{
				if ((localDistance = Vector3.Magnitude(self.transform.position - waypoints[i].transform.position)) < distance)
				{
					distance = localDistance;
					waypointIndex = i;
				}
			}
			waypointReachedTime = -1;
			_agent.SetDestination(Target());
		}
		public override Node.Task OnUpdate()
		{
			if (HasArrived())
			{
				if (waypointReachedTime == -1)
				{
					waypointReachedTime = Time.time;
				}
				
				
            // wait the required duration before switching waypoints.
				if (waypointReachedTime + waypointPauseDuration <= Time.time)
				{
					if (randomPatrol)
					{
						if (waypoints.Count == 1)
						{
							waypointIndex = 0;
						}
						else
						{
                        // prevent the same waypoint from being selected
							var newWaypointIndex = waypointIndex;
							while (newWaypointIndex == waypointIndex)
							{
								newWaypointIndex = Random.Range(0, waypoints.Count);
							}
							waypointIndex = newWaypointIndex;
						}
					}
					else
					{
						waypointIndex = (waypointIndex + 1) % waypoints.Count;
					}
					_agent.SetDestination(Target());
					waypointReachedTime = -1;
				}
			}
			return Task.Running;
		}
		
		// Return the current waypoint index position
		private Vector3 Target()
		{
			if (waypointIndex >= waypoints.Count)
			{
				return self.transform.position;
			}
			return waypoints[waypointIndex].transform.position;
		}
		
		bool HasArrived()
		{
			if (_agent == null) {
				_agent = self.GetComponent<UnityEngine.AI.NavMeshAgent>(); 
				return false;
			}
			var direction = (_agent.destination - self.transform.position);
			// Do not account for the y difference if it is close to zero.
			if (Mathf.Abs(direction.y) < 0.1f)
			{
				direction.y = 0;
			}
			return !_agent.pathPending && direction.magnitude <= arrivedDistance;
		}
 
    // Draw a gizmo indicating a patrol 
		public override void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (waypoints == null)
		{
			return;
		}
		var oldColor = UnityEditor.Handles.color;
		UnityEditor.Handles.color = Color.yellow;
		for (int i = 0; i < waypoints.Count; ++i)
		{
			if (waypoints[i] != null)
			{
				UnityEditor.Handles.SphereCap(0, waypoints[i].transform.position, waypoints[i].transform.rotation, 1);
			}
		}
		UnityEditor.Handles.color = oldColor;
#endif
	}
}
	
}
