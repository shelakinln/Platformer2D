using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySideways : MonoBehaviour
{
    [Header("Урон ловушки")]
    [SerializeField] private float damage;
    [Header("Скорость перемещения ловушки")]
    [SerializeField] private float speed;
    [Header("Расстояния от ловушки до одной из крайних точек")]
    [SerializeField] private float movementDistance;

    // Двигается ли ловушка влево
    private bool movingLeft;
    // Левая граница движения ловушки
    private float leftEdge;
    // Правая граница движения ловушки
    private float rightEdge;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        // Движение ловушки из стороны в сторону
        if(movingLeft)
        {
            if(transform.position.x > leftEdge)
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            else 
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            else 
                movingLeft = true;
        }
        
    }

    // Урон по игроку при соприкосновении с ним
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(false, damage);
        }
    }
}
