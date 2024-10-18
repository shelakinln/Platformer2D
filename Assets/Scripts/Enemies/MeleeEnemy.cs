using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
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
    [SerializeField] private AudioClip attackSound;

    // ������, ����������� ����� �����
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
        // �������
        cooldownTimer += Time.deltaTime;

        // �����, ���� ���� �����������
        if (PlayerInSight())
        {
            if(cooldownTimer > attackCooldown && 
                playerHealth.currentHealth > 0)
            {
                // ��������� �������
                cooldownTimer = 0;
                // ������ �������� �����
                animator.SetTrigger("meleeAttack");
                // ��������������� ����� �����
                SoundManager.instance.PlaySound(attackSound);
            }
        }
        if(patrol != null) 
            patrol.enabled = !PlayerInSight();
    }

    // �������, ������������ ���� �� ����� � ������� �����
    private bool PlayerInSight()
    {
        // ���, ����������� ���� �� ����� ����������� �����
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + 
                transform.right * range * transform.localScale.x * 
                colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, 
            boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
            0, Vector2.left, 0, playerLayer);

        // ��������� ���������� �������� ������
        if(hit.collider != null) 
            playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
    }

    // �������, �������������� � ��������� ������ �����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * 
                transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, 
                boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // ����� ������ ����� �������� �����
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(false, damage);
        }
    }
}
