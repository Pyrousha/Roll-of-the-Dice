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
        public UnityEvent onContinue; // if null, continue to next bit of text, otherwise wait 
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
        if(dialogueEvents[dialogueIndex].onContinue != null)
        {
            dialogueEvents[dialogueIndex].onContinue.Invoke();
            gameObject.SetActive(false);
        }
        dialogueIndex++;
        textBox.SetText(dialogueEvents[dialogueIndex].text);
    }
}