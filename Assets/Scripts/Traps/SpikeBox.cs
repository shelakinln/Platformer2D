using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBox : EnemyDamage
{
    [Header("Скорость перемещения")]
    [SerializeField] private float speed;
    [Header("Дальность видимости ловушки")]
    [SerializeField] private float range;
    [Header("Задержка между попытками отыскать игрока")]
    [SerializeField] private float checkDelay;
    [Header("Слой игрока")]
    [SerializeField] private LayerMask playerLayer;
    [Header("Звук попадания по игроку")]
    [SerializeField] private AudioClip impact;

    // В процессе атаки ли ловушка
    private bool isAttacking;
    // Таймер между поисками игрока
    private float checkTimer;

    // Точка назначения ловушки
    private Vector3 destination;
    private Vector3[] directions = new Vector3[4];

    private void OnEnable()
    {
        // В момент активации ловушка стоит неактивна
        Stop();
    }
    private void Update()
    {
        if (isAttacking)
        {
            // Если ловушка в атакующем состоянии - она 
            // перемещается в сторону игрока
            transform.Translate(destination * Time.deltaTime * speed);
        }
        else
        {
            // Иначе происходит попытка найти игрока
            if(checkTimer > checkDelay)
            {
                CheckForPlayer();
            }
            // Таймер между проверками
            checkTimer += Time.deltaTime;
        }
    }

    private void CheckForPlayer()
    {
        // Расчёт направлений в каждую сторону
        // исходя из текущего положения ловушки
        CalculateDirections();

        // Поиск игрока по каждому из четырёх направлений
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            // Атака игрока если есть возможность
            if(hit.collider != null && !isAttacking)
            {
                // Ловушка в атакующем состоянии
                isAttacking = true;
                // Назначение - сторона, в которой находился
                // игрок в момент проверки
                destination = directions[i];
                // Обнуление таймера поиска
                checkTimer = 0;
            }
        }
    }

    // Функция, просчитывающая направления ловушки
    // в каждую из четырёх сторон с нынешней её позиции
    private void CalculateDirections()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    // Функция, реализующая остановку ловушки
    private void Stop()
    {
        // Назначение движения - текущая позиция
        destination = transform.position;
        // Ловушка не в атакующем состоянии
        isAttacking = false;
    }

    // Попадание по игроку
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        // Воспроизведение звука попадания
        SoundManager.instance.PlaySound(impact);
        // Урон по игроку
        base.OnTriggerEnter2D(collision);
        // Остановка ловушки
        Stop();
    }
}
