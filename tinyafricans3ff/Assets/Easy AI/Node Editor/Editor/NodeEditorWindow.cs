using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif 
using UnityEngine.Events;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
 
namespace AxlPlay {
public class NodeEditorWindow : EditorWindow {
	
	
	 /// Component of the Node Editor GameObject. It will contain Nodes and all settings that are important during the Game
	public static EasyAIFSM coreSystem;
	
     /// Self reference
	public static NodeEditorWindow self;
	
	//
    /// <summary>
    /// Defines a handle for Connections between mouse cursor and a Connector
    /// </summary>
	public struct ConHandle {
		public bool active;
		public Node node;
		public Connector connector;
		public Vector2 position;
		public Connector.knobType left;
	}	    
	
    /// <summary>
    /// The handle for current clicked Node Connector will be stored here for further actions
    /// </summary>
	public static ConHandle clickedConnector;   
	
	 
	public SideWindow sideWindow; // Window at the right. With Menu and help
	
	
	public Vector2 scrollPos;
    // A custom context menu with custom skin
	public static CustomMenu customMenu;
	
	public static float zoomFactor;
	public bool scrollWindow;
	
	public static Dictionary<string,Type> nodes;
 
	public static Texture2D[] ConTex;
	
	// Temp value for storing the matrix before altering
	private static Matrix4x4 prevMatrix;
	private bool initialized;
	private Texture2D Background;
	private Vector2 scrollOffset; // Amount of current Scrolling.
	
	private Vector2 mousePos;
	 // Another GUI Bug prevention, occurs on Scene change with EditorWindow opened
	private UnityEngine.SceneManagement.Scene sc;
	// Bool for GUI Bug prevention, makes sure that Layout Event comes before Repaint Event.       
	static bool firstLayoutCallDone = false;  
	
	
	private GameObject SelectedObject;
	 // Connector that was clicked before Mouse Movements
	private Connector reallyClickedConnector; 
	
	private bool flagIsRunning;
	private float timeCount;
	
