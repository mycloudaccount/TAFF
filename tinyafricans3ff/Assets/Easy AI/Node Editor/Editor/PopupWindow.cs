using UnityEngine;
using System.Collections;
using UnityEditor;
namespace AxlPlay {
public class PopupWindow : EditorWindow {
	
	static PopupWindow curPopup;
	
 
	
	static void ShowEditor()
	{			
		PopupWindow window  = GetWindow<PopupWindow>(); 
		 
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
		window.ShowPopup();
		
	 
	}
	void OnGUI()
	{
		EditorGUILayout.LabelField("This is an example of EditorWindow.ShowPopup",EditorStyles.wordWrappedLabel);
		GUILayout.Space(70);
		if (GUILayout.Button("Agree!")) this.Close();
	}
	 
}
}