using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class DialogueSystem : MonoBehaviour {
	public bool isDialogueActive { get { return panel.activeSelf; } }

	public Text dialogueText;
	public Image image;
	public GameObject panel;
	public AudioSource source;

	private DialogueLine[] lines;
	private int lineIndex = 0;
	private bool lineFinished = false;
	
	public void TriggerDialogue(string scene)
	{
		if (isDialogueActive) return; // dialogue already triggered
		panel.SetActive(true);
		lines = XmlDialogueReader.LoadDialogue(scene);
		DisplayLine(lines[0]);
	}

	private IEnumerator TypeText()
	{
		lineFinished = false;
		DialogueLine line = lines[lineIndex];
		PlayDialogueSound(line.Sound);
		foreach (char letter in line.Text.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForEndOfFrame();
			if (!char.IsWhiteSpace(letter)) // don't type out spaces
				yield return new WaitForSeconds(line.TextSpeed);
			else {
				PlayDialogueSound(line.Sound); // play only on spaces
			}
		}
		lineFinished = true;
	}

	private void PlayDialogueSound(AudioClip sound)
	{
		if (source.isPlaying) source.Stop();
		source.pitch = Random.Range(0.7f, 1.3f);
		source.PlayOneShot(sound);
	}

	private void DisplayLine(DialogueLine line)
	{
		StopCoroutine("TypeText");
		image.sprite = line.Portrait;
		dialogueText.text = string.Empty;
		dialogueText.font = line.Font;
		StartCoroutine("TypeText");
	}

	void Start()
	{
		panel.SetActive(false);
	}

	void Update()
	{
        if (isDialogueActive)
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