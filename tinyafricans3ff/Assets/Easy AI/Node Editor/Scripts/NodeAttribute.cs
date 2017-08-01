using UnityEngine;
using System.Collections;
using System;

namespace AxlPlay {
public class NodeAttribute : Attribute {
 
	public string contextText { get; private set; }
	
	public bool show {get; private set;}
 
	public NodeAttribute (string ReplacedContextText, bool _show)
	{
		contextText = ReplacedContextText;
		show = _show;
	}
	
}
 
}
 