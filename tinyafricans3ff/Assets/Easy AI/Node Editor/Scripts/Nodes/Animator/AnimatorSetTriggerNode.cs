using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AxlPlay{
	
	[Node("Actions/Animator/Set Trigger",true)]
	public class AnimatorSetTriggerNode : Node {
		
		public string Parameter;
		#if UNITY_EDITOR
		public override void Init()
		{
			base.Init();
			headerColor = Color.red;
			for (int i=0; i< 2 ; i++) connectors.Add (ScriptableObject.CreateInstance<Connector>());
			
		////  set the node side
			rect.width = 220f;
			rect.height = 90F;
		// set node name
			title = "Animator Set Trigger";
			myType = type.action;

			connectors[0].Init(ConCorner.left, new Rect(0f, 35f, 20f, 20f), this,Connector.knobType.input , Color.red);
			connectors[1].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.output , Color.red);
			
			
		}
		public override void DrawNode()
		{
			
			base.DrawNode();
			Parameter = EditorGUILayout.TextField("Animator Parameter",Parameter);
			
		}
		#endif
		public override void OnStart()
		{
			isRunning = true;

		}
		public override Node.Task OnUpdate()
		{
			var _anim = self.GetComponent<Animator>();
			if(_anim != null)
				_anim.SetTrigger(Parameter);

			return Task.Success;
		}
	}
	
}
