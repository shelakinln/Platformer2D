using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Левая точка маршрута")]
    [SerializeField] private Transform leftEdge;
    [Header("Правая точка маршрута")]
    [SerializeField] private Transform rightEdge;
    [Header("Объект противника в патруле")]
    [SerializeField] private Transform enemy;
    [Header("Скорость перемещения противника")]
    [SerializeField] private float speed;
    [Header("Аниматор противника в патруле")]
    [SerializeField] private Animator animator;
    [Header("Время простоя на крайней точке")]
    [SerializeField] private float idleDuration;

    // Движется ли противник налево
    private bool movingLeft;
    // Размеры объекта противника
    private Vector3 initScale;
    // Счётчик после остановки
    private float idleTimer;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        // Движение влево / вправо
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

    // Отключение анимации движения
    private void OnDisable()
    {
        animator.SetBool("isMoving", false);
    }

    // Функция, реализующая смену направления
    // по достижению крайней точки патруля
    private void DirectionChange()
    {
        // Отключение анимации движения противника
        animator.SetBool("isMoving", false);
        // Счётчик простоя
        idleTimer += Time.deltaTime;
        // Разворот персонажа
        if(idleTimer > idleDuration) 
            movingLeft = !movingLeft;
    }

    // Функция, реализующая движение противника
    // в сторону крайней точки
    private void MoveInDirection(int _direction)
    {
        // Обнуление счётчика простоя
        idleTimer = 0;
        // Воспроизведение анимации движения
        animator.SetBool("isMoving", true);
        // Разворот спрайта противника
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        // Перемещение объекта противника по маршруту
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
