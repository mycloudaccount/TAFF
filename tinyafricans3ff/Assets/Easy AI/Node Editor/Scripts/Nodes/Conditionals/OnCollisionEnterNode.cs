using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AxlPlay
{

    [Node("Conditionals/On Collision Enter", true)]
    public class OnCollisionEnterNode : Node
    {

        public string Tag;
#if UNITY_EDITOR

        public override void Init()
        {
            base.Init();

            headerColor = Color.cyan;

            for (int i = 0; i < 2; i++) connectors.Add(ScriptableObject.CreateInstance<Connector>());

            ////  set the node side
            rect.width = 200f;
            rect.height = 85f;
            // set node name
            title = "On Collision Enter";
            myType = type.conditional;

            connectors[0].Init(ConCorner.left, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.input, Color.cyan);
            connectors[1].Init(ConCorner.right, new Rect(0f, 35f, 20f, 20f), this, Connector.knobType.output, Color.red);


        }
        public override void DrawNode()
        {
            base.DrawNode();
            Tag = EditorGUILayout.TagField("Tag", Tag);

        }
#endif
        public override void OnStart()
        {
            isRunning = true;
        }
        public override Node.Task OnUpdate()
        {
            return Task.Running;
        }

        public override Task OnCollisionEnter(Collision other)
        {

            if (other.gameObject.tag == Tag)
            {

                return Task.Success;

            }
            else
            {
                return Task.Running;

            }

        }

    }

}
