using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
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