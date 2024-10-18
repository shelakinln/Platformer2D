using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Скорость перемещения камеры")]
    [SerializeField] private float speed;

    // Текущая позиция камеры
    private float currentPosX;
    // Текущая скорость камеры
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        currentPosX = transform.position.x;
    }
    private void Update()
    {
        // Перемещение камеры на положение следующей комнаты
        transform.position = Vector3.SmoothDamp(transform.position, 
            new Vector3(currentPosX, transform.position.y, transform.position.z), 
            ref velocity, speed);
    }

    // Запись координат следующей комнаты
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
