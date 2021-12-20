using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Text nameText;
    public Text dialogueText;
    public Canvas canvas;

    bool isTyping = false;
    string sentence;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (isTyping)
        {
            if (Input.GetButtonDown("Jump"))
            {
                StopAllCoroutines();
                dialogueText.text = sentence;
                isTyping = false;
            }
        }
        else if((!isTyping) && (FindObjectOfType<DialogueTrigger>().isChatting))
        {
            if (Input.GetButtonDown("Jump"))
            {
                NextDialogue();
            }
            
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        canvas.enabled = true;

        FindObjectOfType<NpcMovement>().canMove = false;
        FindObjectOfType<PlayerMovement>().canMove = false;
        FindObjectOfType<DialogueTrigger>().isChatting = true;

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        NextDialogue();
    }

    public void NextDialogue()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        sentence = sentences.Dequeue();
        StartCoroutine(TypeDialogue(sentence));
    }

    IEnumerator TypeDialogue(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }

    public void EndDialogue()
    {
        
        canvas.enabled = false;
        Debug.Log(canvas.enabled);

        FindObjectOfType<NpcMovement>().canMove = true;
        FindObjectOfType<PlayerMovement>().canMove = true;
        FindObjectOfType<DialogueTrigger>().isChatting = false;
    }
}
