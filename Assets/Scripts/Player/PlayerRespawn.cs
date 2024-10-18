using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Звук активации точки сохранения")]
    [SerializeField] private AudioClip checkpointSound;
    [Header("Комнаты")]
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

        // Проверка на хотя бы один активированный чекпоинт
        if(currentCheckpoint == null || playerHealth.currentLives <= 0)
        {
            // Конец игры в случае, если не было ни одного чекпоинта
            uiManager.GameOver();
            return;
        }

        // Перемещение объекта игрока на место последнего чекпоинта
        transform.position = currentCheckpoint.position;

        // Возрождение персонажа
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

        // Перемещение камеры в комнату с последним чекпоинтом
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Активация чекпоинта при перемечении
        // коллизии игрока и неактивированного чекпоинта
        if(collision.transform.tag == "Checkpoint")
        {
            // Запоминание места последнего чекпоинта
            currentCheckpoint = collision.transform;

            // Воспроизведение звука активации чекпоинта
            SoundManager.instance.PlaySound(checkpointSound);

            // Деактивация коллизии чекпоинта
            collision.GetComponent<Collider2D>().enabled = false;

            //Воспроизведение анимации активации чекпоинта
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
