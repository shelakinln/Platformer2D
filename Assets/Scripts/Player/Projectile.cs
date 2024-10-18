using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("���� ������������")]
    [SerializeField] private float damage;
    [Header("�������� ����� ����")]
    [SerializeField] private float speed;
    [Header("����� �� ����������� ����")]
    [SerializeField] private float lifetime;

    private Animator animator;
    private BoxCollider2D boxCollider;

    // ������ ��������� ���� � �����-���� ������
    private bool hit;
    // ����������� ������� ����
    private float direction;
    // ����� ������������� ����
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

        // ����������� ������������ � ������������
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        // ����������� ������������ �� ��������� ����� ��� �����
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
            // ����������� ������������ ��� ������������ � ������ ��������
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger("explode");
        }

        if (collision.tag == "Enemy") // ��������� �� ����������
            collision.GetComponent<Health>().TakeDamage(false, damage);
    }

    public void SetDirection(float _direction) 
    {
        // ��������� ����
        lifetimeCooldown = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        // ����������� ����������� ������� ������������
        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction) 
            localScaleX = -localScaleX;
        transform.localScale = new 
            Vector3(localScaleX, transform.localScale.y, 
                    transform.localScale.z);
    }
    private void Deactivate()
    {
        // ������� ����������� ����, ������� ������������
        // ����� ��������������� �������� � ������
        gameObject.SetActive(false);
    }
}
