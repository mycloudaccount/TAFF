using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace AxlPlay {
public class CustomMenu {	
	private Rect rect;
	private float buttonHeight = 20f;
	private List<Rect> buttonRects;
	private List<string> buttonCaptions;
	GUIStyle menu, butt, buttHover;
	public bool visible = false;
	public delegate void MenuCallback(object obj);
	private MenuCallback callbackReference;
	
	public CustomMenu() {	
		menu = new GUIStyle ();
		menu.normal.background = ColorToTex (new Color (0.2f, 0.2f, 0.2f, 0.7f));
		
		buttonRects = new List<Rect> ();
		buttonCaptions = new List<string> ();
		
		butt = new GUIStyle ();
		butt.normal.textColor = Color.white;
		butt.padding = new RectOffset (5, 0, 3, 3);
		
		buttHover = new GUIStyle (butt);
		buttHover.normal.background = ColorToTex (new Color(0.5f, 0.5f, 0.5f, 0.5f));
	}
	
	public void Call(MenuCallback menuCallback, params string[] items) {
		buttonRects.Clear ();
		buttonCaptions.Clear ();
		if (NodeEditorWindow.coreSystem == null) return;
		
		rect = new Rect (Event.current.mousePosition * 1f, new Vector2 (150f, buttonHeight * items.Length));
			// Configure Buttons
		for (int i=0; i<items.Length; i++) {
			buttonRects.Add(new Rect (rect.x, rect.y + (buttonHeight * i), rect.width, buttonHeight));
			buttonCaptions.Add (items [i]);
		}
		callbackReference = menuCallback;
		
			// EditorWindow needs to trigger OnGUI also on mouseMove while ContextMenu is open - for hover effect
		NodeEditorWindow.self.wantsMouseMove = true;
		visible = true;
	}
	
	public void Draw(Event e) {		
		if (!visible) return;
		
		GUI.Box (rect, "", menu);
		for (int i=0; i<buttonRects.Count; i++) {			
			if (GUI.Button (buttonRects [i], buttonCaptions [i], buttonRects [i].Contains (e.mousePosition) ? buttHover : butt)) {
				if (e.button == 0) {
					
					callbackReference (buttonCaptions [i]);
					Close ();
				}
			}
		}
		NodeEditorWindow.self.Repaint ();
		
		if (e.type == EventType.MouseDown) {							
			Close ();
		}
	}
	
	private void Close () {
		NodeEditorWindow.self.wantsMouseMove = false;
		visible = false;
	}
	public static Texture2D ColorToTex(Color col) {
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(1, 1, col);
		tex.Apply();
		tex.hideFlags = HideFlags.HideAndDontSave;
		return tex;
	}
}
}
