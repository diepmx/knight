using UnityEngine;

public class PlayerPickupRange : Weapon
{
    public void Start()
    {
        // Nâng cấp phạm vi nhặt vật phẩm khi bắt đầu trò chơi.
        UpgradePickupRange();
    }

    public void Update()
    {
        // Kiểm tra nếu các chỉ số đã được cập nhật và nâng cấp phạm vi nhặt vật phẩm.
        if (statsUpdated == true)
        {
            statsUpdated = false;
            // Nâng cấp phạm vi nhặt vật phẩm.
            UpgradePickupRange();
        }
    }

    // Phương thức nâng cấp phạm vi nhặt vật phẩm.
    public void UpgradePickupRange()
    {
        // Gọi PlayerController để nâng cấp phạm vi nhặt vật phẩm.
        PlayerController.instance.PickupRangeLevelUp();
    }
}
