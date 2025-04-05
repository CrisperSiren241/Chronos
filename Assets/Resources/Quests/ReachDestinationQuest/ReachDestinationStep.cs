using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachDestinationStep : QuestStep
{
    void Start()
    {
        UpdateState();
    }
    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onDestinationReached += ReachDestination;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onDestinationReached -= ReachDestination;
    }

    private void ReachDestination()
    {
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    private void UpdateState()
    {
        string state = "";
        string status = "Неподалеку есть испытательный полигон. Пройдите его";
        ChangeState(state, status);
    }

}
