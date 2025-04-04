using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    [Space(10)]
    // Giá trị kinh nghiệm mà vật phẩm này cung cấp cho người chơi
    public int experienValue;
    // Tốc độ di chuyển của vật phẩm về phía người chơi
    public float moveSpeed;
    // Thời gian giữa các lần kiểm tra khoảng cách với người chơi
    public float timeBetweenChecks = .2f;
    // Thời gian tồn tại trước khi vật phẩm biến mất nếu không được thu thập
    public float lifeDuration = 60f;

    // Tham chiếu đến script PlayerController
    private PlayerController player;
    // Cờ đánh dấu xem vật phẩm có đang di chuyển về phía người chơi không
    private bool movingToPlayer;
    // Bộ đếm thời gian giữa các lần kiểm tra khoảng cách
    private float checkCounter;
    // Bộ đếm thời gian tồn tại của vật phẩm
    private float lifeTimer;

    void Start()
    {
        // Gán tham chiếu đến người chơi
        player = PlayerHealthController.instance.GetComponent<PlayerController>();
        // Khởi tạo bộ đếm thời gian tồn tại
        lifeTimer = lifeDuration;
    }

    void Update()
    {
        // Giảm thời gian tồn tại theo thời gian thực
        lifeTimer -= Time.deltaTime;

        // Nếu hết thời gian, vật phẩm sẽ bị hủy
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }

        // Nếu vật phẩm đang di chuyển về phía người chơi
        if (movingToPlayer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Giảm bộ đếm thời gian kiểm tra khoảng cách
            checkCounter -= Time.deltaTime;
            if (checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;

                // Kiểm tra xem người chơi có nằm trong phạm vi thu thập không
                if (Vector3.Distance(transform.position, player.transform.position) < player.pickupRange)
                {
                    // Bắt đầu di chuyển về phía người chơi và tăng tốc độ theo tốc độ của người chơi
                    movingToPlayer = true;
                    moveSpeed += player.speed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu vật phẩm chạm vào người chơi
        if (collision.tag == "Player")
        {
            // Phát âm thanh khi nhặt vật phẩm
            SFXManager.instance.PlaySFXPitched(1);

            // Cộng kinh nghiệm cho người chơi
            LevelController.instance.GetExp(experienValue);

            // Hủy vật phẩm
            Destroy(gameObject);

            // Cập nhật số lượng vật phẩm đã thu thập
            UIController.instance.IncrementCoinsCollected();
        }
    }
}
