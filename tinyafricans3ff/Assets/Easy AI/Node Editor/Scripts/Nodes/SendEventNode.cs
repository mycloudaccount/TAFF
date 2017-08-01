using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AxlPlay{
[Node("Send Event",true)]
	public class SendEventNode : Node {
 	/// <summary>
        /// In order to allow access to SerializedProperties of this class, it has to be converted in
        /// a SerializedObject first
        /// </summary>
    #if UNITY_EDITOR
	public SerializedObject serialObj;
	
        /// <summary>
        /// For drawing a UnityEvent (Actions) on the Node, Actions must be available as SerializedProperty.
        /// It can then be drawn by EditorGUILayout.PropertyField
        /// </summary>
	public SerializedProperty ActionsSerial;
	private Rect newRect;
	void OnEnable() {
		serialObj = new SerializedObject (this);
		ActionsSerial = serialObj.FindProperty("Actions");
	}
		
	public override void Init()
	{
		base.Init();
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 220f;
		rect.height = 100f;
		// set node name
		title = "Send Event";
		
		myType = type.action;
		
		connectors[0].Init(ConCorner.left, new Rect(0f, 65f, 20f, 20f), this,Connector.knobType.input , Color.red);
		connectors[1].Init(ConCorner.right, new Rect(0f, 65f, 20f, 20f), this, Connector.knobType.output , Color.red);
		 
		
	}
	public override void DrawNode()
	{
		
	 
		
		GUI.color =  Color.red;
		GUILayout.BeginHorizontal (EditorStyles.helpBox);	
		
		
		EditorGUILayout.LabelField(title,EditorStyles.label );  //customSkin.label 
		GUI.color = Color.white;
		
		
		EditorGUILayout.EndHorizontal ();
		GUILayout.Space(10f);

	 
		// Turning this Node into a serialized Object to access its properties
		if (serialObj != null) serialObj.Update();
		
		EditorGUILayout.BeginVertical ();
	 
		EditorGUILayout.PropertyField(ActionsSerial);
		newRect = EditorGUILayout.GetControlRect();
		
		if (newRect.y  > 0){
			rect.height  =  100f + newRect.y;
			
		}
		EditorGUILayout.EndVertical (); 
		 
		
		serialObj.ApplyModifiedProperties();
	}
		#endif
		public override void OnStart()
		{
			isRunning = true;
			
			Actions.Invoke();
			
		}
		public override Node.Task OnUpdate()
		{
			
			return Task.Success;
		}
}
}
