using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{
    [Header("Объекты враждебных сущностей в комнате")]
    [SerializeField] GameObject[] enemies;
    [Header("Стартовая позиция")]
    [SerializeField] private Transform startPoint;
    [Header("Финальная позиция")]
    [SerializeField] private Transform endPoint;

    // Изначальные положения каждой враждебной сущности
    private Vector3[] initialPosition;

    private void Awake()
    {
        // Инициализация массива изначальных позиций
        initialPosition = new Vector3[enemies.Length];

        // Начальные позиции - те которые выставлены в Editor
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                initialPosition[i] = enemies[i].transform.position;
            }
        }
    }

    // Деактивация / активация комнаты
    // при выходе / входе в неё игроком
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
