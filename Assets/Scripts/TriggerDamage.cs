using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    public int damageAmount = 20;
    private void OnTriggerEnter(Collider other)
    {
        CharacterStats characterStats = other.GetComponent<CharacterStats>();
        if (characterStats != null)
        {
            characterStats.TakeDamage(damageAmount);
        }
    }
}
