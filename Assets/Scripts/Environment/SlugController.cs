using System.Collections;
using UnityEngine;

public class Slug : MonoBehaviour
{
    [Space(10)]
    // Tham chiếu đến thành phần SpriteRenderer.
    public SpriteRenderer spriteRenderer;

    [Space(10)]
    // Tốc độ di chuyển của Slug.
    public float movementSpeed = 0.1f;
    // Chiều rộng khu vực di chuyển.
    public float movementWidth = 4f;
    // Chiều cao khu vực di chuyển.
    public float movementHeight = 4f;

    // Vị trí ban đầu của Slug.
    private Vector2 initialPosition;
    // Cờ kiểm soát trạng thái di chuyển của Slug.
    private bool isMoving = true;

    private void Start()
    {
        // Lưu vị trí ban đầu.
        initialPosition = transform.position;
        // Bắt đầu di chuyển ngẫu nhiên.
        MoveRandomly();
    }

    private void Update()
    {
        // Kiểm tra xem hộp thoại có đang mở không, nếu có thì dừng Slug lại.
        if (DialogueManager.instance.isDialogueOpen)
        {
            // Dừng di chuyển của Slug.
            StopMoving();
        }
        else
        {
            // Nếu hộp thoại đóng và Slug đang dừng, tiếp tục di chuyển.
            if (!isMoving)
            {
                // Đặt cờ di chuyển thành true.
                isMoving = true;
                // Tiếp tục di chuyển ngẫu nhiên.
                MoveRandomly();
            }
        }
    }

    private void MoveRandomly()
    {
        // Nếu Slug không được phép di chuyển, thoát khỏi phương thức.
        if (!isMoving)
            return;

        // Tính toán vị trí ngẫu nhiên trong phạm vi di chuyển.
        float newPosX = initialPosition.x + Random.Range(-movementWidth / 2f, movementWidth / 2f);
        float newPosY = initialPosition.y + Random.Range(-movementHeight / 2f, movementHeight / 2f);
        Vector2 newPosition = new Vector2(newPosX, newPosY);

        // Lật ảnh của Slug nếu cần.
        spriteRenderer.flipX = (newPosition.x < transform.position.x);

        // Di chuyển đến vị trí mới.
        StartCoroutine(MoveToPosition(newPosition));
    }

    // Coroutine để di chuyển Slug đến vị trí cụ thể.
    private IEnumerator MoveToPosition(Vector2 destination)
    {
        // Di chuyển Slug cho đến khi đạt vị trí đích.
        while ((Vector2)transform.position != destination)
        {
            Vector2 movement = Vector2.ClampMagnitude(destination - (Vector2)transform.position, movementSpeed * Time.deltaTime);
            transform.position += (Vector3)movement;
            yield return null;
        }

        // Chờ một khoảng thời gian ngắn.
        yield return new WaitForSeconds(1f);

        // Tiếp tục di chuyển ngẫu nhiên.
        MoveRandomly();
    }

    private void StopMoving()
    {
        // Dừng tất cả các coroutine đang chạy.
        StopAllCoroutines();
        // Đặt cờ di chuyển thành false.
        isMoving = false;
    }
}
