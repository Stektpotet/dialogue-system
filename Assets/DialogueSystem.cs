using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class DialogueSystem : MonoBehaviour {
	public bool DialogueActive { get { return panel.activeSelf; } }

	public Text dialogueText;
	public Image image;
	public GameObject panel;
	public AudioSource source;

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
		PlayDialogueSound(line.Sound);
		foreach (char letter in line.Text.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForEndOfFrame();
			if (!char.IsWhiteSpace(letter)) // don't type out spaces
				yield return new WaitForSeconds(line.TextSpeed);
			else {
				PlayDialogueSound(line.Sound);
			}
		}
		source.pitch = 1;
		lineFinished = true;
	}

	private void PlayDialogueSound(AudioClip sound) {
		if (source.isPlaying) source.Stop();
		source.pitch = Random.Range(0.5f, 1.5f);
		source.PlayOneShot(sound);
	}

	private void DisplayLine(DialogueLine line)
	{
		image.sprite = line.Portrait;
		StopCoroutine("TypeText");
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