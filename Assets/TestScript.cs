using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestScript : MonoBehaviour {
	public Button button;
	public DialogueSystem dialogue;

	void Start()
	{
		button.onClick.AddListener(delegate { dialogue.TriggerDialogue("scene1",0); });
	}
}
