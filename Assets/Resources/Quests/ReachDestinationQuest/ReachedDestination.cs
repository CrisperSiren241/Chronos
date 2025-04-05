using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachedDestination : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameEventsManager.instance.miscEvents.DestinationReached();
    }
}
