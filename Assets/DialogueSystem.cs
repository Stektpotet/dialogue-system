using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DialogueSystem : MonoBehaviour {
	public Text dialogueText;
	public Image image;

	private List<DialogueLine> lines;
	private int lineIndex = 0;
	private bool lineFinished;

	public List<DialogueLine> LoadDialogue(string scene)
	{
		var lines = new List<DialogueLine>();
		using (XmlReader reader = XmlReader.Create("Assets/Dialogue/" + scene + ".xml"))
		{
			reader.Read(); reader.Read(); // skip <dialogue> 

			while (reader.Read())
			{
				// </dialogue>
				if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "dialogue") break;
				if (reader.Name == "line")
				{
					lines.Add(GetDialogueLine(reader));
				}
			}
			reader.Close();
		}
		return lines;
	}

	private DialogueLine GetDialogueLine(XmlReader reader)
	{
		var line = new DialogueLine();
		print("Getting dialogue line.");
		string element = ""; // if this isn't overwritten, something probably went really wrong
		// read until </line> is encountered
		while (reader.Read())
		{
			if (reader.IsStartElement() && reader.NodeType == XmlNodeType.Element && reader.Name != "line") // dont read worthless </> tag scum
			{
				element = reader.Name;
			}
			else if (reader.NodeType == XmlNodeType.Text)
			{
				switch (element)
				{
					case "portrait":
						line.Portrait = Resources.Load<Sprite>(reader.Value.ToString());
						print(line.Portrait);
						break;
					case "text":
						line.Text = reader.Value;
						break;
					case "speed":
						line.TextSpeed = float.Parse(reader.Value);
						break;
					default:
						print("something probably went really wrong, line: " + reader.Name + " value: " + reader.Value);
						break;
				}
			}
			else
			{
				if (reader.Name == "line") break; // </line>
			}
        }
		return line;
	}

	private IEnumerator TypeText()
	{
		lineFinished = false;
		DialogueLine line = lines[lineIndex];
        foreach (char letter in line.Text.ToCharArray())
		{
			dialogueText.text += letter;
			yield return 0;
			if (!char.IsWhiteSpace(letter)) yield return new WaitForSeconds(line.TextSpeed);
		}
		lineFinished = true;
	}

	private void DisplayLine(DialogueLine line)
	{
		image.sprite = line.Portrait;
		StopCoroutine("TypeText");
		dialogueText.text = string.Empty;
		StartCoroutine("TypeText");
	}

	void Start()
	{
		lineFinished = false;
		lines = LoadDialogue("scene1");
		DisplayLine(lines[0]);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (lineFinished)
			{
				DisplayLine(lines[++lineIndex]);
			}
			else
			{
				dialogueText.text = lines[lineIndex].Text;
				lineFinished = true;
				StopCoroutine("TypeText");
			}
		}
	}
}