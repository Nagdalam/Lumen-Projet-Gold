using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;
    public Image imageToChange;
    public Sprite spriteToChange;
    //public DialogueManager bidon;
    public void TriggerDialogue ()
	{

		FindObjectOfType<DialogueManager>().StartDialogue();
        //bidon.StartDialogue(dialogue);
        imageToChange.sprite = spriteToChange;
    }

}
