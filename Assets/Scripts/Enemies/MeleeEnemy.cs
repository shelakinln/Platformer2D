using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Задержка между атаками")]
    [SerializeField] private float attackCooldown;
    [Header("Дальность видимости противника")]
    [SerializeField] private float range;
    [Header("Расстояние от противника до начальной точки радиуса атаки")]
    [SerializeField] private float colliderDistance;
    [Header("Урон, наносимый противником")]
    [SerializeField] private int damage;
    [Header("Коллайдер противник")]
    [SerializeField] private BoxCollider2D boxCollider;
    [Header("Слой игрока")]
    [SerializeField] private LayerMask playerLayer;
    [Header("Звук атаки противника")]
    [SerializeField] private AudioClip attackSound;

    // Таймер, запускаемый после атаки
    private float cooldownTimer = Mathf.Infinity;

    private Animator animator;
    private Health playerHealth;
    private EnemyPatrol patrol;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        patrol = GetComponentInParent<EnemyPatrol>();
    }
    private void Update()
    {
        // Счётчик
        cooldownTimer += Time.deltaTime;

        // Атака, если есть возможность
        if (PlayerInSight())
        {
            if(cooldownTimer > attackCooldown && 
                playerHealth.currentHealth > 0)
            {
                // Обнуление таймера
                cooldownTimer = 0;
                // Запуск анимации атаки
                animator.SetTrigger("meleeAttack");
                // Воспроизведение звука атаки
                SoundManager.instance.PlaySound(attackSound);
            }
        }
        if(patrol != null) 
            patrol.enabled = !PlayerInSight();
    }

    // Функция, определяющая есть ли игрок в радиусе атаки
    private bool PlayerInSight()
    {
        // Луч, проверяющий есть ли перед противником игрок
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + 
                transform.right * range * transform.localScale.x * 
                colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, 
            boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
            0, Vector2.left, 0, playerLayer);

        // Получение компонента здоровья игрока
        if(hit.collider != null) 
            playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
    }

    // Функция, отрисовывающая в редакторе радиус атаки
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * 
                transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, 
                boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // Атака игрока после анимации атаки
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(false, damage);
        }
    }
}
