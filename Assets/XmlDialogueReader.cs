using UnityEngine;
using System.Collections;
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
			lines.Add(new DialogueLine
				(
					Resources.Load<Sprite>("Sprites/" + (string) eLine.Element("portrait")),
					(string)text,
					(text.Attribute("speed") == null ? DialogueLine.DEFAULT_SPEED : (float)text.Attribute("speed")),
					(text.Attribute("name") == null ? DialogueLine.DEFAULT_NAME : (string)text.Attribute("name"))
				)
			);
			Debug.Log(text.Attribute("speed"));
		}

		return lines.ToArray();
	}
}
