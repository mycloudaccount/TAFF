using UnityEngine;
using System.Collections;
using AxlPlay;
public class Write3DText : MonoBehaviour {

    public TextMesh text;
    private EasyAIFSM easyAIFSM;

    void Awake()
    {
        easyAIFSM = GetComponent<EasyAIFSM>();

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(easyAIFSM != null)
        {
            text.text = easyAIFSM.curNode.title;
        }
	}
}
