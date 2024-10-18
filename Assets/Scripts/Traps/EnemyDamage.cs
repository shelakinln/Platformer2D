using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("����")]
    [SerializeField] protected float damage;

    // �������, ����������� ���� �� ������
    // ��� ������������� � �������
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(false, damage);
        }
    }
}
