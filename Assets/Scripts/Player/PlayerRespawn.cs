using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("���� ��������� ����� ����������")]
    [SerializeField] private AudioClip checkpointSound;
    [Header("�������")]
    [SerializeField] private Room[] rooms;

    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {

        // �������� �� ���� �� ���� �������������� ��������
        if(currentCheckpoint == null || playerHealth.currentLives <= 0)
        {
            // ����� ���� � ������, ���� �� ���� �� ������ ���������
            uiManager.GameOver();
            return;
        }

        // ����������� ������� ������ �� ����� ���������� ���������
        transform.position = currentCheckpoint.position;

        // ����������� ���������
        playerHealth.Respawn();

        if(Math.Round(Camera.main.GetComponent<CameraController>().transform.position.x) != currentCheckpoint.parent.position.x)
        {
            for(int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].transform.position.x == Math.Round(Camera.main.GetComponent<CameraController>().transform.position.x))
                {
                    rooms[i].ActivateRoom(false);
                }
            }
        }
        for (int i = 0; i < rooms.Length; i++)
        {
            if (currentCheckpoint.parent.position.x > rooms[i].transform.position.x - 18 && currentCheckpoint.parent.position.x < rooms[i].transform.position.x + 18)
            {
                rooms[i].ActivateRoom(true);
            }
        }

        // ����������� ������ � ������� � ��������� ����������
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������� ��������� ��� �����������
        // �������� ������ � ����������������� ���������
        if(collision.transform.tag == "Checkpoint")
        {
            // ����������� ����� ���������� ���������
            currentCheckpoint = collision.transform;

            // ��������������� ����� ��������� ���������
            SoundManager.instance.PlaySound(checkpointSound);

            // ����������� �������� ���������
            collision.GetComponent<Collider2D>().enabled = false;

            //��������������� �������� ��������� ���������
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
