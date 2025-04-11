using UnityEngine;

public class BossController : MonoBehaviour
{
    [Space(10)]
    // Thành phần Rigidbody2D dùng để xử lý vật lý cho Boss
    public Rigidbody2D rigidbody2d;
    // Thành phần SpriteRenderer để quản lý hình ảnh của Boss
    public SpriteRenderer spriteRenderer;
    // Hiệu ứng khi Boss bị tiêu diệt
    public GameObject deathEffect;
    // Mục tiêu mà Boss sẽ đuổi theo hoặc tấn công (người chơi)
    private Transform target;

    [Space(10)]
    // Các thuộc tính của Boss: máu, sát thương và tốc độ di chuyển
    public float health = 10000;
    public float damage = 100;
    public float moveSpeed = 2f;

    [Space(10)]
    // Biến thời gian dùng để xử lý knockback (văng ra) và thời gian chờ khi bị trúng đòn
    public float knockBackTime = 0.5f;
    public float hitWaitTime = 0.5f;
    private float knockBackCounter;
    private float hitCounter;

    [Space(10)]
    // Thành phần Animator dùng để điều khiển hoạt ảnh của Boss
    private Animator animator;
    // Bộ đếm thời gian để kiểm soát các giai đoạn hành vi của Boss
    public float timer;
    public float secondTimer;
    public float thirdTimer;
    // Vị trí cuối cùng của người chơi, được dùng để điều khiển AI của Boss
    private Vector3 playerLastPosition;

    void Start()
    {
        // Khởi tạo các thành phần và tìm người chơi ngay khi trò chơi bắt đầu
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        target = FindObjectOfType<PlayerHealthController>().transform;
        animator = GetComponentInChildren<Animator>();
        // Ghi nhớ vị trí ban đầu của người chơi
        playerLastPosition = target.position;
        // Thiết lập giá trị ban đầu cho các bộ đếm thời gian
        timer = 5f;
        secondTimer = 1f;
        thirdTimer = 2f;
    }

    void Update()
    {
        // Kiểm tra và xử lý hiệu ứng knockback
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;

            // Đảo ngược và tăng tốc độ di chuyển trong thời gian bị knockback
            if (moveSpeed > 0)
            {
                moveSpeed = -moveSpeed * 2f;
            }

            // Khi knockback kết thúc, đặt lại tốc độ di chuyển về bình thường
            if (knockBackCounter <= 0)
            {
                moveSpeed = Mathf.Abs(moveSpeed * 0.5f);
            }
        }

        // Gọi phương thức kiểm soát hành vi của Boss
        BehaviorBoss();
        // Đảm bảo Boss luôn hướng về phía người chơi
        FlipTowardsPlayer();

        // Xử lý thời gian chờ sau khi bị trúng đòn
        if (hitCounter > 0f)
        {
            hitCounter -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        // Giảm máu của Boss khi nhận sát thương
        health -= damageToTake;

        // Kiểm tra nếu máu Boss giảm về 0 thì hủy đối tượng và tạo hiệu ứng chết
        if (health <= 0)
        {
            Destroy(gameObject);
            UIController.instance.IncrementEnemiesDefeated();
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            CameraShake.instance.ShakeIt(1f, 0.2f);
        }

        // Hiển thị hiệu ứng sát thương
        DamageController.instance.SpawnDamage(damageToTake, transform.position);
    }

    // Phương thức nhận sát thương có thêm hiệu ứng knockback (văng ra)
    public void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        // Nhận sát thương
        TakeDamage(damageToTake);

        // Nếu có knockback thì áp dụng thời gian knockback và thời gian chờ
        if (shouldKnockBack)
        {
            knockBackCounter = knockBackTime;
            hitCounter = hitWaitTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi va chạm với người chơi, Boss sẽ gây sát thương cho người chơi
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.TakeDamage(damage);
            hitCounter = hitWaitTime;
        }
    }

    private void FlipTowardsPlayer()
    {
        // Đảo ngược hướng của Boss để luôn đối mặt với người chơi
        if (transform.position.x < target.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > target.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void StopBoss()
    {
        // Ngừng di chuyển Boss bằng cách đặt vận tốc về 0
        rigidbody2d.velocity = Vector2.zero;
    }

    private void BehaviorBoss()
    {
        // Kiểm soát hành vi của Boss theo trình tự dựa trên thời gian
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // Khi bộ đếm thời gian chính kết thúc, Boss dừng lại và đổi trạng thái hoạt ảnh
            StopBoss();
            animator.SetBool("isRunning", false);
            secondTimer -= Time.deltaTime;
            if (secondTimer <= 0)
            {
                // Khi bộ đếm thứ hai kết thúc, Boss tiếp tục di chuyển và tăng tốc
                animator.SetBool("isRunning", true);
                // Xác định hướng di chuyển đến vị trí cuối cùng của người chơi
                Vector3 playerPosition = playerLastPosition;
                // Tăng tốc độ tạm thời cho một đợt lao tới
                moveSpeed = 8f;
                // Xác định hướng di chuyển
                Vector3 direction = playerPosition - transform.position;
                // Chuẩn hóa hướng để đảm bảo tốc độ di chuyển ổn định
                direction.Normalize();
                // Di chuyển Boss theo hướng đã xác định
                transform.position += direction * moveSpeed * Time.deltaTime;
                thirdTimer -= Time.deltaTime;
                if (thirdTimer <= 0)
                {
                    // Đặt lại tốc độ và bộ đếm thời gian cho chu kỳ tiếp theo
                    moveSpeed = 2f;
                    timer = 5f;
                    secondTimer = 1f;
                    thirdTimer = 2f;
                }
            }
        }
        else
        {
            // Khi chưa đến thời gian chuyển trạng thái, Boss di chuyển bình thường về phía người chơi
            if (playerLastPosition != target.position)
            {
                playerLastPosition = target.position;
            }
            rigidbody2d.velocity = (target.position - transform.position).normalized * moveSpeed;
        }
    }
}