    #region Main Methods
	[MenuItem("Window/Easy AI Node Editor")]
	static void ShowEditor()
	{			
		self = GetWindow<NodeEditorWindow>(); 
		
		self.minSize = new Vector2(800, 600);
		 
		
		Texture iconTexture =  (Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>(EditorGUIUtility.isProSkin? "Textures/Icon_Dark.png" : "Textures/Icon_Light.png");
		self.titleContent = new GUIContent ("Node Editor", iconTexture);
	        if(PreferencesEditor.GetBool (Preference.ShowWelcomeWindow,true)) {
                WelcomeWindow.ShowWindow();
            }
	 
	}
	void OnEnable(){
		SelectedObject = Selection.activeGameObject;
		coreSystem = null;
		
		CreateEasyAISystem();
		EditorApplication.update += Update;
 
	}
	public void Init() {		
		self = this;
		
		 
		nodes = new Dictionary<string, Type>();
		// reflection nodes by attribute
		getNodes();

		
		Background = (Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Easy AI/Node Editor/Resources/background.png");
		zoomFactor = 1f;
		DrawBackground();
		
		 // Create Replacement for Context menu that will open with right mouse button
		customMenu = new CustomMenu();
	 
		
		if (coreSystem == null)
			CreateEasyAISystem();
		
		
		
			sideWindow = new SideWindow ();
		
			
			initialized = true; 
		
	}
	void  Update(){
		
		if (EditorApplication.isPlaying) {
				
			timeCount = timeCount + Time.deltaTime;
			
			if (timeCount > 2f){
				timeCount = 0f;
			}
			if (timeCount > 1f){
				flagIsRunning = true;
			}else {
				flagIsRunning = false;
			} 
					

		}else{
			
			flagIsRunning = false;
		}
		
		
	}
	 // OnGUI is called for rendering and handling GUI events
	void OnGUI() {  
		
		
		
		if (customMenu == null){
			
			Init();
		}
		
		if (!initialized){
			
			Init();	
		}
		// Input Events
		Event currentEvent = Event.current;
		// Zoom Operations
		if (coreSystem == null || zoomFactor == 0) {
			// If coreSystem.zoomfactor is not available
			zoomFactor = 1f;
		} else {
			// Get zoomfactor from serialized variable
			zoomFactor = coreSystem.zoomFactor;
		}
		
		
		SelectedObject = Selection.activeGameObject;
			
			if (SelectedObject != null){
				
				if (SelectedObject.GetComponent<EasyAIFSM>()){
					
					coreSystem = SelectedObject.GetComponent<EasyAIFSM>();
				}
			}
		// Side Window       
		if (sideWindow != null && coreSystem != null) {									
			sideWindow.Draw (position , coreSystem);
		}
		
		// User Input
		if (!customMenu.visible) InputEvents (currentEvent);
		
		 // Begin scaled GUI Area
		Begin (zoomFactor, new Rect(0f, 11.5f, position.width - (sideWindow.opened ? 220f : 25f), position.height));
		
		
		// Background
		if (currentEvent.type == EventType.Repaint) {
			DrawBackground ();
		}
		
		
		
		// Nodes
		if (coreSystem != null) {
			if (coreSystem.nodes != null){
				if (coreSystem.nodes.Count > 0 && sc == UnityEngine.SceneManagement.SceneManager.GetActiveScene()){
					
						// Draw Curves between Connection Knobs
				 
					DrawCurves ();
 
					BeginWindows();
					for (int i = 0; i < coreSystem.nodes.Count; i++) {
						//// Draw Connection Knobs only
						coreSystem.nodes[i].DrawConnectors ();
						if (coreSystem.nodes[i].isRunning && flagIsRunning){
							GUI.color =  Color.green;
						}else{
							
							GUI.color =  Color.white;
						}
						coreSystem.nodes[i].rect = GUI.Window(i,coreSystem.nodes[i].rect,DrawNodeWindow,"",EditorStyles.helpBox);// customSkin.GetStyle("Eventwindow")
 					}
					
					EndWindows();
					
					
					
				}
				
			}
		}else{
			
			
			//// If savePoint doesn't exist, there is no Tutorial System Object in the Scene
			//GameObject temp = GameObject.Find("DialogueSystem");
			//if (temp != null) {
			//	coreSystem = temp.GetComponent<CoreSystem>();
			
			//}
		}
		
		if (wantsMouseMove) EditorGUI.EndDisabledGroup();
		// End Scaled Area
		ZoomAreaEnd();
		
		
		// Context Menu
		customMenu.Draw(currentEvent);
		
		
	 if (currentEvent.type == EventType.Repaint) {
		 sc = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		 
		 Repaint ();
	 }

    }
    #endregion
	
    #region  Utility Methodos
	 /// <summary>
        /// Determines if n is an EventNode wich has "forever" toggeled
        /// </summary>
        /// <param name="n">Any Nodetype</param>
	public static bool IsNodeForeverEvent(Node n) {
			// Check if current Node is an Forever-Event
		bool nodeIsForeverEvent = false;
		//if ((n.nodeType & 2) == 2)
			//if (((EventNode)n).forEver)
			//	nodeIsForeverEvent = true;
			
		return nodeIsForeverEvent;
	}

    /// Draws the curves between Node-Connectors    
	public void DrawCurves() {
		if (clickedConnector.active) {            
			Vector3 startPos = new Vector3(clickedConnector.position.x, clickedConnector.position.y);
			Vector3 endPos = new Vector3(mousePos.x, mousePos.y);
			float distance = Mathf.Min(Vector3.Distance(startPos, endPos), 50f);
			
			Vector3 startTan;
			if (clickedConnector.left == Connector.knobType.input){
				
				startTan = startPos + ( Vector3.left) * distance;
			}else{
				startTan = startPos + ( Vector3.right ) * distance;
			}
			
			Vector3 endTan;
			if (clickedConnector.left == Connector.knobType.input){
				
				endTan = endPos + (Vector3.right) * distance;
			}else{
				endTan = endPos + (Vector3.left) * distance;
			}
		 
			Color col = Color.yellow;  //new Color(1f, 1f, 0, 1f);
			
	        // Curve of currently clicked Connector Handle to Mouse Position
			Handles.DrawBezier(startPos, endPos, startTan, endTan, col, null, 4f);            
			//Repaint();
		}
		
		// Draw all Connections between Nodes here
		if (coreSystem != null) {
			foreach (Node n in coreSystem.nodes) {
				foreach (Connector c in n.connectors) {
					if (c != null) {
							// Allways draw from OutNode to InNode	
						
						// c.input  == Connector.knobType.input && 
						if ( c.connections.Count == 1 && c.color == Color.red ) { // Output Node (can only have 1 connection)
							if (c.visible) {
								Vector3 startPos = c.GetRect().center;
								Vector3 endPos = c.connections[0].GetRect().center;
								float distance = Mathf.Min(Vector3.Distance(startPos, endPos), 50f);
								Vector3 startTan = startPos + c.getCurveTangent() * distance;
								Vector3 endTan = endPos + c.connections[0].getCurveTangent() * distance;
									// Current Node will be connected with a yellow thick line. To the last active Node.
								Color col =   c.color; //new Color(0.608f, 0.832f, 0.804f, 1f); 
								float thickness = 3f;
							
					            // Curve of currently clicked Connector Handle
								Handles.DrawBezier(startPos, endPos, startTan, endTan, col, null, thickness);
								//Repaint();
							}
								// Only for conditional Nodes
						} else if (c.connections.Count >= 1 && c.color == Color.cyan){
							if (c.visible) {
								Vector3 startPos = c.GetRect().center;
								Vector3 endPos = c.connections[0].GetRect().center;
								float distance = Mathf.Min(Vector3.Distance(startPos, endPos), 50f);
								Vector3 startTan = startPos + c.getCurveTangent() * distance;
								Vector3 endTan = endPos + c.connections[0].getCurveTangent() * distance;
									// Current Node will be connected with a yellow thick line. To the last active Node.
								Color col =   c.color; //new Color(0.608f, 0.832f, 0.804f, 1f); 
								float thickness = 3f;
								
					            // Curve of currently clicked Connector Handle
								Handles.DrawBezier(startPos, endPos, startTan, endTan, col, null, thickness);
								//Repaint();
							}
								
							}else {
                                    // If not visible but still has 1 connection, it's because Dialogue
                                    // Buttons have changed, making one connected Con invisible
								  c.Break();

									
							}
						}
					}
				}
		}
	}

	void DrawNodeWindow(int id) {
		if (UnityEditor.EditorApplication.isPlaying || customMenu.visible){
			GUI.enabled = false;
		
		}
		
		 
		// Prevent a GUI Bug here. If Repaint Event comes before the first Layout Event, this is gonna fail
		if (!firstLayoutCallDone) {
			
			if (Event.current.type == EventType.Layout) {
				firstLayoutCallDone = true;
				coreSystem.nodes[id].Id = id;

					#if UNITY_EDITOR
					coreSystem.nodes[id].DrawNode();
					#endif
			

			}
		} else {
				
				try{
					coreSystem.nodes[id].Id = id;

					#if UNITY_EDITOR
					coreSystem.nodes[id].DrawNode();
					#endif
				}
				catch (System.Exception)
				{
					
				
				}
		}
		
		
		// Make Node dragable
		if (!customMenu.visible) GUI.DragWindow();
	}
	    // Handles mouse operations
	void InputEvents(Event e) {
	  // Events        
		mousePos = e.mousePosition;  
	   // Mouse Events
		Node clickedNode = null;       
	
		if (e.type == EventType.MouseDown) {
			if (coreSystem != null && !EditorApplication.isPlaying) {
				clickedNode = NodeAtPosition (mousePos);
				if (clickedNode == null) {
					clickedConnector = ConAtPosition (mousePos);
				}
			}
			
			
			if (clickedNode != null && !EditorApplication.isPlaying) {
					// Click on a Node
				
				if (e.button == 0) {
						// Left Click -> Window Drag. Handled by Unity   
			
					 
				} else if (e.button == 1) {
					
					if (clickedNode is StartAINode){
				
					}
					else 
					{
								// Right click -> Node delete
						customMenu.Call(ContextCallback, "Delete Node", "Duplicate Node");
						e.Use ();
						
					}
						
				}
			}else if (clickedConnector.active && !EditorApplication.isPlaying) {
					// Click on Connector
				
					// Store really clicked connector. Because clickedConnector might change below
				reallyClickedConnector = clickedConnector.connector;
				
					// Break old Connection(s) if there are some
				if (clickedConnector.connector.connections.Count > 0) {
					Undo.RegisterCompleteObjectUndo (clickedConnector.connector, "Disconnect Nodes");
					Connector otherConnector = clickedConnector.connector.Break ();
					
					if (otherConnector != null) {
							// The Curve Handle should now be detatched from Connector,
							// And still be connected with the Connector on the other Node
						clickedConnector = ConAtPosition (otherConnector.GetRect ().center);
					}
				}
			}  else {
	 
					// Click on empty Canvas (no Node, not sideWindow)
				if (e.button == 2 || e.button == 0) {
					
					Rect rect = new Rect ( sideWindow.opened ? position.width - 220 : position.width -20, 0f, 220, position.height); 

					if (!rect.Contains(mousePos)){
						// Left/Middle Click -> Start scrolling
						scrollWindow = true;
						e.delta = new Vector2 (0, 0);
					}
					
				} else if (e.button == 1) {	
					if(SelectedObject != null && coreSystem == null) { 
						CreateEasyAISystem();
						EditorGUIUtility.ExitGUI();
					}	
					// new menu system
					// Now create the menu, add items and show it NEED TO CHANGE TO CUSTOM GENERIC MENU
					GenericMenu menu = new GenericMenu();
					
					
					//  no gameobject selected in scene
					if (coreSystem == null){
						menu.AddItem(new GUIContent("Create GameObject"), false, ContextCallback, "Create GameObject");
						
					}else {
						coreSystem.zoomFactor = 1f;
						
						foreach (var item in nodes.Keys)
						{
							menu.AddItem(new GUIContent(item.ToString()), false, ContextCallback, item.ToString());
						}
					}
					 
					menu.ShowAsContext();
					e.Use();
					
					 
						// Right click -> Editor Context Click
					//customMenu.Call(ContextCallback, nodes.Keys.ToArray());
					//e.Use ();
				}                
			}
		} else if (e.type == EventType.MouseUp) {
				// Left/Middle click up
			if (e.button == 2 || e.button == 0) {
					// Connect 2 Nodes, if possible
				if (clickedConnector.active) {
					
					
					
					ConHandle secondHandle = ConAtPosition (mousePos);
					if (secondHandle.active) {
						if (clickedConnector.node != secondHandle.node ) {
								// If Mouse Click and Release on the same Connector, connect them but don't record UNDO
							
							if (secondHandle.connector != reallyClickedConnector) {
								Undo.RegisterCompleteObjectUndo (clickedConnector.connector, "Connect Nodes");
								Undo.RegisterCompleteObjectUndo (secondHandle.connector, "Connect Nodes");
							} 
							
							if (clickedConnector.connector.color == secondHandle.connector.color ){
								// Connect both Connectors to each other.
								clickedConnector.connector.ConnectTo (secondHandle.connector);
							}
							
							// // RECALCULE NODES
							//clickedConnector.node.CalculateNode();
							//secondHandle.node.CalculateNode();
						}
					}
					// // RECALCULE NODES
					//if (clickedConnector.node != null) clickedConnector.node.CalculateNode();
					//if (secondHandle.node != null) secondHandle.node.CalculateNode();
					// recalcule all nodes ONLY FOR TEST
					foreach (var item in coreSystem.nodes)
					{
						item.CalculateNode();
					}
					
				}
				
				
				// Stop scrolling
				clickedConnector.active = false;
				scrollWindow = false;

			}
		}
		else if (e.type == EventType.ScrollWheel){
			
			if (e.delta.y > 1  && coreSystem.zoomFactor < 1f){
				coreSystem.zoomFactor = coreSystem.zoomFactor + 0.1f;
				
			}else if (e.delta.y < 0 && coreSystem.zoomFactor > 0.4f ){
				coreSystem.zoomFactor = coreSystem.zoomFactor - 0.1f;
			}
		} 
		
		
		// Scroll Mainwindow
		if (scrollWindow) {
	        // Change Window and Nodes by Mouse delta (difference to last rendered position)
			scrollOffset += e.delta / 2;
			if (coreSystem != null) {
				foreach (Node n in coreSystem.nodes) {
					
					n.rect.position += e.delta / 2;
					
					 
				}
			}
			//Repaint();            
		}
	}
	public void getNodes(){
		
		//var   scriptAssemblies = AppDomain.CurrentDomain.GetAssemblies()
		//	.SelectMany(t => t.GetTypes())
		//	.Where(t => t.IsClass && t.Namespace == "AxlPlay");

			
		IEnumerable<Assembly> scriptAssemblies = AppDomain.CurrentDomain.GetAssemblies ().Where ((Assembly assembly) => assembly.FullName.Contains ("Assembly"));
		foreach (Assembly assembly in scriptAssemblies) 
		{
			foreach (Type type in assembly.GetTypes().Where(T => T.Namespace =="AxlPlay" &&  T.IsClass && !T.IsAbstract && T.IsSubclassOf(typeof(Node))))
			{
				object[] nodeAttributes = type.GetCustomAttributes(typeof(NodeAttribute), false);                    
				NodeAttribute attr = nodeAttributes[0] as NodeAttribute;
				if(attr != null && attr.show)
				{
					nodes.Add(attr.contextText,type);
				}
			}
	}
	 
	}
	public Node NodeAtPosition(Vector2 pos) {
	        // Check if we clicked inside a window
		if (coreSystem != null) {
			for (int i = coreSystem.nodes.Count - 1; i >= 0; i--) { // From top to bottom because of the render order (though overwritten by active Window)
				if (coreSystem.nodes[i].rect.Contains(pos)) {
	                    // Click on Node
					coreSystem.nodes[i].Activate();
					return coreSystem.nodes[i];                    
				} 
			}
		}
		
		return null;
	} 
	  /// <summary>
        /// Gets Handle struct of clicked Connector of any Node
        /// </summary>
        /// <returns>ConHandle at current mouseposition. If there's no Connector at current position, the returned ConHandle is not active</returns>
        /// <param name="pos">Position of mouse cursor</param>
	public ConHandle ConAtPosition(Vector2 pos) {
		ConHandle handle = new ConHandle {
			active = false
	        };        
		
		foreach (Node n in coreSystem.nodes) {
			foreach (Connector c in n.connectors) {
				if (c.visible && c.GetRect().Contains(pos)) {
					handle.active = true;
					handle.node = n;
					handle.connector = c;
					handle.position = c.GetRect().center;
					handle.left = c.input;
					break;
				}
			}
		}
		
		return handle;
	}
	 // Callback from Context Menu
	private void ContextCallback(object obj) {
 
		
		if (obj.ToString() == "Delete Node"){
			
			Node d_node = NodeAtPosition(mousePos);
			if (d_node != null) {
				Undo.RegisterCompleteObjectUndo(coreSystem, "Delete Node");						
				// Break opposite connections of this Nodes Connectors
				foreach (Connector c in d_node.connectors) {
					c.BreakOpposite();
				}
				
				coreSystem.nodes.Remove(d_node);   
				
				Undo.DestroyObjectImmediate(d_node);		
			}
		}else if (obj.ToString() == "Duplicate Node"){
			Node dup_node = NodeAtPosition(mousePos);
			if (dup_node != null) {
 
				Node node = ScriptableObject.CreateInstance (dup_node.GetType().Name) as Node;
				node.Init();
				node.rect.x = mousePos.x + 60f;
				node.rect.y = mousePos.y;	
				coreSystem.nodes.Add(node);
				 
				//EditorUtility.CopySerialized(dup_node, node);
			
				
			}
			
			
		}else if (obj.ToString() == "Create GameObject"){
			
			SelectedObject = new GameObject ("Easy AI Agent");
			coreSystem = SelectedObject.AddComponent<EasyAIFSM> ();
		
			coreSystem.Init ();
			
			Node node = ScriptableObject.CreateInstance <StartAINode>();
			node.self = coreSystem.gameObject;
			node.Init();
			node.rect.x = mousePos.x;
			node.rect.y = mousePos.y;	
			coreSystem.nodes.Add(node);
			
		}
		
		else{
			
			
			Type typeData;
	
			if (nodes.TryGetValue(obj.ToString(), out typeData)) // Returns true.
			{
	 
				
				Node node = ScriptableObject.CreateInstance (typeData.Name) as Node;
				node.self = coreSystem.gameObject;
				node.Init();
				node.rect.x = mousePos.x;
				node.rect.y = mousePos.y;	
				coreSystem.nodes.Add(node);
			}
		}
 
	}
	// Update Node Settings after Undo, deletion etc...
	void UpdateNodes() {	
		Debug.Log("UpdateNodes");
			// Update glogal Node Count
		if (coreSystem != null) {
			if (coreSystem.nodes.Count > 0) {
					// Check connections between Nodes
				foreach (Node n in coreSystem.nodes) {
					foreach (Connector c in n.connectors) {
							// Mark a connector for removal here. Delete if afterwards if necessary, so the foreach loop doens't break
						Connector conToRemove = ScriptableObject.CreateInstance<Connector>();
						
							// Restore Homenode of Connector
						if (c.homeNode == null)
							c.homeNode = n;
						
							// Restore Connections between Connectors
						if (c.connections.Count > 0 && c.visible) {
					
							foreach (Connector oppositeConnector in c.connections) {
									// If opposite Connector does not have this in his connection list
								if (!oppositeConnector.connections.Contains (c)) {
									
										// Input can have > 1 connections. Output only one!
									if (oppositeConnector.input == Connector.knobType.input || (oppositeConnector.input != Connector.knobType.input && oppositeConnector.connections.Count == 0)) {
										
									
										
										oppositeConnector.connections.Add (c);
										
										
									} else {
											// If opposite is an Output with > 1 Connections, don't connect this connector to it. Because
											// it already has one connection. Mark it for safe removal after the foreach-loop
										conToRemove = oppositeConnector;
									}
								} else {										
										// If opposite Node was deleted, delete also the connection to it. Otherwise the curve will be drawn into emptiness
									if (oppositeConnector.homeNode == null) {
										conToRemove = oppositeConnector;
									}
								}
							}
						}
						
                            // Safely remove Connector from List
						c.connections.Remove(conToRemove);
					}
				}
			}
		}
	}
	public void CreateEasyAISystem() {
		
		
 
		if (SelectedObject != null) {
			if (!SelectedObject.GetComponent<EasyAIFSM>()){
				coreSystem = SelectedObject.AddComponent<EasyAIFSM> ();
				coreSystem.Init ();
				
				Node node = ScriptableObject.CreateInstance <StartAINode>();
				node.self = coreSystem.gameObject;
				node.Init();
				node.rect.x = 100f;
				node.rect.y = 100f;	
				coreSystem.nodes.Add(node);
				
				
				
			}else{
				
				coreSystem = SelectedObject.GetComponent<EasyAIFSM>();
			}
		}else {
		//	SelectedObject = new GameObject ("Easy AI Agent");
		//	coreSystem = SelectedObject.AddComponent<CoreSystem> ();
		//	coreSystem.Init ();
		//
		}
	}
	
	
	 // Background drawing, incl tiling and scrolling
	private void DrawBackground() {
		 // Draw Background at current Scrollposition            
		if (Background != null) {			
			Vector2 offset = new Vector2(scrollOffset.x % Background.width - Background.width,
				scrollOffset.y % Background.height - Background.height);
			int tileX = Mathf.CeilToInt((position.width * (1 / zoomFactor) + (Background.width - offset.x)) / Background.width);
			int tileY = Mathf.CeilToInt((position.height * (1 / zoomFactor) + (Background.height - offset.y)) / Background.height);
			
			for (int x = 0; x < tileX; x++) {
				for (int y = 0; y < tileY; y++) {
					Rect texRect = new Rect(offset.x + x * Background.width,
						offset.y + y * Background.height,
						Background.width, Background.height);
					GUI.DrawTexture(texRect, Background);
				}
			}
		}else{
			Background = (Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Easy AI/Node Editor/Resources/background.png");
		} 
			
		 
	}
	public static Rect Begin(float zoomScale, Rect screenCoordsArea) 
	{
		GUI.EndGroup(); //End the group that Unity began so we're not bound by the EditorWindow
		
				// Define the visible area of the zoomed area
		Rect clippedArea = new Rect (screenCoordsArea.position, screenCoordsArea.size * (1.0f / zoomScale));				
		
				// Adjust y of clippedArea Rect. Float values are necessary to balance Unity's dynamic window position
		double zoom = System.Math.Round(zoomFactor, 1);
		if (zoom == 1f) {
			clippedArea.y = 21f;
		} else if (zoom == 0.8) {
			clippedArea.y = 17.5f;
		} else if (zoom == 0.6) {
			clippedArea.y = 15f;
		} else if (zoom == 0.4) {
			clippedArea.y = 13f;
		} else if (zoom == 0.2) {
			clippedArea.y = 11.5f;
		}
		
				// Begin the calculated draw group
		GUI.BeginGroup(clippedArea);
		
		// Store the current matrix before altering
		prevMatrix = GUI.matrix;
		 
		// Zoom the whole Editor screen
		EditorGUIUtility.ScaleAroundPivot (new Vector2(zoomScale, zoomScale), clippedArea.min);
		
		// Return the calculated area
		return clippedArea;
	}
	static void ZoomAreaEnd(){
		GUI.matrix = prevMatrix;
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(0, 21, Screen.width, Screen.height));
		
	}
	 
    #endregion

	 
}
 
}