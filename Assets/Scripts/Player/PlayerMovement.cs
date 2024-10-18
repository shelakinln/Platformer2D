using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Оружия")]
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject rifle;
    [Header("Звук подбора")]
    [SerializeField] private AudioClip collectSound;
    [Header("Скорость передвижения")]
    [SerializeField] private float speed;
    [Header("Сила прыжка")]
    [SerializeField] private float jumpPower;
    [Header("Поверхности")]
    [SerializeField] private LayerMask groundLayer;
    [Header("Звук прыжка")]
    [SerializeField] private AudioClip jumpSound;
    [Header("Время для буфферизации ввода прыжка")]
    [SerializeField] private float coyoteTime;
    [Header("Количество дополнительных прыжков")]
    [SerializeField] private int extraJumps;

    private Animator animator;
    private Rigidbody2D body;
    private Health playerHealth;

    private float scale;
    public bool isBootsReceiced;
    [Header("Активатор дробовика")]
    [SerializeField] public bool isShotGunReceiced;
    [Header("Активатор винтовки")]
    [SerializeField] public bool isRifleReceiced;
    // Ввод перемещения по горизонтали
    private float horizontalInput;
    // Таймер времени буферизованного прыжка
    private float coyoteCounter;
    // Количество сделанных прыжков
    private int jumpCounter;

    public Vector2 boxSize;
    public float castDistanceX;
    public float castDistanceY;

    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<Health>();
        pistol.SetActive(true);
        shotgun.SetActive(false);
        rifle.SetActive(false);
        scale = transform.localScale.x;
        isBootsReceiced = extraJumps > 0;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (!pistol.activeInHierarchy)
                {
                    pistol.GetComponent<PlayerAttack>().cooldownTimer = pistol.GetComponent<PlayerAttack>().attackCooldown;
                    pistol.SetActive(true);
                    shotgun.SetActive(false);
                    rifle.SetActive(false);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (!shotgun.activeInHierarchy && isShotGunReceiced)
                {
                    shotgun.GetComponent<PlayerAttack>().cooldownTimer = shotgun.GetComponent<PlayerAttack>().attackCooldown;
                    shotgun.SetActive(true);
                    pistol.SetActive(false);
                    rifle.SetActive(false);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (!rifle.activeInHierarchy && isRifleReceiced)
                {
                    rifle.GetComponent<PlayerAttack>().cooldownTimer = rifle.GetComponent<PlayerAttack>().attackCooldown;
                    rifle.SetActive(true);
                    pistol.SetActive(false);
                    shotgun.SetActive(false);
                }
            }
            // Ввод перемещения по горизонтали
            horizontalInput = Input.GetAxis("Horizontal");
            Move(horizontalInput);

            // Прыжок
            if (Input.GetKeyDown(KeyCode.Space)) 
                Jump();

            //Ускорение падения
            if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

            if (isGrounded())
            {
                // Установка всех счётчиков на начальное значение при приземлении
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            }
            else //Отсчёт счётчика буферизованного прыжка
            {
                coyoteCounter -= Time.deltaTime;
            }
        }
    }


    private void Move(float input)
    {
        // Перемещение персонажа по горизонтали
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Разворот спрайта персонажа
        if (input > 0) transform.localScale = new Vector3(scale, scale, scale);
        else if (input < 0) transform.localScale = new Vector3(-scale, scale, scale);

        // Переключение анимаций
        animator.SetBool("isRunning", input != 0);
        animator.SetBool("isGrounded", isGrounded());
    }
    private void Jump() // Функция прыжка
    {
        // Прыжок не выполняется если нет для него возможностей
        if (coyoteCounter <= 0 && jumpCounter <= 0) return;

        // Воспроизведение звука прыжка
        SoundManager.instance.PlaySound(jumpSound);

        if (isGrounded()) // Реализация прыжка
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else
        {
            if (coyoteCounter > 0) // Буферизованный прыжок
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else if (jumpCounter > 0)
            {
                // Повторный прыжок если есть для него возможность
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                jumpCounter--;
            }
        }
        coyoteCounter = 0;
    }

    public bool isGrounded()
    {
        // Функция определяет стоит ли персонаж на земле
        return Physics2D.BoxCast(transform.position, boxSize, 0, 
                        -transform.up, castDistanceY, groundLayer); ;
    }

    public bool canAttack() 
    {
        // Функция определяет способен ли персонаж стрелять
        return horizontalInput == 0 && isGrounded();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Fall Trigger")
        {
            playerHealth.TakeDamage(true, playerHealth.currentHealth);
        }
        if (collision.transform.tag == "Shotgun Drop")
        {
            isShotGunReceiced = true;
            shotgun.SetActive(true);
            pistol.SetActive(false);
            rifle.SetActive(false);
            shotgun.GetComponent<PlayerAttack>().cooldownTimer = shotgun.GetComponent<PlayerAttack>().attackCooldown;
            SoundManager.instance.PlaySound(collectSound);
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Rifle Drop")
        {
            isRifleReceiced = true;
            rifle.SetActive(true);
            shotgun.SetActive(false);
            pistol.SetActive(false);
            rifle.GetComponent<PlayerAttack>().cooldownTimer = rifle.GetComponent<PlayerAttack>().attackCooldown;
            SoundManager.instance.PlaySound(collectSound);
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Jump Boots")
        {
            isBootsReceiced = true;
            extraJumps++;
            jumpCounter = extraJumps;
            SoundManager.instance.PlaySound(collectSound);
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Heart Live")
        {
            playerHealth.currentLives++;
            jumpCounter = extraJumps;
            SoundManager.instance.PlaySound(collectSound);
            Destroy(collision.gameObject);
        }
    }
}
