using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Hiệu ứng nổ sẽ kích hoạt khi đối tượng bị hủy.
    public GameObject deathEffect;

    void OnDestroy()
    {
        // Nếu game vẫn đang chạy thì mới tạo hiệu ứng nổ
        if (GameController.instance != null && GameController.instance.gameActive)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
    }

}
