using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialoguePipeline : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueEvent
    {
        public string text;
        public bool hideOnContinue;
        public UnityEvent onContinue;
    }

    public TextMeshProUGUI textBox;
    [SerializeField]
    public DialogueEvent[] dialogueEvents;
    
    int dialogueIndex = 0;

    // starts automatically
    // call gameObject.SetActive(true) to resume
    void Start()
    {
        if(dialogueEvents.Length == 0) gameObject.SetActive(false);
        else textBox.SetText(dialogueEvents[dialogueIndex].text);
    }

    public void onClick()
    {
        if(dialogueEvents[dialogueIndex].hideOnContinue)
        {
            gameObject.SetActive(false);
        }
        dialogueEvents[dialogueIndex].onContinue?.Invoke();
        dialogueIndex++;
        if(dialogueIndex < dialogueEvents.Length) textBox.SetText(dialogueEvents[dialogueIndex].text);
    }
}