using UnityEngine;
using System.Collections;
using UnityEditor;
 
 namespace AxlPlay {
public class SideWindow {
 /// <summary>
		/// Drawing position in EditorWindow
		/// </summary>
	public Rect rect;
	
		/// <summary>
		/// Monitor Text: The current active node
		/// </summary>
	public string currentNode;
	
		/// <summary>
		/// Monitor Text: The current active "global" node
		/// </summary>
	public string globalNode;
	
		/// <summary>
		/// The start node, from which the Tutorial will start
		/// </summary>
	public string startNode = "";
	
		/// <summary>
		/// Open or Closed. To see more of the TurorialDesigner Canvas, the SideWindow can be put aside
		/// </summary>
	public bool opened;	    
	
		// Different formatting styles for elements within SideWindow
	private GUIStyle sideWindowStyle, labelStyle, labelBold, labelBoldBlack, monitorStyle, monitorText;
		// Since this class doesn's use GUILayout, but GUI instead - this value is used for calculating
		// the elements y positions within Draw function
	float stackHeight;
	
	 
	int _choiceIndex = 0;
	string varName ="";
	
	Vector2 scrollPos;
	    /// <summary>
	    /// Initializes a new instance of the <see cref="TutorialDesigner.SideWindow"/> class.
	    /// </summary>
	public SideWindow() {
		sideWindowStyle = new GUIStyle ();
		//labelStyle = TutorialDesigner.TutorialEditor.customSkin.label;
		//labelBold = new GUIStyle(labelStyle);
		//labelBold.fontStyle = FontStyle.Bold;
		//labelBoldBlack = new GUIStyle(labelBold);
		//labelBoldBlack.normal.textColor = Color.black;
		//monitorStyle = new GUIStyle(TutorialDesigner.TutorialEditor.customSkin.textArea);
		//monitorStyle.stretchHeight = false;
		//monitorText = new GUIStyle ();
		//monitorText.normal.textColor = Color.black;
		//monitorText.wordWrap = true;
		sideWindowStyle.normal.background = ColorToTex(new Color(0.23f, 0.23f, 0.23f));
		sideWindowStyle.padding = new RectOffset (5, 5, 5, 5);
		opened = true;
	}
	
        // Create Button that shows the About Window
	private void AboutButton() {            
		if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),"About")){
			
			
		}
	}
	
		/// <summary>
		/// Basic Draw routine. Called in OnGUI in TutorialEditor
		/// </summary>
		/// <param name="position">Absolute position</param>
	public void Draw(Rect position , EasyAIFSM coreSystem) {
		float width = opened ? 220f : 25f;
		rect = new Rect (position.width - width, 0f, width, position.height); 
		stackHeight = 0f;
		
		GUI.BeginGroup(rect, sideWindowStyle);
		 
			// Open- / Close Button
		if (GUI.Button (new Rect( opened ? 10f:5f, stackHeight += 5f, opened ? 190f : 15f, 20f),
			new GUIContent (opened ? ">> Close >>" : "<", opened ? "Close Sidewindow" : "Open Sidewindow"))) {
				opened = !opened;
			
			}
 
			// Elements on opened SideWindow
		if (opened) {
			
			stackHeight = 20f;
			if (GUI.Button (new Rect(10f, stackHeight += 25f, 190f, 20f),
				"New Canvas")) {
				NodeEditorWindow.self.CreateEasyAISystem();
				
				}
			if (GUI.Button (new Rect(10f, stackHeight += 35f, 190f, 20f),
				"Clear Canvas")) {
				coreSystem.nodes.Clear();
				coreSystem.fsmVariables.Clear();
				Node node = ScriptableObject.CreateInstance <StartAINode>();
				node.self = coreSystem.gameObject;
				node.Init();
				node.rect.x = 100f;
				node.rect.y = 100f;	
			 
				coreSystem.nodes.Add(node);
				
				}
			if (GUI.Button (new Rect(10f, stackHeight += 35f, 190f, 20f),
				"Welcome Window")) {
					WelcomeWindow.ShowWindow();
					
				}
			
 			//GUI.Label(new Rect(5f, stackHeight += 40f, 120f, 20f),
			//	"Variables: ");
			GUI.BeginGroup(new Rect(5f, stackHeight += 40, 210f, 1500f)); // 190
			
			
			GUILayout.BeginVertical("box");
			
			GUILayout.Label("Variables: ",GUILayout.Height(20), GUILayout.Width(80));
			
			
			
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Add",GUILayout.Height(15), GUILayout.Width(35)) && _choiceIndex != 0 && !string.IsNullOrEmpty(varName) ){
				foreach (var item in coreSystem.fsmVariables)
				{
					
					if (item.Name == varName){
						
						EditorGUIUtility.ExitGUI();
					}
				}
				
			
 				switch (_choiceIndex)
				{
					// String
				case 1:
					FsmString item ="";
					item.Name = varName;
					coreSystem.fsmVariables.Add(item);
					break;
					// Bool
				case 2:
					FsmBool item2 = false;
					item2.Name = varName;
					coreSystem.fsmVariables.Add(item2);
					break;
					// Color
				case 3:
					FsmColor item3 = Color.white;
					item3.Name = varName;
					coreSystem.fsmVariables.Add(item3);
					break;
					// Float
				case 4:
					FsmFloat item4 = 0f;
					item4.Name = varName;
					coreSystem.fsmVariables.Add(item4);
					break;
					// GameObject
				case 5:
					FsmGameObject item5 = ScriptableObject.CreateInstance<FsmGameObject>() ;
					item5.Name = varName;
					coreSystem.fsmVariables.Add(item5);
					break;
					// Object
				case 6:
					FsmObject item6 = ScriptableObject.CreateInstance<FsmObject>();
 
 					item6.Name = varName;
					coreSystem.fsmVariables.Add(item6);
					break;
					// Int
				case 7:
					FsmInt item7 = 0;
					item7.Name = varName;
					coreSystem.fsmVariables.Add(item7);
					break;
					// Vector 2
				case 8:
					FsmVector2 item8 = Vector2.zero;
					item8.Name = varName;
					coreSystem.fsmVariables.Add(item8);
					break;
					// Vector3
				case 9:
					FsmVector3 item9 = Vector3.zero;
					item9.Name = varName;
					coreSystem.fsmVariables.Add(item9);
					break;
				
				default:
					break;
				}
				varName ="";
				
				
 			}
			varName  = GUILayout.TextField(varName,GUILayout.Width(80));
			
			_choiceIndex  = EditorGUILayout.Popup(_choiceIndex,DisplayNames,GUILayout.Height(10),GUILayout.Width(60) );
			
		
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			GUILayout.Space(8f);
			
			
			// scrolll variables
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width (210), GUILayout.Height (400));
			
			
			
			
			for (int i = 0; i <  coreSystem.fsmVariables.Count; i++) {
				GUILayout.BeginVertical("box");
				GUILayout.BeginHorizontal();
				
 				GUILayout.Label(coreSystem.fsmVariables[i].Name,GUILayout.Width(60));
				GUILayout.Label(SerializedVariable.GetTypeName(coreSystem.fsmVariables[i].VariableType),GUILayout.Width(75));
				
				GUILayout.Space(15f);
				if (GUILayout.Button("X",GUILayout.Height(15), GUILayout.Width(20))){
					
					coreSystem.fsmVariables.RemoveAt(i);
					
					
					EditorGUIUtility.ExitGUI();
				 
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginVertical();
				
			 if (coreSystem.fsmVariables[i].VariableType == typeof(GameObject)){
			 	
						GameObject temp = coreSystem.fsmVariables[i].GetValue() as GameObject;

						 temp  = EditorGUILayout.ObjectField ("", temp , typeof(GameObject), true,GUILayout.Width(160)) as GameObject;
				 if (temp != null)
				 {
				 	var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables [i] = (FsmGameObject)temp; //temp as FsmGameObject;
				 	coreSystem.fsmVariables[i].Name = _name;
				 	
				 }
 				 
			 } else if (coreSystem.fsmVariables[i].VariableType == typeof(string)){
			 	
						string temp = coreSystem.fsmVariables[i].GetValue() as string;
			 	
				 temp  = EditorGUILayout.TextField ("", temp ,GUILayout.Width(160));
				 
				 if (temp != null)
				 {
				 	var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmString)temp;
				 	coreSystem.fsmVariables[i].Name = _name;
				 	
				 }
 				 
			 }
			 else if (coreSystem.fsmVariables[i].VariableType == typeof(int)){
			 	
				int temp = (int)coreSystem.fsmVariables[i].GetValue();
			 	
				 temp  = EditorGUILayout.IntField ("", temp ,GUILayout.Width(160));
				 

				 	var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmInt)temp;
				 	coreSystem.fsmVariables[i].Name = _name;
				 	
 				 
			 }else if (coreSystem.fsmVariables[i].VariableType == typeof(float)){
			 	
						float temp = (float)coreSystem.fsmVariables[i].GetValue();
			 	
				 temp  = EditorGUILayout.FloatField ("", temp ,GUILayout.Width(160));
				 
		
				 	var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmFloat)temp;
				 	coreSystem.fsmVariables[i].Name = _name;
				 	
				 
			 }else if (coreSystem.fsmVariables[i].VariableType == typeof(bool)){
			 	
						bool temp = (bool)coreSystem.fsmVariables[i].GetValue();
			 	EditorGUILayout.BeginHorizontal();
			 	GUILayout.Space(80f);
				 temp  = EditorGUILayout.Toggle ("", temp ,GUILayout.Width(80));
				 
				
				 	var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmBool)temp;
				 	coreSystem.fsmVariables[i].Name = _name;
				 	
				 EditorGUILayout.EndHorizontal();
				 
			 } else if (coreSystem.fsmVariables[i].VariableType == typeof(Vector2)){
			 	
				 
						Vector2 temp = (Vector2)coreSystem.fsmVariables[i].GetValue();
			 	
				 temp  = EditorGUILayout.Vector2Field ("", temp , GUILayout.Width(160));
				 
				
				 	var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmVector2)temp;
				 	coreSystem.fsmVariables[i].Name = _name;
				 	
				 
				 
			 
			} else if (coreSystem.fsmVariables[i].VariableType == typeof(Vector3)){
			 	
						Vector3 temp = (Vector3)coreSystem.fsmVariables[i].GetValue();
			 	
				temp  = EditorGUILayout.Vector3Field ("", temp , GUILayout.Width(160));
				 
			
				 	var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmVector3)temp;
				 	coreSystem.fsmVariables[i].Name = _name;
				 	
				 
				 
			}else  if (coreSystem.fsmVariables[i].VariableType == typeof(Color)){
				
						Color temp = (Color)coreSystem.fsmVariables[i].GetValue();
				
				temp  = EditorGUILayout.ColorField (temp ,GUILayout.Width(160) );
				
			
					var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmColor)temp;
					coreSystem.fsmVariables[i].Name = _name;
					
				
				
			}
			else  if (coreSystem.fsmVariables[i].VariableType == typeof(Object)){
				
						Object temp = (Object)coreSystem.fsmVariables[i].GetValue();
				
						temp  = EditorGUILayout.ObjectField ("", temp , typeof(Object), true,GUILayout.Width(160));
				
				if (temp != null)
				{
					var _name = coreSystem.fsmVariables[i].Name;
							coreSystem.fsmVariables[i] = (FsmObject)temp;
					coreSystem.fsmVariables[i].Name = _name;
					
				}
				
			}
			
			 
				
				//GUILayout.Label(coreSystem.fsmVariables[i].GetValue().ToString(),GUILayout.Width(220));
				
				GUILayout.EndVertical();
				GUILayout.Space(5f);
				GUILayout.EndVertical();
				
		 
				
				
			}
			
			EditorGUILayout.EndScrollView();
			
			GUILayout.EndVertical();
			
			GUI.EndGroup();
			
			//if (GUI.Button (new Rect(5f, stackHeight += 25f, 60f, 20f),"Add New")) {
					  
				
				
			//}
			
			
			
			 
 			
			//	// If TutorialSystem Gameobject was not yet created
			//if (!TutorialEditor.tutSystemCreated) {
			//		// New Tutorial Button
			//	if (GUI.Button (new Rect(5f, stackHeight += 25f, 190f, 20f),
			//		"New Tutorial", TutorialEditor.customSkin.button)) {
			//		TutorialEditor.CreateTutorialSystem ();
			//		}
				
			//	AboutButton();
			//	// If TutorialSystem was already created
			//} else {
			//		// Delete everything from TutorialDesigner Canvas
			//	if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),
			//		"Clear Workspace", TutorialEditor.customSkin.button)) {
			//		Undo.RegisterCompleteObjectUndo (TutorialEditor.savePoint, "Clear Workspace");
			//		foreach (Node n in TutorialEditor.savePoint.nodes) {
			//			n.Remove ();
			//		}
			//		TutorialEditor.savePoint.nodes.Clear ();
			//		TutorialEditor.savePoint.startNode = null;
			//		}
				
			//		// Hide all Dialogue Gameobjects in current scene
			//	if (GUI.Button(new Rect(5f, stackHeight += 25f, 190f, 20f),
			//		"Hide Dialogues", TutorialEditor.customSkin.button)) {
			//		TutorialEditor.savePoint.HideAllDialogues();
			//		}
				
			//	AboutButton();
				
			//		// Basic Zooming in the EditorWindow
			//	float zoomChange = 0f;
			//	GUI.Label(new Rect(5f, stackHeight += 30f, 120f, 20f),
			//		"Zoom : " + TutorialEditor.zoomFactor, labelBold);
			//	if (GUI.Button (new Rect(125f, stackHeight, 30f, 20f),
			//		"+", TutorialEditor.customSkin.button)) {
			//		zoomChange = 0.2f;
			//		}
			//	if (GUI.Button (new Rect(160f, stackHeight, 30f, 20f),
			//		"-", TutorialEditor.customSkin.button)) {
			//		zoomChange = -0.2f;
			//		}
				
			//		// If user changed zoomFactor
			//	if (zoomChange != 0) {
			//		TutorialEditor.zoomFactor = Mathf.Clamp (TutorialEditor.zoomFactor + zoomChange, 0.2f, 1f);
			//			// Store here for Serialization
			//		TutorialEditor.savePoint.zoomFactor = TutorialEditor.zoomFactor;
			//	}
				
			//		// Display StartNode if available
			//	Node startNode =  TutorialEditor.savePoint.startNode;
			//	string start = "missing";
			//	if (startNode != null) {
			//		bool isItStep = ((TutorialEditor.savePoint.startNode.nodeType & 1) == 1);
			//		start = (isItStep ? "Step - " : "Event - ") + startNode.description;
			//	}
				
				//GUI.Label(new Rect(5f, stackHeight += 30f, 120f, 20f),
				//	"Startnode: ");
				//GUI.Label(new Rect(5f, stackHeight += 18f, 190f, 52f),
				//	"start");
				
				//GUI.BeginGroup(new Rect(5f, stackHeight += 60, 190f, 230f));
				//int groupStack = 0;
				
				//	// Tutorial Monitor
				//GUI.Label (new Rect(10f, groupStack, 180f, 20f), "Tutorial Monitor");
				//GUI.Label (new Rect(10f, groupStack += 20, 180f, 18f), "--- Default Path ---");
				//GUI.Label (new Rect(10f, groupStack += 18, 180f, 0f), "-> " + currentNode);
				//GUI.Label (new Rect(10f, groupStack += 80, 180f, 18f), "--- Global ---");
				//GUI.Label (new Rect(10f, groupStack += 18, 180f, 0f), "-> " + globalNode);
				
			//GUI.EndGroup();
		}
		GUI.EndGroup ();        
		}
	public static Texture2D ColorToTex(Color col) {
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(1, 1, col);
		tex.Apply();
		tex.hideFlags = HideFlags.HideAndDontSave;
		return tex;
	}
	public static string[] DisplayNames{
		get{
			return new string[10]{
				"None",
				"String",
				"Bool",
				"Color",
				"Float",
				"GameObject",
				"Object",
				"Int",
				"Vector2",
				"Vector3"
			};
		}
	}
		
}
 }
