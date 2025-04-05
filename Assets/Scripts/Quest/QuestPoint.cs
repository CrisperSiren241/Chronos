using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]

    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    [Header("Quest Assignment")]
    [SerializeField] private bool assignBySubmit = true;
    [SerializeField] private bool assignByDialogue = false;
    [SerializeField] private string npcName = "";

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;
    public GameObject objectToActive;

    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.instance.miscEvents.onNPCTalked += QuestAssignByDialogue;
        GameEventsManager.instance.inputEvents.onSubmitPressed += QuestAssignBySubmit;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.miscEvents.onNPCTalked -= QuestAssignByDialogue;
        GameEventsManager.instance.inputEvents.onSubmitPressed -= QuestAssignBySubmit;
    }


    public void QuestAssignBySubmit()
    {
        if (!assignByDialogue && assignBySubmit)
        {
            SubmitPressed();
        }
    }

    void QuestAssignByDialogue(string npcName)
    {
        if (assignByDialogue && !assignBySubmit)
        {
            if (npcName == this.npcName)
                SubmitPressed();
        }
    }

    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return;
        }

        // start or finish a quest
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            GameEventsManager.instance.questEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        Debug.Log("Current State: " + quest.state);
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            // questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }


    void OnValidate()
    {
#if UNITY_EDITOR
        if (assignBySubmit)
            assignByDialogue = false;
        else if (assignByDialogue)
            assignBySubmit = false;

        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

}