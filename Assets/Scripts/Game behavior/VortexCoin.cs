using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexCoin : MonoBehaviour 
{
    // Tham chiếu đến script PlayerController
    private CharacterController player;
    // Lưu lại phạm vi nhặt vật phẩm ban đầu của người chơi
    private float originalPickupRange;
    // Tham chiếu đến component Animator
    private Animator animator;

    // Giá trị tăng phạm vi nhặt vật phẩm khi người chơi tương tác với VortexCoin
    public float increasePickupRange = 120f;
    // Thời gian hiệu ứng có hiệu lực
    public float effectDuration = 5f;

    void Start()
    {
        // Gán PlayerController hiện tại vào biến player
        player = CharacterController.instance;
        // Lưu phạm vi nhặt vật phẩm ban đầu của người chơi
        originalPickupRange = player.pickupRange;
        // Lấy component Animator từ VortexCoin
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu người chơi va chạm với VortexCoin và rương đang đóng
        if (collision.CompareTag("Player") && CharacterController.instance.isChestClosed)
        {
            // Mở rương và tăng phạm vi nhặt vật phẩm của người chơi
            CharacterController.instance.isChestClosed = false;
            player.pickupRange = increasePickupRange;

            // Kích hoạt animation nếu có Animator
            if (animator != null)
            {
                animator.SetBool("IsTouched", true);
            }

            // Đặt lại phạm vi nhặt vật phẩm và hủy VortexCoin sau một khoảng thời gian
            StartCoroutine(ResetPickRangeAfterDelay());
        }
    }

    // Coroutine để đặt lại phạm vi nhặt vật phẩm và hủy VortexCoin sau một khoảng thời gian
    private IEnumerator ResetPickRangeAfterDelay()
    {
        // Chờ đến khi hết thời gian hiệu ứng
        yield return new WaitForSeconds(effectDuration);

        // Đặt lại phạm vi nhặt vật phẩm của người chơi, hủy VortexCoin và đặt lại trạng thái rương
        player.pickupRange = originalPickupRange;
        Destroy(gameObject);
        CharacterController.instance.isChestClosed = true;
        CharacterController.instance.isChestSpawned = false;
    }
}
