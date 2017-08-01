using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;

public class HeroMotionController : MonoBehaviour
{
	protected UnityEngine.AI.NavMeshAgent agent;
	protected Animator animator;
	protected Locomotion locomotion;

	private float newIdleStateValue = 0.0f;
	private int idleStateId = 0;
	public float idleStateDampTime = 0.1f;
	public float rotationToObjectSpeed = 2.0f;

	private GameObject objectToLookAt = null;

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.updateRotation = false;

		animator = GetComponent<Animator>();
		locomotion = new Locomotion(animator);
	}

	// Update is called once per frame
	void Update () 
	{
		SetupAgentLocomotion();

		// get the IdleState value
		float currentIdleStateValue = animator.GetFloat ("IdleState");
		if (Math.Abs(currentIdleStateValue - newIdleStateValue) > 0.02) {
			animator.SetFloat(idleStateId, newIdleStateValue, idleStateDampTime, Time.deltaTime);
			//Debug.Log ("currentIdleStateValue: " + currentIdleStateValue);
			//Debug.Log ("newIdleStateValue: " + newIdleStateValue);
		}

		// rotate if needed
		if (objectToLookAt != null) {
			LookAtObject ();
		}

	}

	protected void SetupAgentLocomotion()
	{
		if (AgentDone())
		{
			locomotion.Do(0, 0);
		}
		else
		{
			float speed = agent.desiredVelocity.magnitude;

			Vector3 velocity = Quaternion.Inverse(transform.rotation) * agent.desiredVelocity;

			float angle = Mathf.Atan2(velocity.x, velocity.z) * 180.0f / 3.14159f;

			locomotion.Do(speed, angle);
		}
	}

	void OnAnimatorMove()
	{
		agent.velocity = animator.deltaPosition / Time.deltaTime;
		transform.rotation = animator.rootRotation;
	}

	public void ChangeRandomIdleAnim () {
		newIdleStateValue = UnityEngine.Random.Range (0, 3) * 1.0f;
		//Debug.Log ("new IdleState: " + newIdleStateValue);
		StartCoroutine (ChangeIdleAnim ());
	}

	IEnumerator ChangeIdleAnim () {
		yield return new WaitForSeconds (5.0f);
		idleStateId = Animator.StringToHash("IdleState");
	}

	protected bool AgentDone()
	{
		return !agent.pathPending && AgentStopping();
	}

	protected bool AgentStopping()
	{
		return agent.remainingDistance <= agent.stoppingDistance;
	}

	public void TriggerRandomAnim () {
		int animTriggerKey = UnityEngine.Random.Range (0, 3);
		string animTriggerValue;
		switch (animTriggerKey)
		{
		case 0:
			animTriggerValue = "TalkState";
			break;
		case 1:
			animTriggerValue = "WaveState";
			break;
		case 2:
			animTriggerValue = "VictoryState";
			break;
		default:
			animTriggerValue = "VictoryState";
			break;
		}
		StartCoroutine (TriggerAnim (animTriggerValue, 5.0f));
	}

	IEnumerator TriggerAnim (string name, float delay) {
		yield return new WaitForSeconds (delay);
		animator.SetTrigger (name);
	}

	public void SetObjectToLookAt (GameObject lookAt) {
		objectToLookAt = lookAt;
	}

	private void LookAtObject () {
		Vector3 direction = (objectToLookAt.transform.position - gameObject.transform.position).normalized;
		Quaternion lookAtRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookAtRotation, Time.deltaTime * rotationToObjectSpeed);

		float lookAtRotationAngle = Mathf.Abs (transform.rotation.eulerAngles.y - lookAtRotation.eulerAngles.y);
		animator.SetFloat ("LookAtRotationAngle", lookAtRotationAngle);
		//Debug.Log ("rotation diff: " + lookAtRotationAngle);

		if (lookAtRotationAngle <= 1.0f) { 
			objectToLookAt = null;
			animator.SetFloat ("LookAtRotationAngle", 0.0f);
		}
	}

}

