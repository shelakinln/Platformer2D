using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{
    [Header("������� ���������� ��������� � �������")]
    [SerializeField] GameObject[] enemies;
    [Header("��������� �������")]
    [SerializeField] private Transform startPoint;
    [Header("��������� �������")]
    [SerializeField] private Transform endPoint;

    // ����������� ��������� ������ ���������� ��������
    private Vector3[] initialPosition;

    private void Awake()
    {
        // ������������� ������� ����������� �������
        initialPosition = new Vector3[enemies.Length];

        // ��������� ������� - �� ������� ���������� � Editor
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                initialPosition[i] = enemies[i].transform.position;
            }
        }
    }

    // ����������� / ��������� �������
    // ��� ������ / ����� � �� �������
    public void ActivateRoom(bool _status)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];
            }
        }
    }

    public void PlayerTransition(Collider2D player, bool next)
    {
        if (next)
        {
            player.transform.position = new Vector3(startPoint.transform.position.x, 
                player.transform.position.y, player.transform.position.z);
        }
        else
        {
            player.transform.position = new Vector3(endPoint.transform.position.x, 
                player.transform.position.y, player.transform.position.z);
        }
    }
}
