using UnityEngine;
using System.Collections;
using AxlPlay;

[AddComponentMenu ("Easy AI/Formation/Agent")]
public class AgentScript : MonoBehaviour
{
	#region Public Variables

	// position in formation of the agent
	[HideInInspector]
	public int index;


	[HideInInspector]
	public bool isLeader;

	[Tooltip ("For test purposes")]
	public bool isDead;

	#endregion

	#region Private Variables

	private Agents _agent;

	private Vector3 target;
	[HideInInspector]
	public UnityEngine.AI.NavMeshAgent navMeshAgent;


	#endregion

	// init FSM
	public enum States
	{
		Idle,
		goPos,
		inPos,
		goLeader,
		LeaderArrived,
		goToLastLeaderPos,
		Dead
	}

	public StateMachine<States> fsm;

	private int notificationCount = 0;

	void Awake ()
	{
		notificationCount = 0;
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();

		//Initialize State Machine Engine		
		fsm = StateMachine<States>.Initialize (this);
	}

	void OnEnable ()
	{
		notificationCount = 0;
	}

	void Start ()
	{
		if (gameObject.tag.Contains ("Untagged")) {
			Debug.Log ("You need to set the tag to the agent. => " + gameObject.name);
			return;
		}
		// create a notification center delegate
		NotificationCenter.DefaultCenter.AddObserver (this, "AgentInPosition");

		NotificationCenter.DefaultCenter.AddObserver (this, "LeaderArrived");

		NotificationCenter.DefaultCenter.AddObserver (this, "agentIsDead");

		//Initialize State Machine Engine		
		fsm.ChangeState (States.Idle);
	}

	void Update ()
	{


	}

	IEnumerator Idle_Enter ()
	{

		// wait for create formation  => AgentManager
		yield return new WaitForSeconds (2f);

		index = getMyIndex ();


		// is 0 is Leader
		if (index == 0) {
			navMeshAgent.speed = FormationManager.current.formationSpeed;

		}
		//  si no hay agentes en el manager no hacer nada
		if (FormationManager.current.agentsNum > 0 && isLeader == false)
			fsm.ChangeState (States.goPos);

	}

	void goPos_Update ()
	{
		target = FormationManager.current.GetPosition (index);
		navMeshAgent.SetDestination (target);
		if (HasArrived ()) {
			fsm.ChangeState (States.inPos);
		}
		if (isDead) {
			fsm.ChangeState (States.Dead);
		}
	}
	// already in position move formation to target
	void inPos_Enter ()
	{
		NotificationCenter.DefaultCenter.PostNotification (this, "AgentInPosition");
	}

	void inPos_Update ()
	{
		target = FormationManager.current.GetPosition (index);
		navMeshAgent.SetDestination (target);
		if (isDead) {
			fsm.ChangeState (States.Dead);
		}
	}
	// this is for future release
	void goLeader_Enter ()
	{
		navMeshAgent.SetDestination (FormationManager.current.target.transform.position);
		Debug.Log ("goLeader_Enter");
	}

	void goLeader_Update ()
	{
		if (isDead) {
			fsm.ChangeState (States.Dead);
		}
		if (HasArrived ()) {
			fsm.ChangeState (States.LeaderArrived);
		}

	}

	void LeaderArrived_Enter ()
	{
		NotificationCenter.DefaultCenter.PostNotification (this, "LeaderArrived");
	}

	void goToLastLeaderPos_Enter ()
	{

		navMeshAgent.SetDestination (FormationManager.current.LastLeaderPos);

	}

	void goToLastLeaderPos_Update ()
	{
		if (HasArrived ()) {
			fsm.ChangeState (States.goLeader);

		}
	}

	void Dead_Enter ()
	{
		navMeshAgent.Stop ();
		if (isDead) {
			if (isLeader) {
				FormationManager.current.LastLeaderPos = transform.position;
			}
			FormationManager.current.LoadAgentList ();
			NotificationCenter.DefaultCenter.PostNotification (this, "agentIsDead");

		}

	}
	// call when agent is in position
	void AgentInPosition (NotificationCenter.Notification note)
	{
		notificationCount++;
		if (notificationCount == FormationManager.current.agentsNum - 1) {

			Debug.Log ("All in position.");

			if (isLeader) {
				fsm.ChangeState (States.goLeader);
			}

		}

	}

	void agentIsDead ()
	{
		index = getMyIndex ();

		if (index == 0) {

			Renderer rend = GetComponent<Renderer> ();

			rend.material.color = Color.red;
			navMeshAgent.speed = FormationManager.current.formationSpeed;

			fsm.ChangeState (States.goLeader, StateTransition.Overwrite);
		}
	}

	void LeaderArrived ()
	{
		Debug.Log ("Leader Arrived => All agent waiting for order ex: shoot target ...");
	}

	private int getMyIndex ()
	{

		// find my index
		foreach (var item in FormationManager.current.listAgents) {
			// assign a position to the agent
			if (item.go == gameObject) {
				index = item.idAgent;
				return index;
			}
		}
		return -1;

	}

	private bool HasArrived ()
	{
		return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.001f;
	}

}


