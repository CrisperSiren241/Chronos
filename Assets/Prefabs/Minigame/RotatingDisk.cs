using UnityEngine;

public class RotatingDisk : MonoBehaviour
{
    public Transform linkedDisk;
    public float linkedDiskSpeed = 0.5f;
    public float rotationStep = 10f; // Шаг вращения в градусах

    private void OnMouseDown()
    {
        // Вращаем основной диск
        transform.Rotate(0, rotationStep, 0);

        // Вращаем связанный диск (если есть)
        if (linkedDisk != null)
        {
            linkedDisk.Rotate(0, rotationStep * linkedDiskSpeed, 0);
        }
    }
}