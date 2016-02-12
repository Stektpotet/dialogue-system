using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Linq;

public static class XmlDialogueReader {
	public static DialogueLine[] LoadDialogue(string scene)
	{
		var lines = new List<DialogueLine>();
		XDocument xml = XDocument.Load("Assets/Dialogue/" + scene + ".xml");

		foreach (var eLine in xml.Element("dialogue").Element("lines").Elements())
		{
			XElement text = eLine.Element("text");
			Font font = (Font)AssetDatabase.LoadAssetAtPath<Font>(
				"Assets/Fonts/" + ((string) text.Attribute("font") ?? "Linux-Libertine") + ".ttf"
			);
			AudioClip sound = (AudioClip)AssetDatabase.LoadAssetAtPath<AudioClip>(
				"Assets/Sounds/" + ((string)text.Attribute("sound") ?? "silent") + ".wav"
			);
			lines.Add(new DialogueLine
				(
					Resources.Load<Sprite>("Sprites/" + (string) eLine.Element("portrait")),
					(string)text,
					(text.Attribute("speed") == null ? DialogueLine.DEFAULT_SPEED : (float)text.Attribute("speed")),
					(text.Attribute("name") == null ? DialogueLine.DEFAULT_NAME : (string)text.Attribute("name")),
					font,
					sound
				)
			);
			
			Debug.Log(text.Attribute("speed"));
		}

		return lines.ToArray();
	}
}
