using UnityEngine;

public class AimPlayer : MonoBehaviour
{

    public GameObject aimPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aimPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            aimPanel.SetActive(true);
        }
        else
        {
            aimPanel.SetActive(false);

        }
    }
}
