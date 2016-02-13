using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Linq;

public static class XmlDialogueReader {
	public const string DEFAULT_PORTRAIT = "question-mark";
	public const string DEFAULT_FONT = "Linux-Libertine";
	public const string DEFAULT_SOUND = "fast-car-honk";
	public static string DEFAULT_NAME = "???";
	public static float DEFAULT_SPEED = 0.02f;

	public static DialogueLine[] LoadDialogue(string scene)
	{
		var lines = new List<DialogueLine>();
		XDocument xml = XDocument.Load("Assets/Dialogue/" + scene + ".xml");

		foreach (var eLine in xml.Element("dialogue").Element("lines").Elements())
		{
			XElement text = eLine.Element("text");
			Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(
				"Assets/Sprites/"+ ((string) eLine.Element("portrait") ?? DEFAULT_PORTRAIT) + ".png" // TODO question mark default
			);
			Font font = AssetDatabase.LoadAssetAtPath<Font>(
				"Assets/Fonts/" + ((string) text.Attribute("font") ?? DEFAULT_FONT) + ".ttf"
			);
			AudioClip sound = AssetDatabase.LoadAssetAtPath<AudioClip>(
				"Assets/Sounds/" + ((string)text.Attribute("sound") ?? DEFAULT_SOUND) + ".wav"
			);
			lines.Add(new DialogueLine
				(
					sprite,
					(string)text,
					(text.Attribute("speed") == null ? DEFAULT_SPEED : (float)text.Attribute("speed")), // float is non-nullable so... ugly code it is
					(string)text.Attribute("name") ?? DEFAULT_NAME,
					font,
					sound
				)
			);
		}

		return lines.ToArray();
	}
}
