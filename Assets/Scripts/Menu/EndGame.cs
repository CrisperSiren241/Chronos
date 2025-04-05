using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject endGamePanel;
    void Start()
    {
        endGamePanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        endGamePanel.SetActive(true);
        PauseMenu.isAnyMenuOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnTriggerExit(Collider other)
    {
        endGamePanel.SetActive(false);
        PauseMenu.isAnyMenuOpen = false;
    }
}
