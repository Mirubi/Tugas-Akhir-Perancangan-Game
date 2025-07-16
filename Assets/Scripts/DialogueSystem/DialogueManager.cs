using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{

    public enum SpeakerType { Player, NPC };

    [System.Serializable]
    public class DialogData
    {
        public SpeakerType speakerType;
        [TextArea(4, 6)]
        public string dialog;
        [Header("Opsional Ganti Data Default")]
        public string overrideName;
        public Sprite overrideIcon;
    }

    [Header("Info Karakter Default")]
    public string defaultPlayerName = "Yi";
    public Sprite defaultPlayerIcon;
    public string defaultNpcName = "White-Haired Girl";
    public Sprite defaultNpcIcon;

    private List<DialogData> dialogsToPlay;

    [Header("Hubungkan Objek dari Hierarchy")]
    public GameObject playerBubbleObject;
    public GameObject npcBubbleObject;

    [Header("Pengaturan")]
    public float typingSpeed = 0.04f;

    [Header("Events")]
    public UnityEvent onDialogueFinish;

    private int currentDialogIndex = 0;
    private bool isDialogueActive = false;
    private DialogueBubbleUI playerBubbleUI;
    private DialogueBubbleUI npcBubbleUI;

    void Start()
    {
        playerBubbleUI = playerBubbleObject.GetComponent<DialogueBubbleUI>();
        npcBubbleUI = npcBubbleObject.GetComponent<DialogueBubbleUI>();
        playerBubbleObject.SetActive(false);
        npcBubbleObject.SetActive(false);
    }

    void Update()
    {
        if (!isDialogueActive) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(List<DialogData> newDialogs)
    {
        if (isDialogueActive) return;

        isDialogueActive = true;
        this.dialogsToPlay = newDialogs;
        currentDialogIndex = 0;
        ShowDialogue();
    }


    private void AdvanceDialogue()
    {
        currentDialogIndex++;
        if (currentDialogIndex < dialogsToPlay.Count)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowDialogue()
    {
        DialogData data = dialogsToPlay[currentDialogIndex];
        string nameToShow;
        Sprite iconToShow;
        DialogueBubbleUI bubbleToSetup;

        if (data.speakerType == SpeakerType.Player)
        {
            nameToShow = string.IsNullOrEmpty(data.overrideName) ? defaultPlayerName : data.overrideName;
            iconToShow = data.overrideIcon == null ? defaultPlayerIcon : data.overrideIcon;
            bubbleToSetup = playerBubbleUI;
            playerBubbleObject.SetActive(true);
            npcBubbleObject.SetActive(false);
        }
        else // Jika NPC
        {
            nameToShow = string.IsNullOrEmpty(data.overrideName) ? defaultNpcName : data.overrideName;
            iconToShow = data.overrideIcon == null ? defaultNpcIcon : data.overrideIcon;
            bubbleToSetup = npcBubbleUI;
            npcBubbleObject.SetActive(true);
            playerBubbleObject.SetActive(false);
        }
        bubbleToSetup.Setup(nameToShow, data.dialog, iconToShow, typingSpeed);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        playerBubbleObject.SetActive(false);
        npcBubbleObject.SetActive(false);
        onDialogueFinish?.Invoke();
    }
}