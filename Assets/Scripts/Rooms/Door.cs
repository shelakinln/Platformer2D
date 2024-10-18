using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("��������� �������� �������")]
    [SerializeField] private Transform previousRoom;
    [Header("��������� ��������� �������")]
    [SerializeField] private Transform nextRoom;
    [Header("������ ������� ������")]
    [SerializeField] private CameraController cam;

    private bool next = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������� ������ �� �� ������� � ������� ����� �����
        if(collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
                nextRoom.GetComponent<Room>().PlayerTransition(collision, next);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().PlayerTransition(collision, !next);
            }
        }
    }
}
