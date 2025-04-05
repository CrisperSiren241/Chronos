using UnityEngine;
using System.Collections;

public class StartQuestMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created  public GameObject puzzleCanvas;
    public string questId;
    private bool isPlayerInTrigger;
    public float moveSpeed = 2f;

    QuestState currentQuestState;

    private bool isMoving = false;

    void Update()
    {
        Debug.Log(QuestManager.Instance);
        currentQuestState = QuestManager.Instance.CheckQuestState(questId);

        if (currentQuestState == QuestState.IN_PROGRESS && !isMoving)
        {
            StartCoroutine(MovePlatform());
        }
    }

    private IEnumerator MovePlatform()
    {
        isMoving = true;
        Vector3 targetPosition = new Vector3(transform.position.x, 17f, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }


}
