using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("���������� ������")]
    [SerializeField] private float startLives;
    [Header("��������� �������� ���������")]
    [SerializeField] private float startHealth;
    [Header("���������� ������ ������������")]
    [SerializeField] private float iFramesDuration;
    [Header("���������� ������� ������� ������")]
    [SerializeField] private int numberOfFlashes;
    [Header("������� ���������")]
    [SerializeField] private Behaviour[] components;
    [Header("���� �������")]
    [SerializeField] private AudioClip hurtSound;
    [Header("���� ������")]
    [SerializeField] private AudioClip deathSound;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float currentLives;
    // ������� �������� ���������
    public float currentHealth;
    // ��������� ���������
    private bool dead;
    // ������ ������������
    private bool isInvulnerable;

    private void Awake()
    {
        currentHealth = startHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLives = startLives;
    }

    public void TakeDamage(bool isFallTrigger, float _damage) // ������� ��������� �����
    {
        if (isInvulnerable && !isFallTrigger) // ���� �� �������� ���� �������� ��������
            return;

        // �������� ����� ��������
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startHealth);

        if (currentHealth > 0)
        {
            // �������� ���������� �� ������������
            StartCoroutine(Invunerabilty());
            // ��������������� ����� �������
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead) // ���� ������ ��������� ����
            { 
                // ����������� ���� �������� ��������
                foreach (Behaviour component in components) 
                    component.enabled = false;

                //boxCollider.enabled = false;
                gameObject.layer = 0;

                // ��������������� �������� ������ ���������
                animator.SetTrigger("dead");
                animator.SetBool("isGrounded", true);

                // ��������� - ����
                dead = true;
                currentLives--;
                // ��������������� ����� ������ ���������
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    private IEnumerator Invunerabilty() // ���������� ������������
    {
        //�������� ��������
        isInvulnerable = true;

        // �������� ������ � ����������� ���������� ���� �����
        Physics2D.IgnoreLayerCollision(8, 9, true);

        // ������� ������� ��������� ��� �����������
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new 
                WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new 
                WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        isInvulnerable = false;
    }

    private void Deactivate()
    {
        // ������� ����������� ���������, ������� ������������
        // ����� ��������������� �������� ��� ������
        gameObject.SetActive(false);
    }

    private void AddHealth(float _value)
    {
        // ���������� �������� ���������
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startHealth);
    }

    public void Respawn() // ������� ����������� �� ����� ����������
    {
        // ��������� - ���
        dead = false;

        // �������� ������������ � ���������� ��������
        AddHealth(startHealth);

        gameObject.layer = 8;

        // ��������� idle-��������
        animator.ResetTrigger("dead");
        animator.Play("Idle");

        // ������������ ����� �����������
        StartCoroutine(Invunerabilty());

        // ��������� ��������� �������� ���������
        foreach (Behaviour component in components) 
            component.enabled = true;
    }
}
