using UnityEngine;
using System.Collections;

public class NavAgentMover : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent navMeshAgent;
	private Animator animator;
	private PlayMakerFSM[] fsms;
	private PlayMakerFSM AnimationStateFSM;
	private PlayMakerFSM NPCPostionControllerFSM;
	private float IdleState = 0.0f;
	private float Speed = 0.0f;
	private float InitialAgentSpeed = 0.5f;
	private GameObject CurrentWayPoint;

	void Start() {
	
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		// verify that the navmesh agent is not null.  if it is
		// look to the parent for the navmesh agent (sometimes)
		// the object and within a nav mesh agent.
		if (navMeshAgent == null) {
			navMeshAgent = transform.parent.GetComponent<UnityEngine.AI.NavMeshAgent>();
		}

		animator = GetComponent<Animator> ();
		fsms = GetComponents<PlayMakerFSM>();
	
		foreach (PlayMakerFSM fsm in fsms)
		{
			if (fsm.FsmName == "AnimationStateFSM") {
				AnimationStateFSM = fsm;
			} else if (fsm.FsmName == "PositionControllerFSM") {
				NPCPostionControllerFSM = fsm;
			}
		}

	}

	void Update() {

		// For debugging only - use the moouse to move the navemesh agent around
		RaycastHit hit;
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				navMeshAgent.SetDestination(hit.point);

		}

		// handle the target location of the navmesh using CurrentWayPoint
		CurrentWayPoint = NPCPostionControllerFSM.FsmVariables.GetFsmGameObject ("CurrentWayPoint").Value;
		if (CurrentWayPoint != null) {
			navMeshAgent.SetDestination(CurrentWayPoint.transform.position);
		}

		// handle Idle states (the FSM is doing all of the work)
		if (AnimationStateFSM != null) {
			IdleState = AnimationStateFSM.FsmVariables.GetFsmFloat ("IdleState").Value;
			animator.SetFloat ("IdleState", IdleState);
		}

		// handle Speed (TODO: InitialAgentSpeed should not be hard coded)
		if (AnimationStateFSM != null) {
			Speed = navMeshAgent.desiredVelocity.magnitude / InitialAgentSpeed;
			AnimationStateFSM.FsmVariables.GetFsmFloat ("Speed").Value = Speed;
			animator.SetFloat ("Speed", Speed);
		}

	}

}
