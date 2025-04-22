using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectiles)
        {
            if (projectile != this.gameObject)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), projectile.GetComponent<Collider>());
            }
        }
    }

}