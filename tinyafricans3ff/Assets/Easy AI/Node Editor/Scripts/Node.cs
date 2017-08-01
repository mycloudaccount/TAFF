using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AxlPlay {
	
public class Node : ScriptableObject {
	
	[HideInInspector]
	public int Id;
	// for show in the editor
	[HideInInspector]
	public bool isRunning;
	[HideInInspector]
	public bool hasBack;
	
 	
	// for OnTrigger Enter
	[HideInInspector]
	public Task currentNodeTask;
	
	// for use as returned gameobject ex: Can See
	//public GameObject target;
	[HideInInspector]
	public bool useReturnedObject;
	// for use in evade when Has Arrived
	[HideInInspector]
	public bool hasFinished;
	
	// the gameObject where the nodes are running
	[HideInInspector]
	public List<FsmVariable> fsmVariables;
	[HideInInspector]
	public GameObject self;
	/// Rect of the Node's draw position in Unity's EditorWindow
	[HideInInspector]
	public Rect rect;
	// Title of the Node
	[HideInInspector]
	public string title = "";
 
	/// <summary>
		/// A List of connectors of this Node
		/// </summary>
	 [HideInInspector]
	public List<Connector> connectors;

		public UnityEvent Actions;
	
	[HideInInspector]
	public Color headerColor;
	
	public enum Task {
		Success,
		Failure,
		Running
	}

	
	
	// for use internal variables
	[HideInInspector]
	public int[] choiceIndex;
	[HideInInspector]
	public bool[] useInternalVar;
	[HideInInspector]
	public int []indexVarSelected;	
	[HideInInspector]
	public string[] varGameObjects;
	
	
	//public SerializedObject serialObj;

	//public SerializedProperty ActionsSerial;
	
	//public UnityEvent Actions;
	/// <summary>
		/// The working process of the Node that happens during gamePlay
		/// </summary>
	 
	protected IEnumerator workRoutine;
		/// <summary>
		/// The type of the node
		/// </summary>
	[HideInInspector]
	public byte nodeType; // Bit1=StepNode (1), Bit2=EventNode (2), Bit3=CurrentNode (4)
	
	public enum type {
		
		action,
		conditional
		
	}
		[HideInInspector]
	public type myType;	
	
	public virtual void Init() {
 
		connectors = new List<Connector> ();
		
		fsmVariables = self.GetComponent<EasyAIFSM>().fsmVariables;
		
 	}
	#if UNITY_EDITOR
	public virtual void DrawNode(){
		
		
		////if internal variables change
		if (fsmVariables.Count != self.GetComponent<EasyAIFSM>().fsmVariables.Count) {
			
			fsmVariables = self.GetComponent<EasyAIFSM>().fsmVariables;
			
		}
		varGameObjects = fsmVariables.Where(q=> q.VariableType  == typeof(GameObject)).Select(x=>x.Name).ToArray();
		
		var centeredStyle = GUI.skin.GetStyle("Label");
		centeredStyle.alignment = TextAnchor.MiddleCenter;
		
		GUI.color =  headerColor;
		GUILayout.BeginHorizontal (EditorStyles.helpBox);	
		EditorGUILayout.LabelField(title,centeredStyle );   
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(10f);
		
	}
	#endif
	// for internal calculation
	public virtual void CalculateNode(){
	 
	}
	
	/// <summary>
		/// Draws all connectors of the Node into EditorWindow
		/// </summary>
		/// 

	#if UNITY_EDITOR
	public virtual void DrawConnectors() {    
		foreach (Connector c in connectors) {
			if (c != null) c.Draw();
		}
	}
	#endif
	public virtual void OnStart(){}
	public virtual Task OnUpdate(){ return Task.Running;}
	public virtual Task OnTriggerEnter(Collider other){return Task.Running;}
	public virtual Task OnTriggerStay(Collider other){return Task.Running;}
	public virtual Task OnTriggerExit(Collider other){return Task.Running;}
	public virtual Task OnCollisionEnter(Collision other){return Task.Running;}
	public virtual Task OnCollisionStay(Collision other){return Task.Running;}
	public virtual Task OnCollisionExit(Collision other){return Task.Running;}
	public virtual void OnDrawGizmos(){}
	
    /// <summary>
	// Sets the Node active in UnityEditor and shows its parameters in Inspector
	// </summary>
	#if UNITY_EDITOR
	public virtual void Activate() {		
		UnityEditor.Selection.activeObject = this;
	}
	#endif
	
 
}
}
