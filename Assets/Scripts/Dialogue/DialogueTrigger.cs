using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    
    public PlayerMovementAdvanced playerMovement;
    private bool hasBeenTriggered = false;
    private bool isPlayerInTrigger = false;

    void Update()
    {
        // Проверяем, находится ли игрок в зоне триггера и нажата ли клавиша "E"
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            playerMovement.isDialogueActive = false;
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        QuestManager questManager = FindObjectOfType<QuestManager>();

        if (questManager != null)
        {
            QuestState questState = questManager.CheckQuestState(dialogue.questId);

            if (questState == QuestState.CAN_FINISH)
            {
                dialogueManager.ShowCompletionDialogue(dialogue);
                hasBeenTriggered = true;
                return;
            }
            else if (questState == QuestState.FINISHED)
            {
                string lastSentence = dialogue.completionDialogue[dialogue.completionDialogue.Length - 1];
                dialogueManager.ShowShortMessage(lastSentence, dialogue);
                return;
            }
        }

        if (!hasBeenTriggered)
        {
            dialogueManager.StartDialogue(dialogue); // Начало полного диалога 
            hasBeenTriggered = true;
        }
        else
        {
            string lastSentence = dialogue.sentences[dialogue.sentences.Length - 1];
            dialogueManager.ShowShortMessage(lastSentence, dialogue); // Отображение последней реплики 
        }
    }

}
