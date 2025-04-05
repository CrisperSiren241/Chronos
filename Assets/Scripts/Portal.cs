using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal; 
    private bool isPlayerTeleporting = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && linkedPortal != null && !isPlayerTeleporting)
        {
            StartCoroutine(TeleportPlayer(other));
        }
    }

    private IEnumerator TeleportPlayer(Collider player)
    {
        isPlayerTeleporting = true;
        linkedPortal.isPlayerTeleporting = true;

        Vector3 offset = player.transform.position - transform.position;
        player.transform.position = linkedPortal.transform.position + offset;

        yield return new WaitForSeconds(0.2f);

        isPlayerTeleporting = false;
        linkedPortal.isPlayerTeleporting = false;
    }
}
