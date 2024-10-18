using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("����������� ����� ����� ����������")]
    [SerializeField] public float attackCooldown;
    [Header("����")]
    [SerializeField] private GameObject[] bullets;
    [Header("���� ��������")]
    [SerializeField] private AudioClip shotSound;

    private PlayerMovement playerMovement;
    private Transform firePoint;

    // ������� ������� �� ����������� ���������� ��� ���
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
            // ������� ���� ���� ��� ���� �����������
            if (Input.GetMouseButton(0) &&
                cooldownTimer > attackCooldown &&
                playerMovement.canAttack())
                Attack();

            // ������ ������� ��� ���������� ��������
            cooldownTimer += Time.deltaTime;
        }
    }

    private void Attack() // ���������� ��������
    {
        // ��������������� ����� ��������
        SoundManager.instance.PlaySound(shotSound);

        // ��������� �������� ����� ��������
        cooldownTimer = 0;

        // ����������� ������� ���� �� ����� ��������
        bullets[FindBullet()].transform.position = firePoint.position;

        // ��������� ������� ����
        bullets[FindBullet()].GetComponent<Projectile>().
            SetDirection(Mathf.Sign(playerMovement.transform.localScale.x) * transform.localScale.x);
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
