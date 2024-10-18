using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Минимальное время между выстрелами")]
    [SerializeField] public float attackCooldown;
    [Header("Пули")]
    [SerializeField] private GameObject[] bullets;
    [Header("Звук выстрела")]
    [SerializeField] private AudioClip shotSound;

    private PlayerMovement playerMovement;
    private Transform firePoint;

    // Счётчик времени до возможности выстрелить ещё раз
    public float cooldownTimer;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        firePoint = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            //Debug.Log(transform.localScale.x);
            // Выстрел если есть для него возможность
            if (Input.GetMouseButton(0) &&
                cooldownTimer > attackCooldown &&
                playerMovement.canAttack())
                Attack();

            // Отсчёт времени для повторного выстрела
            cooldownTimer += Time.deltaTime;
        }
    }

    private void Attack() // Реализация выстрела
    {
        // Воспроизведение звука выстрела
        SoundManager.instance.PlaySound(shotSound);

        // Обнуление счётчика после выстрела
        cooldownTimer = 0;

        // Перемещение объекта пули на место выстрела
        bullets[FindBullet()].transform.position = firePoint.position;

        // Активация объекта пули
        bullets[FindBullet()].GetComponent<Projectile>().
            SetDirection(Mathf.Sign(playerMovement.transform.localScale.x) * transform.localScale.x);
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
