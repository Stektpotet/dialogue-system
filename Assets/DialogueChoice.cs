using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
        try
        {
            if (buttonNumber <= DIALOGUE_OPTION_COUNT - 1)
                return new List<int>(Targets.Keys)[buttonNumber];
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debug.Log(ex.Message);
        }
        return -1;
	}

    public string getTarget(int buttonNumber)
    {
        Debug.Log("button:" + buttonNumber);
        try
        {
            if (buttonNumber <= DIALOGUE_OPTION_COUNT - 1)
                return new List<string>(Targets.Values)[buttonNumber];
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debug.Log(ex.Message);
        }
        return null;
    }

	public DialogueChoice(string[] choiceText = null, Dictionary<int, string> targets = null)
	{
		ChoiceText = choiceText ?? new string[DIALOGUE_OPTION_COUNT];

		var tempTargets = new Dictionary<int, string>(DIALOGUE_OPTION_COUNT);
		for (int i = 0; i > DIALOGUE_OPTION_COUNT; i++)
			tempTargets.Add(i, "END"); // im desperate
		Targets = targets ?? tempTargets;
	}
}
