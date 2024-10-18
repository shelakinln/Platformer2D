using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Урон проджектайла")]
    [SerializeField] private float damage;
    [Header("Скорость полёта пули")]
    [SerializeField] private float speed;
    [Header("Время до деактивации пули")]
    [SerializeField] private float lifetime;

    private Animator animator;
    private BoxCollider2D boxCollider;

    // Статус попадания пули в какой-либо объект
    private bool hit;
    // Направление спрайта пули
    private float direction;
    // Время существования пули
    private float lifetimeCooldown;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) 
            return;

        // Перемещение проджектайла в пространстве
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // Деактивация проджектайла по истечении срока его жизни
        lifetimeCooldown += Time.deltaTime;
        if(lifetimeCooldown > lifetime) 
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Checkpoint" && 
            collision.transform.tag != "Level Trigger" &&
            collision.transform.tag != "Bullet")
        {
            // Уничтожение проджектайла при столкновении с другим объектом
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger("explode");
        }

        if (collision.tag == "Enemy") // Попадание по противнику
            collision.GetComponent<Health>().TakeDamage(false, damage);
    }

    public void SetDirection(float _direction) 
    {
        // Активация пули
        lifetimeCooldown = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        // Определение направления спрайта проджектайла
        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction) 
            localScaleX = -localScaleX;
        transform.localScale = new 
            Vector3(localScaleX, transform.localScale.y, 
                    transform.localScale.z);
    }
    private void Deactivate()
    {
        // Функция деактивации пули, которая используется
        // после воспроизведения анимации её взрыва
        gameObject.SetActive(false);
    }
}
