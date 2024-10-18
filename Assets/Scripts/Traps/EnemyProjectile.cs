using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [Header("Скорость перемещения проджектайла")]
    [SerializeField] private float speed;
    [Header("Время жизни проджектайла")]
    [SerializeField] private float resetTime;

    private Animator animator;
    private BoxCollider2D boxCollider;

    // Попал ли снаряд в игрока
    private bool hit;
    // Счётчик времени существования снаряда
    private float lifeTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        // Движение снаряда по горизонтали
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        // Счётчик существования
        lifeTime += Time.deltaTime;

        // Деактивация снаряда по истечению счётчика
        if(lifeTime > resetTime) 
            gameObject.SetActive(false);
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifeTime = 0;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
    }

    public void ActivateEnemyProjectile(int _damage)
    {
        hit = false;
        lifeTime = 0;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
        base.damage = _damage;
    }

    // Попадание коллизии снаряда в коллизию другого объекта
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag != "Bullet" && collision.transform.tag != "Enemy")
        {
            // Попадание
            hit = true;

            // Урон по игроку
            base.OnTriggerEnter2D(collision);

            // Отключение коллизии снаряда
            boxCollider.enabled = false;

            // Воспроизведение анимации уничтожения снаряда
            if (animator != null)
                animator.SetTrigger("explode");
            else
                gameObject.SetActive(false);
        }
    }

    // Деактивация объекта после анимации его уничтожения
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
