using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DialogueSystem : MonoBehaviour {
	public bool DialogueActive { get { return panel.activeSelf; } }

	public Text dialogueText;
	public Image image;
	public GameObject panel;

	private DialogueLine[] lines;
	private int lineIndex = 0;
	private bool lineFinished = false;
	
	public void TriggerDialogue(string scene)
	{
		if (DialogueActive) return; // dialogue already triggered
		panel.SetActive(true);
		lines = XmlDialogueReader.LoadDialogue(scene);
		DisplayLine(lines[0]);
	}

	private IEnumerator TypeText()
	{
		lineFinished = false;
		DialogueLine line = lines[lineIndex];
        foreach (char letter in line.Text.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForEndOfFrame();
			if (!char.IsWhiteSpace(letter)) // don't type out spaces
				yield return new WaitForSeconds(line.TextSpeed);
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
		panel.SetActive(false);
	}

	void Update()
	{
        if (DialogueActive)
		{
			if (Input.GetKeyDown(KeyCode.Z)) // TODO rebindable
			{
				if (lineIndex >= lines.Length-1 && lineFinished)
				{
					panel.SetActive(false);
					return;
				}

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
}