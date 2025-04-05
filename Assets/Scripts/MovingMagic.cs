using UnityEngine;
using System.Collections;

public class MovingMagic : MonoBehaviour
{
    public Transform targetPoint;
    public float speed = 2f;
    public float delay = 2f;
    private Vector3 startPoint;
    private Coroutine moveCoroutine;

    void Start()
    {
        startPoint = transform.position;
    }

    public void MoveObject()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveToTargetAndBack());
    }

    private IEnumerator MoveToTargetAndBack()
    {
        while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        while (Vector3.Distance(transform.position, startPoint) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint, speed * Time.deltaTime);
            yield return null;
        }
        moveCoroutine = null;
    }
}
