using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;

    private Queue<string> sentences;

    public PlayerMovementAdvanced playerMovement;
    public CinemachineFreeLook freeLookCamera;

    private string lastNPCName = "";


    void Start()
    {
        sentences = new Queue<string>();
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (playerMovement.isDialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            NextSentence();
        }
    }

    private Dialogue currentDialogue;

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue; // Сохраняем текущий диалог
        playerMovement.isDialogueActive = true;
        LockCamera(true);

        dialogueBox.SetActive(true);
        nameText.text = dialogue.name;
        sentences.Clear();

        lastNPCName = dialogue.name;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForEndOfFrame();
        }
    }

    void EndDialogue()
    {
        playerMovement.isDialogueActive = false;
        LockCamera(false);
        GameEventsManager.instance.miscEvents.NPCTalked(lastNPCName);
        dialogueBox.SetActive(false);
    }

    public void NextSentence()
    {
        DisplayNextSentence();
    }

    public void ShowShortMessage(string message, Dialogue dialogue)
    {
        playerMovement.isDialogueActive = true;
        dialogueBox.SetActive(true);
        nameText.text = dialogue.name;
        dialogueText.text = message;
    }

    void LockCamera(bool isLocked)
    {
        if (isLocked)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 300;
            freeLookCamera.m_YAxis.m_MaxSpeed = 2;
        }
    }

    public void ShowCompletionDialogue(Dialogue dialogue)
    {
        playerMovement.isDialogueActive = true;
        dialogueBox.SetActive(true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.completionDialogue)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }


    private IEnumerator WaitAndEndDialogue()
    {
        yield return new WaitForSeconds(3); // Показываем текст 3 секунды
        EndDialogue();
    }


}
