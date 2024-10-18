using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("����� ����� ��������")]
    [SerializeField] private Transform leftEdge;
    [Header("������ ����� ��������")]
    [SerializeField] private Transform rightEdge;
    [Header("������ ���������� � �������")]
    [SerializeField] private Transform enemy;
    [Header("�������� ����������� ����������")]
    [SerializeField] private float speed;
    [Header("�������� ���������� � �������")]
    [SerializeField] private Animator animator;
    [Header("����� ������� �� ������� �����")]
    [SerializeField] private float idleDuration;

    // �������� �� ��������� ������
    private bool movingLeft;
    // ������� ������� ����������
    private Vector3 initScale;
    // ������� ����� ���������
    private float idleTimer;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        // �������� ����� / ������
        if (movingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x) 
                MoveInDirection(-1);
            else 
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x) 
                MoveInDirection(1);
            else 
                DirectionChange();
        }
    }

    // ���������� �������� ��������
    private void OnDisable()
    {
        animator.SetBool("isMoving", false);
    }

    // �������, ����������� ����� �����������
    // �� ���������� ������� ����� �������
    private void DirectionChange()
    {
        // ���������� �������� �������� ����������
        animator.SetBool("isMoving", false);
        // ������� �������
        idleTimer += Time.deltaTime;
        // �������� ���������
        if(idleTimer > idleDuration) 
            movingLeft = !movingLeft;
    }

    // �������, ����������� �������� ����������
    // � ������� ������� �����
    private void MoveInDirection(int _direction)
    {
        // ��������� �������� �������
        idleTimer = 0;
        // ��������������� �������� ��������
        animator.SetBool("isMoving", true);
        // �������� ������� ����������
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        // ����������� ������� ���������� �� ��������
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
