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
					Resources.Load<Sprite>("Sprites/" + (string)eLine.Element("portrait")),
					(string)text,
					(text.Attribute("speed") == null ? DialogueLine.DEFAULT_SPEED : (float)text.Attribute("speed")),
					(text.Attribute("name") == null ? DialogueLine.DEFAULT_NAME : (string)text.Attribute("name"))
				)
			);
		}

		var choices = xml.Element("dialogue").Element("choice");
		if (choices != null)
		{
			var choice = new DialogueChoice(); // temp value

			int i = 0;
			foreach (var eLine in choices.Elements()) // not cooperating to let me make a for loop here... wweh
			{
				choice.ChoiceText[i] = (string)eLine;

				string target = (string)eLine.Attribute("target");
				choice.Targets.Add((int)eLine.Attribute("id"),target);
				i++;
			}
			lines[lines.Count - 1].Choice = choice; // choice always lies in the last line of dialogue
		}

		return lines.ToArray();
	}
}
