using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBox : EnemyDamage
{
    [Header("�������� �����������")]
    [SerializeField] private float speed;
    [Header("��������� ��������� �������")]
    [SerializeField] private float range;
    [Header("�������� ����� ��������� �������� ������")]
    [SerializeField] private float checkDelay;
    [Header("���� ������")]
    [SerializeField] private LayerMask playerLayer;
    [Header("���� ��������� �� ������")]
    [SerializeField] private AudioClip impact;

    // � �������� ����� �� �������
    private bool isAttacking;
    // ������ ����� �������� ������
    private float checkTimer;

    // ����� ���������� �������
    private Vector3 destination;
    private Vector3[] directions = new Vector3[4];

    private void OnEnable()
    {
        // � ������ ��������� ������� ����� ���������
        Stop();
    }
    private void Update()
    {
        if (isAttacking)
        {
            // ���� ������� � ��������� ��������� - ��� 
            // ������������ � ������� ������
            transform.Translate(destination * Time.deltaTime * speed);
        }
        else
        {
            // ����� ���������� ������� ����� ������
            if(checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
            // ������ ����� ����������
            checkTimer += Time.deltaTime;
        }
    }

    private void CheckForPlayer()
    {
        // ������ ����������� � ������ �������
        // ������ �� �������� ��������� �������
        CalculateDirections();

        // ����� ������ �� ������� �� ������ �����������
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            // ����� ������ ���� ���� �����������
            if(hit.collider != null && !isAttacking)
            {
                // ������� � ��������� ���������
                isAttacking = true;
                // ���������� - �������, � ������� ���������
                // ����� � ������ ��������
                destination = directions[i];
                // ��������� ������� ������
                checkTimer = 0;
            }
        }
    }

    // �������, �������������� ����������� �������
    // � ������ �� ������ ������ � �������� � �������
    private void CalculateDirections()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    // �������, ����������� ��������� �������
    private void Stop()
    {
        // ���������� �������� - ������� �������
        destination = transform.position;
        // ������� �� � ��������� ���������
        isAttacking = false;
    }

    // ��������� �� ������
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������������� ����� ���������
        SoundManager.instance.PlaySound(impact);
        // ���� �� ������
        base.OnTriggerEnter2D(collision);
        // ��������� �������
        Stop();
    }
}
