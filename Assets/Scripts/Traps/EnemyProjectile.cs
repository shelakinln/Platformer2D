using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [Header("�������� ����������� ������������")]
    [SerializeField] private float speed;
    [Header("����� ����� ������������")]
    [SerializeField] private float resetTime;

    private Animator animator;
    private BoxCollider2D boxCollider;

    // ����� �� ������ � ������
    private bool hit;
    // ������� ������� ������������� �������
    private float lifeTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        // �������� ������� �� �����������
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        // ������� �������������
        lifeTime += Time.deltaTime;

        // ����������� ������� �� ��������� ��������
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

    // ��������� �������� ������� � �������� ������� �������
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag != "Bullet" && collision.transform.tag != "Enemy")
        {
            // ���������
            hit = true;

            // ���� �� ������
            base.OnTriggerEnter2D(collision);

            // ���������� �������� �������
            boxCollider.enabled = false;

            // ��������������� �������� ����������� �������
            if (animator != null)
                animator.SetTrigger("explode");
            else
                gameObject.SetActive(false);
        }
    }

    // ����������� ������� ����� �������� ��� �����������
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
