using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Space(10)]
    // Thành phần Rigidbody2D để xử lý vật lý
    public Rigidbody2D rigidbody2d;
    // Thành phần SpriteRenderer để hiển thị hình ảnh của kẻ địch
    public SpriteRenderer spriteRenderer;

    [Space(10)]
    // Thuộc tính của kẻ địch: máu, tốc độ di chuyển và sát thương
    public float health = 5f;
    public float moveSpeed = 2f;
    public float damage = 5f;

    [Space(10)]
    // Thời gian chờ trước khi kẻ địch có thể tấn công lại
    public float hitWaitTime = 0.5f;
    // Thời gian kẻ địch bị đẩy lùi sau khi nhận sát thương
    public float knockBackTime = .5f;

    [Space(10)]
    // Lượng kinh nghiệm rơi ra khi kẻ địch bị tiêu diệt
    public int experienceToGive = 1;
    // Xác suất rơi ra một đồng xu
    public float coinDropRate = 0.5f;
    // Xác suất rơi ra một rương kho báu
    private float chestDropRate = 0.001f;

    // Bộ đếm thời gian để kiểm soát thời gian giữa các lần tấn công
    private float hitCounter;
    // Bộ đếm thời gian để kiểm soát hiệu ứng đẩy lùi
    private float knockBackCounter;
    // Đối tượng mục tiêu của kẻ địch (người chơi)
    private Transform target;
    // Cờ kiểm tra nếu kẻ địch đã bị tiêu diệt
    private bool isDefeated = false;

    void Start()
    {
        // Tìm người chơi trong scene và gán làm mục tiêu
        target = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        // Xử lý hiệu ứng đẩy lùi
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;

            // Đảo ngược và tăng tốc độ di chuyển trong thời gian đẩy lùi
            if (moveSpeed > 0)
            {
                moveSpeed = -moveSpeed * 2f;
            }

            // Khôi phục tốc độ di chuyển sau khi hết hiệu ứng đẩy lùi
            if (knockBackCounter <= 0)
            {
                moveSpeed = Mathf.Abs(moveSpeed * 0.5f);
            }
        }

        // Di chuyển kẻ địch về phía người chơi
        rigidbody2d.velocity = (target.position - transform.position).normalized * moveSpeed;

        // Cập nhật hướng quay mặt của sprite
        FlipTowardsPlayer();

        // Giảm thời gian chờ sau mỗi lần tấn công
        if (hitCounter > 0f)
        {
            hitCounter -= Time.deltaTime;
        }
    }

    private void FlipTowardsPlayer()
    {
        // Đảo chiều sprite dựa vào vị trí của người chơi
        if (transform.position.x < target.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > target.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu va chạm với người chơi và đã hết thời gian chờ, gây sát thương
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);

            // Đặt lại thời gian chờ trước khi có thể tấn công lại
            hitCounter = hitWaitTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        // Nếu kẻ địch đã bị tiêu diệt, không làm gì cả
        if (isDefeated)
            return;

        // Giảm máu kẻ địch
        health -= damageToTake;

        // Kiểm tra nếu máu về 0 và xử lý việc tiêu diệt kẻ địch
        if (health <= 0)
        {
            isDefeated = true;
            Destroy(gameObject);

            // Cập nhật UI số lượng kẻ địch đã bị tiêu diệt
            UIController.instance.IncrementEnemiesDefeated();

            // Xác suất rơi ra vật phẩm khi bị tiêu diệt
            float random = Random.value;

            if (random <= coinDropRate && random > chestDropRate)
            {
                LevelController.instance.SpawnExp(transform.position, experienceToGive);
            }
            else if (random <= chestDropRate)
            {
                LevelController.instance.SpawnChest(transform.position);
            }
        }

        // Hiển thị hiệu ứng sát thương
        DamageController.instance.SpawnDamage(damageToTake, transform.position);
    }

    public void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        // Gây sát thương cho kẻ địch
        TakeDamage(damageToTake);

        // Nếu có hiệu ứng đẩy lùi, kích hoạt hiệu ứng
        if (shouldKnockBack == true)
        {
            knockBackCounter = knockBackTime;
        }
    }
}
