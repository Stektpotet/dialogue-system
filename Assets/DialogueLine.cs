using UnityEngine;
using System.Collections;

/*
* Things yet to be added: text speed
*/
public class DialogueLine {
	public static string DEFAULT_NAME = "???";
	public static float DEFAULT_SPEED = 0.001f;

	public Sprite Portrait { get; set; }
	public string Text { get; set; }
	public string Name { get; set; }
	public float TextSpeed { get; set; }
	public DialogueChoice Choice { get; set; }

	public DialogueLine(Sprite portrait = null, string text = null, float textSpeed = 0.001f, string name = "???", DialogueChoice choice = null)
	{
		Portrait = portrait;
		Text = text;
		TextSpeed = (textSpeed == default(float) ? DEFAULT_SPEED : textSpeed);
		Name = name ?? DEFAULT_NAME;
		Choice = choice;
	}
}
