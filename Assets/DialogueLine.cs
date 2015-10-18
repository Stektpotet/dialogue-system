using UnityEngine;
using System.Collections;

/*
* Things yet to be added: text speed
*/
public class DialogueLine {
	public Sprite Portrait { get; set; }
	public string Text { get; set; }
	public float TextSpeed { get; set; }
	
	public DialogueLine() {
		TextSpeed = 0.0001f;
	}
}
