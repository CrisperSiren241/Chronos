using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject spawnPrefab; 

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Instantiate(spawnPrefab, contactPoint, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
