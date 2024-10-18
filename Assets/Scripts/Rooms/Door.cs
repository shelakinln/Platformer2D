using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Положение нынешней комнаты")]
    [SerializeField] private Transform previousRoom;
    [Header("Положение следующей комнаты")]
    [SerializeField] private Transform nextRoom;
    [Header("Объект игровой камеры")]
    [SerializeField] private CameraController cam;

    private bool next = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Перемещает камеру на ту комнату в которую вошёл игрок
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
