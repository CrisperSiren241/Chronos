using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    // Start is called before the first frame update
    Transform AimCheck;
    public Transform _Barrel;
    float dist;
    public float howClose;
    public Transform head;
    public GameObject _projectile;
    public float fireRate, nextFire;
    void Start()
    {
        AimCheck = GameObject.FindGameObjectWithTag("AimCheck").transform;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(AimCheck.position, transform.position);
        if (dist <= howClose)
        {
            // Получаем направление к игроку
            Vector3 direction = AimCheck.position - head.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            head.rotation = Quaternion.Slerp(head.rotation, targetRotation, Time.deltaTime * 5f);

            Debug.DrawRay(_Barrel.position, direction.normalized * 5, Color.red, 2f); // Используем нормализованное направление для отладки

            if (Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                Shoot(direction); // Передаем точное направление к игроку
            }
        }
    }

    void Shoot(Vector3 direction)
    {
        // Инстанциируем снаряд
        GameObject clone = Instantiate(_projectile, _Barrel.position, Quaternion.identity);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Применяем силу в направлении к игроку
            rb.AddForce(direction.normalized * 3000);
        }
        Destroy(clone, 10f);
    }

}