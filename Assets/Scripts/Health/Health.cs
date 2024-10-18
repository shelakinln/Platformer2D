using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Количество жизней")]
    [SerializeField] private float startLives;
    [Header("Начальное здоровье персонажа")]
    [SerializeField] private float startHealth;
    [Header("Количество кадров неуязвимости")]
    [SerializeField] private float iFramesDuration;
    [Header("Количество миганий спрайта игрока")]
    [SerializeField] private int numberOfFlashes;
    [Header("Скрипты персонажа")]
    [SerializeField] private Behaviour[] components;
    [Header("Звук ранения")]
    [SerializeField] private AudioClip hurtSound;
    [Header("Звук смерти")]
    [SerializeField] private AudioClip deathSound;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float currentLives;
    // Текущее здоровье персонажа
    public float currentHealth;
    // Состояние персонажа
    private bool dead;
    // Статус неуязвимости
    private bool isInvulnerable;

    private void Awake()
    {
        currentHealth = startHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLives = startLives;
    }

    public void TakeDamage(bool isFallTrigger, float _damage) // Функция получения урона
    {
        if (isInvulnerable && !isFallTrigger) // Урон не проходит если персонаж неуязвим
            return;

        // Снижение очков здоровья
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startHealth);

        if (currentHealth > 0)
        {
            // Корутина отвечающая за неуязвимость
            StartCoroutine(Invunerabilty());
            // Воспроизведение звука ранения
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead) // Если нанесён летальный урон
            { 
                // Деактивация всех скриптов песонажа
                foreach (Behaviour component in components) 
                    component.enabled = false;

                //boxCollider.enabled = false;
                gameObject.layer = 0;

                // Воспроизведение анимации смерти персонажа
                animator.SetTrigger("dead");
                animator.SetBool("isGrounded", true);

                // Состояние - умер
                dead = true;
                currentLives--;
                // Воспроизведение звука смерти персонажа
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    private IEnumerator Invunerabilty() // Реализация неуязвимости
    {
        //Персонаж неуязвим
        isInvulnerable = true;

        // Коллизии игрока и противников игнорируют друг друга
        Physics2D.IgnoreLayerCollision(8, 9, true);

        // Мигание спрайта персонажа при повреждении
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new 
                WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new 
                WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        isInvulnerable = false;
    }

    private void Deactivate()
    {
        // Функция деактивации персонажа, которая используется
        // после воспроизведения анимации его смерти
        gameObject.SetActive(false);
    }

    private void AddHealth(float _value)
    {
        // Добавление здоровья персонажу
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startHealth);
    }

    public void Respawn() // Функция возрождения на точке сохранения
    {
        // Состояние - жив
        dead = false;

        // Здоровье возвращается к начальному значению
        AddHealth(startHealth);

        gameObject.layer = 8;

        // Включение idle-анимации
        animator.ResetTrigger("dead");
        animator.Play("Idle");

        // Неуязвимость после возрождения
        StartCoroutine(Invunerabilty());

        // Повторная активация скриптов персонажа
        foreach (Behaviour component in components) 
            component.enabled = true;
    }
}
