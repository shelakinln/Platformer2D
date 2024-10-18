using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrap : MonoBehaviour
{
    [Header("�������� ����� ����������")]
    [SerializeField] private float attackCooldown;
    [Header("����� ��������� ����")]
    [SerializeField] private Transform firePoint;
    [Header("������ ����")]
    [SerializeField] private GameObject[] bullets;
    [Header("���� ��������")]
    [SerializeField] private AudioClip shot;

    // ������, ����������� ����� ��������
    private float cooldownTimer;

    private void Update()
    {
        // ������ �������
        cooldownTimer += Time.deltaTime;

        // �������, ���� ���� �����������
        if (cooldownTimer > attackCooldown) 
            Attack();
    }

    // ������� ��������
    private void Attack()
    {
        // ��������� �������
        cooldownTimer = 0;
        // ��������������� ����� ��������
        SoundManager.instance.PlaySound(shot);
        // ����������� ������� ���� �� ����� ��������
        bullets[FindBullet()].transform.position = firePoint.position;
        // ��������� ������� ����
        bullets[FindBullet()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    // �������, ������������ ������ �������
    // ����������� � �������� ������� ����
    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy) return i;
        }
        return 0;
    }
}
