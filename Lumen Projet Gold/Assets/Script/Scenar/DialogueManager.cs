using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;

	public Animator animator;
	public Image myImage;
	public static int numberOfClicks = 0;
	public string[] nameArray = new string[1];
	public string[] textArray = new string[1];
	public Sprite[] spriteArray = new Sprite[1];
	public int lvlID;
	
	private Queue<string> sentences;
	private Queue<string> names;
	private Queue<Sprite> sprites;
	public Testing2 testingScript;

	// Use this for initialization
	void Start () {
		numberOfClicks = 0;
		sentences = new Queue<string>();
		names = new Queue<string>();
		sprites = new Queue<Sprite>();
		animator.SetBool("IsOpen", true);
		DisplayNextSentence();
	}

	private void Update()
	{
		if (Testing2.activateTutorial == true)
		{
			animator.SetBool("IsOpen", true);
		}
	}
	public void StartDialogue ()
	{
		animator.SetBool("IsOpen", true);
		
	}

	public void DisplayNextSentence ()

	{
		if (lvlID == 1 && numberOfClicks == 1)
		{
			testingScript.SwitchTutorial();
			numberOfClicks++;

			return;
		}
		if (numberOfClicks >= nameArray.Length)
		{
			EndDialogue();
			return;
		}
		Debug.Log("Ca a marché");
		Sprite newSprite = spriteArray[numberOfClicks];
		Debug.Log(newSprite);
		myImage.sprite = newSprite;
		nameText.text = nameArray[numberOfClicks];
		string sentence = textArray[numberOfClicks];
		//Sprite mySprite = sprites.Dequeue();
		//string sentence = sentences.Dequeue();
		//string name = names.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
		numberOfClicks++;
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
	}

}
