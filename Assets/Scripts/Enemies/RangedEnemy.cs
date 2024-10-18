using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
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
    [SerializeField] private AudioClip shotSound;
    [Header("Место выстрела противника")]
    [SerializeField] private Transform firepoint;
    [Header("Объекты пуль противника")]
    [SerializeField] private GameObject[] bullets;

    // Таймер, запускаемый после атаки
    private float cooldownTimer = Mathf.Infinity;

    private Animator animator;
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
            if (cooldownTimer > attackCooldown)
            {
                cooldownTimer = 0;
                animator.SetTrigger("rangeAttack");
            }
        }
        if (patrol != null) 
            patrol.enabled = !PlayerInSight();
    }

    // Функция, определяющая есть ли игрок в радиусе атаки
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + 
                transform.right * range * transform.localScale.x * 
                colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, 
                boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
            0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    // Функция, отрисовывающая в редакторе радиус атаки
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * 
                range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, 
            boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // Выстрел в игрока после анимации атаки
    private void RangedAttack()
    {
        // Воспроизведение звука выстрела
        SoundManager.instance.PlaySound(shotSound);
        // Обнуление таймера
        cooldownTimer = 0;
        // Перемещение объекта пули на место выстрела
        bullets[FindBullet()].transform.position = firepoint.position;
        // Активация объекта пули
        bullets[FindBullet()].GetComponent<EnemyProjectile>().
            ActivateEnemyProjectile(damage);
    }

    // Функция, возвращающая индекс первого
    // неактивного в иерархии объекта пули
    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy) return i;
        }
        return 0;
    }
}
