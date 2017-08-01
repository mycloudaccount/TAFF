using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

using UnityEngine.EventSystems;  // in order to use" IBeginDragHandler,IDragHandler,etc"
using UnityEngine.Events; 
using AxlPlay;

namespace AxlPlay {
	
	public class EasyAIFSM : MonoBehaviour {
		
		[HideInInspector]
	public List<Node> nodes;
 
		[HideInInspector]
    public List<FsmVariable> fsmVariables;
	
	[HideInInspector]
	public float zoomFactor = 1.0f;
 
	
	private bool initialized;
	
        [HideInInspector]
	public Node curNode;
	
	private List<Node> condiotionalNodes;
	
	private bool deleteConditional;
	
	private float _timer;
	public void Init() {
		if (nodes == null) {
			nodes = new List<Node>();
		}
		if (fsmVariables == null) {
			fsmVariables = new List<FsmVariable>();
		}
	}
	void Awake(){
		
		condiotionalNodes = new List<Node>();

		
	}
	// Use this for initialization
	void Start () {

		for (int i = 0; i < nodes.Count; i++) {
	
			if (nodes[i] is StartAINode ) {
				// get next node 
				if ( nodes[i].connectors[0].connections.Count > 0 ){
				 
					curNode = nodes[i].connectors[0].connections[0].homeNode;
					// get next conditionals 
						for (int x = 0; x < curNode.connectors [1].connections.Count; x++) {
							if (curNode.connectors [1].connections [x].homeNode.myType == Node.type.conditional) {
								condiotionalNodes.Add (curNode.connectors [1].connections [x].homeNode);
								curNode.connectors [1].connections [x].homeNode.OnStart ();

							} 
						}
						//if(curNode.myType == Node.type.conditional)
				
				}
			}
			}// end for

		curNode.OnStart();
		initialized = true;
	}
 

	void Update(){
		
		// actions
		
		if (!initialized){
			curNode.OnStart();
			foreach (var item in condiotionalNodes)
			{
				item.OnStart();
			}
			initialized = true;
		}
		if( curNode.OnUpdate() == Node.Task.Success){
			
			if (curNode.connectors[1].connections.Count > 0)
				NextState(curNode.connectors[1].connections[0].homeNode);
			
		} 

		// conditionals
	
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnUpdate() == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
	 

	}

	public void NextState(Node _nextNode){

		curNode.isRunning = false;
			if (_nextNode.myType == Node.type.action) {
				//Debug.Log ("cur node -> " + curNode.title + " cur node type -> " + curNode.myType);
			//	Debug.Log ("next node -> " + _nextNode.title + "next node type => " + _nextNode.myType);
				curNode = _nextNode;

				for (int i = 0; i < condiotionalNodes.Count; i++) {
			
					condiotionalNodes [i].isRunning = false;

				}
				condiotionalNodes = new List<Node> ();
	 
				// get next node 
				if (curNode.connectors [1].connections.Count > 0) {
		
					// get next actions 
					for (int x = 0; x < curNode.connectors [1].connections.Count; x++) {
						if (curNode.connectors [1].connections [x].homeNode.myType == Node.type.conditional) {
							condiotionalNodes.Add (curNode.connectors [1].connections [x].homeNode);
						}
					}
				}
				initialized = false;
			}
	}
	void OnTriggerEnter(Collider other){
		
		// conditionals
		
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnTriggerEnter(other) == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
	}
	void OnTriggerStay(Collider other){
		// conditionals
		
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnTriggerStay(other) == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
	}
	void OnTriggerExit(Collider other){
		// conditionals
		
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnTriggerExit(other) == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
		
	}
	void OnCollisionEnter(Collision other){
		
		// conditionals
		
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnCollisionEnter(other) == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
		
	}
	void OnCollisionStay(Collision other){
		 // conditionals
		
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnCollisionStay(other) == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
	}
	void OnCollisionExit(Collision other){
		
		 // conditionals
		
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnCollisionExit(other) == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
	}
	void OnTrigger(Collider other){
		
	}
	void OnCollison(Collider other){
		// conditionals
		
		for (int i = 0; i < condiotionalNodes.Count; i++) {
			
			if(condiotionalNodes[i].OnTriggerEnter(other) == Node.Task.Success){
				
				condiotionalNodes[i].isRunning = false;		
				if(condiotionalNodes[i].connectors[1].connections.Count > 0) {
					NextState(condiotionalNodes[i].connectors[1].connections[0].homeNode);
				}else{
					condiotionalNodes.RemoveAt(i);
				}
				
			} 
		}
	}
	void OnDrawGizmos(){
		
	 foreach (var item in nodes)
	 {
	 	item.OnDrawGizmos();
	 }
	}
 
}
	
	
}
