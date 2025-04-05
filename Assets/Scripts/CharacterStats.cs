using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Подключаем для работы с UI

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100; // Максимальное здоровье
    public int healAmount = 20; // Количество восстанавливаемого здоровья за одну флягу
    public int maxFlasks = 3;   // Максимальное количество лечебных фляг

    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int currentFlasks;

    //private Animator animator;

    public Text flaskCountText;
    public bool isInvincible = false;
    public bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;
        currentFlasks = maxFlasks;
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Использование фляги на клавишу R
        if (Input.GetKeyDown(KeyCode.R) && currentHealth < maxHealth)
        {
            if (UseHealthFlask()) // Лечение успешное
            {
                
                //animator.SetBool("IsHealing", true); // Активируем анимацию лечения
                SetLayerWeight(1, 1f); // Устанавливаем вес слоя на 1
                StartCoroutine(StopHealingAnimationAfterDelay(2f)); // Ожидаем завершения анимации
            }
        }
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье
        Debug.Log("Текущее здоровье: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // Если здоровье на нуле, вызываем метод Die
        }
    }

    private bool UseHealthFlask()
    {
        if (currentFlasks > 0) // Проверяем, остались ли фляги
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье максимумом
            currentFlasks--; // Уменьшаем количество фляг

            Debug.Log("Использована фляга. Текущее здоровье: " + currentHealth + ". Осталось фляг: " + currentFlasks);
            return true;
        }
        else
        {
            Debug.Log("Нет доступных фляг.");
            return false;
        }
    }

    private void Die()
    {
        Debug.Log("Персонаж умер.");
        //animator.SetTrigger("Death");
        isDead = true;
        GameEventsManager.instance.playerEvents.PlayerDeath();
    }

    private IEnumerator StopHealingAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Ждем завершения анимации лечения
        //animator.SetBool("IsHealing", false); // Возвращаем персонажа в обычное состояние
        SetLayerWeight(1, 0f); // Сбрасываем вес слоя
    }

     private void SetLayerWeight(int layerIndex, float weight)
    {
        // if (animator != null)
        // {
        //     animator.SetLayerWeight(layerIndex, weight);
        // }
    }

    public void ResetHealth(){
        currentHealth = maxHealth;        
    }
}
