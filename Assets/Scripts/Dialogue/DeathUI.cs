using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public GameObject deathUI;
    private string scene;
    void Start()
    {
        deathUI.SetActive(false);
    }

    void OnEnable()
    {
        GameEventsManager.instance.playerEvents.onPlayerDeath += DisplayPanel;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPlayerDeath -= DisplayPanel;

    }

    void DisplayPanel()
    {
        if (PauseMenu.isAnyMenuOpen) return;
        deathUI.SetActive(true);
        // CameraService.Instance.Lock();
        PauseMenu.isAnyMenuOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        // DataPersistenceManager.instance.SaveGame();   
        // stats.isDead = false;
        // stats.currentHealth = stats.maxHealth;
        // CameraService.Instance.UnLock();
        deathUI.SetActive(false);
        PauseMenu.isAnyMenuOpen = false;
        if (QuestManager.Instance != null)
        {
            foreach (Quest quest in QuestManager.Instance.questMap.Values)
            {
                QuestManager.Instance.SaveQuest(quest);
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
