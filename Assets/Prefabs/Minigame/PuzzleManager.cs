using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Transform[] disks; // Массив всех дисков
    public float[] targetAngles; // Желаемые углы вращения

    private void Update()
    {
        if (CheckSolution())
        {
            Debug.Log("Победа!");
            // Добавьте действия после решения головоломки
        }
    }

    private bool CheckSolution()
    {
        for (int i = 0; i < disks.Length; i++)
        {
            float currentAngle = disks[i].rotation.eulerAngles.y;
            float targetAngle = targetAngles[i];

            // Сравниваем углы с небольшой погрешностью
            if (Mathf.Abs(currentAngle - targetAngle) > 1f)
            {
                return false;
            }
        }
        return true;
    }
}