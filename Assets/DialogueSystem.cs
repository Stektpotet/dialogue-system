using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public class DialogueSystem : MonoBehaviour {
	/**
	*	The results of your choices in dialogue are stored here.
	*	Every choice has a unique identifier given by button number * branch
	*	Ex. if your dialogue is in branch 1 and you choose button number 1,
	*	1*1 = 1 will be stored in the HashSet
	*   wait this doesn't work...
	*/
	public HashSet<int> ChoiceResults { get; set; }

	public bool IsDialogueActive { get { return panel.activeSelf; } }
	public bool IsLastLine { get { return lineIndex >= lines.Length-1; } }
	public bool IsChoice { get { return lines == null || lines[lineIndex].Choice != null; } }

	public Text dialogueText;
	public Image image;
	public GameObject panel;
	public Button[] dialogueOptions;
	public List<int> results;

	private DialogueLine[] lines;
	private int lineIndex = 0;
	private bool lineFinished = false;
	private bool choiceLoaded = false;
	private string dialogueScene; // purely to make sure the same trigger isn't activated twice
	private DialogueChoice choice;

	public void TriggerDialogue(string scene, int result, bool reset = false)
	{
		dialogueScene = (dialogueScene == null ? scene : dialogueScene);
		if (IsDialogueActive && scene == dialogueScene && !reset) // this dialogue already triggered
			return;

		lineIndex = 0;
		lineFinished = false;
		choiceLoaded = false;
		lines = XmlDialogueReader.LoadDialogue(scene);
		ActivateDialogueGUI(true);
		choice = lines[lines.Length - 1].Choice;
		DisplayLine(lines[0]);

		if (result != -1) results.Add(result);
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

	private void ActivateDialogueGUI(bool activate)
	{
		panel.SetActive(activate);
		if (IsChoice)
			ActivateButtons(activate);
	}

	private void ActivateButtons(bool activate)
	{
		foreach (var button in dialogueOptions) button.gameObject.SetActive(activate);
		if (activate)
		{
			for (int i = 0; i < choice.AmountOfChoices; i++)
			{
				print("target: " + choice.Targets[choice.getID(i)]);
				if (choice.Targets[choice.getID(i)] != null) // if there is a target scene
				{
					int tempVar = i; // lambdas am i right
					dialogueOptions[i].onClick.AddListener(
						() => ButtonListener(tempVar)
					);
				}
				else // else end dialogue
				{
					dialogueOptions[i].onClick.AddListener(
						() => ActivateDialogueGUI(false)
					);
				}
				dialogueOptions[i].GetComponentInChildren<Text>().text = choice.ChoiceText[i];
			}
		}
	}

	private void ButtonListener(int i) {
		var id = choice.getID(i);
		TriggerDialogue(choice.Targets[id], id);
		ActivateButtons(false);
	}

	void Start()
	{
		panel.SetActive(false);
		ActivateButtons(false);
		results = new List<int>();
	}

	void Update()
	{
        if (IsDialogueActive)
		{
			if (Input.GetKeyDown(KeyCode.Z)) // TODO rebindable
			{
				if (lineFinished && !IsChoice) // dont wanna display the line thats after the choice STUPIOD
				{
					if (IsLastLine)
					{
						panel.SetActive(false);
						return;
					}
					DisplayLine(lines[++lineIndex]);
				}
				else
				{
					dialogueText.text = lines[lineIndex].Text;
					lineFinished = true;
					StopCoroutine("TypeText");
				}
			}
			if (IsChoice && !choiceLoaded) // check if it now is a choice
			{
				ActivateButtons(true);
				choiceLoaded = true;
			}
		}
	}
}

