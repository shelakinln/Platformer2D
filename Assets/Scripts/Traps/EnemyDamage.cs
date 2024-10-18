using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Урон")]
    [SerializeField] protected float damage;

    // Функция, реализующая урон по игроку
    // при прикосновении к ловушке
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(false, damage);
        }
    }
}
