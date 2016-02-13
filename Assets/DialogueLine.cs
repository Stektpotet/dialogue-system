using UnityEngine;
using System.Collections;

/*
* Things yet to be added: text speed
*/
public class DialogueLine {
	public Sprite Portrait { get; set; }
	public string Text { get; set; }
	public string Name { get; set; }
	public float TextSpeed { get; set; }
	public Font Font { get; set; }
	public AudioClip Sound { get; set; }

	public DialogueLine(Sprite portrait = null, string text = null, float textSpeed = default(float), string name = "???", Font font = null, AudioClip sound = null)
	{
		Portrait = portrait;
		Text = text;
		TextSpeed = (textSpeed == default(float) ? XmlDialogueReader.DEFAULT_SPEED : textSpeed);
		Name = name ?? XmlDialogueReader.DEFAULT_NAME;
		Font = font;
		Sound = sound;
	}
}
