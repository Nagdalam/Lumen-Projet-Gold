using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	bool tutorialOpen = false;
	public Animator panelAnim;
	bool isFlashing = false;
	public GameObject button;
	// Use this for initialization
	void Start () {
		numberOfClicks = 0;
		sentences = new Queue<string>();
		names = new Queue<string>();
		sprites = new Queue<Sprite>();
		if(lvlID != 3) { 
		animator.SetBool("IsOpen", true);
		DisplayNextSentence();
		}
	}


	private void Update()
	{
		if (Testing2.activateTutorial == true)
		{
			animator.SetBool("IsOpen", true);
		}

		if (lvlID == 3 && GameManager.numberOfLights == 1 && tutorialOpen == false)
		{
			
			tutorialOpen = true;
			animator.SetBool("IsOpen", true);
			DisplayNextSentence();
		}
		if(lvlID == 5 && GameManager.numberOfLights == 0 && isFlashing == false)
		{
			
			StartCoroutine(WaitASecond());
		}
	}
	public void StartDialogue ()
	{
		animator.SetBool("IsOpen", true);
		
	}

	

	public void DisplayNextSentence ()

	{
		Debug.Log("Ca a marché");
		if (lvlID == 1 && numberOfClicks == 1)
		{
			Debug.Log("hey");
			testingScript.SwitchTutorial();
			numberOfClicks++;

			return;
		}
		

		if (numberOfClicks >= nameArray.Length)
		{
			if(lvlID == 2)
			{
				testingScript.SwitchTutorial();
			}
			if (lvlID == 3)
			{
				testingScript.SwitchTutorial();
			}
			if (lvlID == 4)
			{
				SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
			}
			if(lvlID == 5)
			{
				button.SetActive(false);

			}
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

	IEnumerator WaitASecond()
	{
		yield return new WaitForSeconds(2f);
		panelAnim.SetBool("isFlashing", true);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
	}
}
