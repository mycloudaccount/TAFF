using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AxlPlay{
[Node("Conditionals/Timer",true)]
public class TimerNode : Node {
	
	 public float timer;
	 private float lastTime;
	#if UNITY_EDITOR
	 public override void Init()
	{
		base.Init();
		
		headerColor = Color.cyan	;
		
		for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
		
		////  set the node side
		rect.width = 200f;
		rect.height = 85f;
		// set node name
		title = "Timer";
		
		myType = type.conditional;
		
		connectors[0].Init(ConCorner.left, new Rect(0f, 35f, 20f, 20f), this,Connector.knobType.input , Color.cyan);
		connectors[1].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.output , Color.red);
		
		
	}
	public override void DrawNode()
	{
		base.DrawNode();
		
		EditorGUILayout.BeginVertical("box");
		
		timer = EditorGUILayout.FloatField("Time: ",timer, EditorStyles.label);
	 
		
		EditorGUILayout.EndVertical();
		
		
	}
	#endif
	public override void OnStart()
	{
		isRunning = true;
		lastTime = timer;
	}
	public override Node.Task OnUpdate()
	{
		timer = timer - Time.deltaTime;
		
	 
			if (timer <= 0) {
				timer = lastTime;
				return Task.Success;
			} 
		
			return Task.Running;
			
	}
 
		
		
		
}
}

