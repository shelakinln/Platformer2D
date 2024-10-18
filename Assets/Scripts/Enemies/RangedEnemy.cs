using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("�������� ����� �������")]
    [SerializeField] private float attackCooldown;
    [Header("��������� ��������� ����������")]
    [SerializeField] private float range;
    [Header("���������� �� ���������� �� ��������� ����� ������� �����")]
    [SerializeField] private float colliderDistance;
    [Header("����, ��������� �����������")]
    [SerializeField] private int damage;
    [Header("��������� ���������")]
    [SerializeField] private BoxCollider2D boxCollider;
    [Header("���� ������")]
    [SerializeField] private LayerMask playerLayer;
    [Header("���� ����� ����������")]
    [SerializeField] private AudioClip shotSound;
    [Header("����� �������� ����������")]
    [SerializeField] private Transform firepoint;
    [Header("������� ���� ����������")]
    [SerializeField] private GameObject[] bullets;

    // ������, ����������� ����� �����
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
        // �������
        cooldownTimer += Time.deltaTime;

        // �����, ���� ���� �����������
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

    // �������, ������������ ���� �� ����� � ������� �����
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

    // �������, �������������� � ��������� ������ �����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * 
                range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, 
            boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // ������� � ������ ����� �������� �����
    private void RangedAttack()
    {
        // ��������������� ����� ��������
        SoundManager.instance.PlaySound(shotSound);
        // ��������� �������
        cooldownTimer = 0;
        // ����������� ������� ���� �� ����� ��������
        bullets[FindBullet()].transform.position = firepoint.position;
        // ��������� ������� ����
        bullets[FindBullet()].GetComponent<EnemyProjectile>().
            ActivateEnemyProjectile(damage);
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
