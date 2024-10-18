using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("����� ��������� �������")]
    [SerializeField] private float activationDelay;
    [Header("����� ������ �������")]
    [SerializeField] private float activeTime;
    [Header("���� �������")]
    [SerializeField] private float damage;
    [Header("���� ������ �������� �������")]
    [SerializeField] private AudioClip trapSound;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Health playerHealth;

    // �������������� �� �������
    private bool isTriggered;
    // ������� �� �������
    private bool isActive;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        isTriggered = false;
        isActive = false;
        spriteRenderer.color = Color.white;
    }
    private void Update()
    {
        // ���� �� ������ ���� �� �����������
        // � �������� �������
        if (playerHealth != null && isActive)
        {
            playerHealth.TakeDamage(false,damage);
        }
    }

    // ��� ��������������� ������ � ��������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // ��������� ���������� �������� ������
            playerHealth = collision.GetComponent<Health>();

            if(!isTriggered) 
            {
                // ��������� �������
                StartCoroutine(ActivateFiretrap());
            }
            if (isActive)
            {
                // ���� �� ������
                collision.GetComponent<Health>().TakeDamage(false, damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �������� ���������� �������� ������
        // �� ������ �������
        if(collision.tag == "Player")
        {
            playerHealth = null;
        }
    }

    // �������, ����������� ��������� �������
    private IEnumerator ActivateFiretrap()
    {
        // ��������� - ��������������
        isTriggered = true;
        // ����� ����� �������
        spriteRenderer.color = Color.red;
        // �������� ������� ���������
        yield return new WaitForSeconds(activationDelay);
        // ��������������� �����  �������� �������
        SoundManager.instance.PlaySound(trapSound);
        // ����� ����� ������� �� �����������
        spriteRenderer.color = Color.white;
        // ������� �������
        isActive = true;
        // ��������������� �������� ���������� �������
        animator.SetBool("isActivated", true);
        // �������� ������� ������
        yield return new WaitForSeconds(activeTime);
        // ������� ���������
        isActive = false;
        // ������� �� ������������
        isTriggered = false;
        // ����������� ��������������� ��������
        animator.SetBool("isActivated", false);
    }
}
