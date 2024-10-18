using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("Время активации ловушки")]
    [SerializeField] private float activationDelay;
    [Header("Время работы ловушки")]
    [SerializeField] private float activeTime;
    [Header("Урон ловушки")]
    [SerializeField] private float damage;
    [Header("Звук работы активной ловушки")]
    [SerializeField] private AudioClip trapSound;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Health playerHealth;

    // Активировалась ли ловушка
    private bool isTriggered;
    // Активна ли ловушка
    private bool isActive;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        isTriggered = false;
        isActive = false;
        spriteRenderer.color = Color.white;
    }
    private void Update()
    {
        // Урон по игроку если он прикоснулся
        // к активной ловушке
        if (playerHealth != null && isActive)
        {
            playerHealth.TakeDamage(false,damage);
        }
    }

    // При соприкосновении игрока с ловушкой
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // Получение компонента здоровья игрока
            playerHealth = collision.GetComponent<Health>();

            if(!isTriggered) 
            {
                // Активация ловушки
                StartCoroutine(ActivateFiretrap());
            }
            if (isActive)
            {
                // Урон по игроку
                collision.GetComponent<Health>().TakeDamage(false, damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Удаление компонента здоровья игрока
        // из памяти ловушки
        if(collision.tag == "Player")
        {
            playerHealth = null;
        }
    }

    // Функция, реализующая активацию ловушки
    private IEnumerator ActivateFiretrap()
    {
        // Состояние - активированное
        isTriggered = true;
        // Смена цвета спрайта
        spriteRenderer.color = Color.red;
        // Ожидание времени активации
        yield return new WaitForSeconds(activationDelay);
        // Воспроизведение звука  активной ловушки
        SoundManager.instance.PlaySound(trapSound);
        // Смена цвета спрайта на стандартный
        spriteRenderer.color = Color.white;
        // Ловушка активна
        isActive = true;
        // Воспроизведение анимации работающей ловушки
        animator.SetBool("isActivated", true);
        // Ожидание времени работы
        yield return new WaitForSeconds(activeTime);
        // Ловушка неактивна
        isActive = false;
        // Ловушка не активирована
        isTriggered = false;
        // Прекращения воспроизведения анимации
        animator.SetBool("isActivated", false);
    }
}
