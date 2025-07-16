using System.Collections.Generic;
using UnityEngine;

// Pastikan objek ini punya Collider
[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Data Dialog")]
    public List<DialogueManager.DialogData> dialogs;

    [Header("Pengaturan Trigger")]
    [Tooltip("Hubungkan Dialogue Manager yang ada di scene")]
    public DialogueManager dialogueManager;

    [Tooltip("Centang jika dialog hanya ingin muncul sekali saja")]
    public bool playOnce = true;

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playOnce && hasPlayed)
            {
                return;
            }

            dialogueManager.StartDialogue(dialogs);
            hasPlayed = true;
            Debug.Log("collide");
        }
    }
}