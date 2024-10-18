using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrap : MonoBehaviour
{
    [Header("Задержка между выстрелами")]
    [SerializeField] private float attackCooldown;
    [Header("Точка появления пули")]
    [SerializeField] private Transform firePoint;
    [Header("Массив пуль")]
    [SerializeField] private GameObject[] bullets;
    [Header("Звук выстрела")]
    [SerializeField] private AudioClip shot;

    // Таймер, запускаемый после выстрела
    private float cooldownTimer;

    private void Update()
    {
        // Работа таймера
        cooldownTimer += Time.deltaTime;

        // Выстрел, если есть возможность
        if (cooldownTimer > attackCooldown) 
            Attack();
    }

    // Функция выстрела
    private void Attack()
    {
        // Обнуление таймера
        cooldownTimer = 0;
        // Воспроизведение звука выстрела
        SoundManager.instance.PlaySound(shot);
        // Перемещение объекта пули на место выстрела
        bullets[FindBullet()].transform.position = firePoint.position;
        // Активация объекта пули
        bullets[FindBullet()].GetComponent<EnemyProjectile>().ActivateProjectile();
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
