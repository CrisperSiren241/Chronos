using System.Threading;
using UnityEngine;

public class PortalCreate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static int count = 0;
    public GameObject enterPortal; 
    public GameObject exitPortal; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Projectile")){
            Vector3 contactPoint = other.contacts[0].point;
            if(count > 0) Instantiate(enterPortal, contactPoint, Quaternion.identity);
            else Instantiate(exitPortal, contactPoint, Quaternion.identity);
        }
    }
}
