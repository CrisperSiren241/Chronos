using UnityEngine;

public class ProjectilePortal : MonoBehaviour
{
    public GameObject bluePortalPrefab;
    public GameObject orangePortalPrefab;
    private static GameObject bluePortal;
    private static GameObject orangePortal;
    
    private static bool nextIsBlue = true; // Флаг, какой портал создавать следующим

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            Vector3 contactPoint = other.contacts[0].point;
            Quaternion contactRotation = Quaternion.LookRotation(other.contacts[0].normal);

            if (nextIsBlue)
            {
                if (bluePortal != null)
                {
                    Destroy(bluePortal);
                }
                bluePortal = Instantiate(bluePortalPrefab, contactPoint, contactRotation);
            }
            else
            {
                if (orangePortal != null)
                {
                    Destroy(orangePortal);
                }
                orangePortal = Instantiate(orangePortalPrefab, contactPoint, contactRotation);
            }

            // Если оба портала существуют, связываем их
            if (bluePortal != null && orangePortal != null)
            {
                Portal bluePortalComponent = bluePortal.GetComponent<Portal>();
                Portal orangePortalComponent = orangePortal.GetComponent<Portal>();

                if (bluePortalComponent != null && orangePortalComponent != null)
                {
                    bluePortalComponent.linkedPortal = orangePortalComponent;
                    orangePortalComponent.linkedPortal = bluePortalComponent;
                }
            }

            // Чередуем тип следующего портала
            nextIsBlue = !nextIsBlue;

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
