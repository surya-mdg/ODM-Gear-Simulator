using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    bool isNearby = false;

    [HideInInspector]
    public bool isChatting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isNearby = false;
        }
    }

    private void Update()
    {
        if (isNearby && (!isChatting))
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Ok");
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
        }
    }
}
