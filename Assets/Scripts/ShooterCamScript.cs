using Cinemachine;
using UnityEngine;

public class ShooterCamScript : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCamera;
    public CinemachineFreeLook combatPersonCamera;

    void Start()
    {
        thirdPersonCamera.Priority = 10;
        combatPersonCamera.Priority = 0;
    }

    void Update()
    {
        CameraChange();
    }

    void CameraChange()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            combatPersonCamera.Priority = 20;
            thirdPersonCamera.Priority = 10; 
        }
        else
        {
            combatPersonCamera.Priority = 10;
            thirdPersonCamera.Priority = 20; 
        }
    }
}
