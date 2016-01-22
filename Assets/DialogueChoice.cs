using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueChoice {
	public const int DIALOGUE_OPTION_COUNT = 2;

	public string[] ChoiceText { get; set; }
	public Dictionary<int,string> Targets { get; set; } // id and target

	public int AmountOfChoices {
		get {
			int i = 0;
			while(i < DIALOGUE_OPTION_COUNT) {
				if (ChoiceText[i] == null) break;
				else i++;
			}
			return i;
		}
	}

	/// <remarks>
	/// buttonNumber starts counting from 0
	/// </remarks>
	public int getID(int buttonNumber)
	{
		return new List<int>(Targets.Keys)[buttonNumber];
	}

	public DialogueChoice(string[] choiceText = null, Dictionary<int, string> targets = null)
	{
		ChoiceText = choiceText ?? new string[DIALOGUE_OPTION_COUNT];
		Targets = targets ?? new Dictionary<int,string>(DIALOGUE_OPTION_COUNT);
	}
}
