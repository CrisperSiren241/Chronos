using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretScript : MonoBehaviour
{
    // Start is called before the first frame update
    Transform _Player;
    public Transform _Barrel;
    float dist;
    public float howClose;
    public Transform head;
    public GameObject _projectile;
    public float fireRate, nextFire;
    public float TimeDestroy = 5f;
    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(_Player.position, transform.position);

        if (Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Shoot();
        }

    }

    void Shoot()
    {
        GameObject clone = Instantiate(_projectile, _Barrel.position, Quaternion.identity);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(_Barrel.forward * 5000);
        }
        Destroy(clone, TimeDestroy);
    }

}