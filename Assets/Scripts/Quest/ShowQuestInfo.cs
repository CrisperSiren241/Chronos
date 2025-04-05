using UnityEngine;
using TMPro;
using System.Linq;

public class ShowQuestInfo : MonoBehaviour
{
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public TextMeshProUGUI questProgressText;

    private QuestManager questManager;
    private Quest currentQuest;

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    public void ShowQuestById(string questId)
    {
        if (questManager == null) return;

        currentQuest = questManager.GetQuestById(questId);
        if (currentQuest != null)
        {
            UpdateQuestInfo();
        }
        else
        {
            ClearQuestInfo();
            Debug.LogWarning($"Квест с ID {questId} не найден.");
        }
    }

    private void UpdateQuestInfo()
    {
        if (currentQuest == null) return;

        questTitleText.text = currentQuest.info.displayName;
        questDescriptionText.text = GetQuestDescription(currentQuest);
        questProgressText.text = GetQuestProgress(currentQuest);
    }

    private string GetQuestDescription(Quest quest)
    {
        if (quest.state == QuestState.REQUIREMENTS_NOT_MET)
            return "Требования к началу квеста не выполнены.";
        if (quest.state == QuestState.CAN_START)
            return "Квест готов к началу!";
        if (quest.state == QuestState.IN_PROGRESS)
            return "Квест в процессе выполнения.";
        if (quest.state == QuestState.CAN_FINISH)
            return "Квест готов к завершению!";
        if (quest.state == QuestState.FINISHED)
            return "Квест завершён!";

        return "Неизвестное состояние квеста.";
    }

    private string GetQuestProgress(Quest quest)
    {
        if (quest.state == QuestState.IN_PROGRESS && quest.CurrentStepExists())
        {
            return $"Шаг {quest.currentQuestStepIndex + 1} из {quest.info.questStepPrefabs.Length}";
        }

        if (quest.state == QuestState.FINISHED)
        {
            return "Прогресс: 100%";
        }

        return "Прогресс отсутствует.";
    }

    private void ClearQuestInfo()
    {
        questTitleText.text = "";
        questDescriptionText.text = "";
        questProgressText.text = "";
    }
}
