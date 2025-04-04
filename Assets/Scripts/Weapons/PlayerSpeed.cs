using UnityEngine;

public class PlayerSpeed : Weapon
{
    public void Start()
    {
        // Nâng cấp tốc độ di chuyển khi bắt đầu trò chơi.
        UpgradeMovespeed();
    }

    public void Update()
    {
        // Kiểm tra nếu các chỉ số đã được cập nhật và nâng cấp tốc độ di chuyển.
        if (statsUpdated == true)
        {
            statsUpdated = false;
            // Nâng cấp tốc độ di chuyển.
            UpgradeMovespeed();
        }
    }

    // Phương thức nâng cấp tốc độ di chuyển.
    public void UpgradeMovespeed()
    {
        // Gọi PlayerController để nâng cấp tốc độ di chuyển.
        PlayerController.instance.SpeedLevelUp();
    }
}
