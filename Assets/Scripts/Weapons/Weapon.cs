using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Space(10)]
    // Danh sách thống kê vũ khí.
    public List<WeaponStats> stats;
    // Cấp độ hiện tại của vũ khí.
    public int weaponLevel;
    // Biểu tượng của vũ khí.
    public Sprite icon;

    [HideInInspector]
    // Cờ để xác định liệu các thống kê có được cập nhật hay không.
    public bool statsUpdated;

    // Nâng cấp vũ khí.
    public void LevelUp()
    {
        // Kiểm tra xem có còn cấp độ để nâng cấp không.
        if (weaponLevel < stats.Count - 1)
        {
            // Tăng cấp độ vũ khí.
            weaponLevel++;
            // Đánh dấu thống kê đã được cập nhật.
            statsUpdated = true;

            // Nếu đã đạt đến cấp độ tối đa, quản lý danh sách vũ khí của người chơi.
            if (weaponLevel >= stats.Count - 1)
            {
                // Thêm vũ khí đã nâng cấp hoàn toàn vào danh sách và loại bỏ khỏi vũ khí đã được chỉ định.
                CharacterController.instance.fullyLevelledWeapons.Add(this);
                CharacterController.instance.assignedWeapons.Remove(this);
            }
        }
    }
}

// Lớp có thể tuần tự hóa cho thống kê vũ khí.
[System.Serializable]
public class WeaponStats
{
    // Các thuộc tính cho thống kê vũ khí.
    public float speed, damage, range, timeBetweenAttacks, amount, duration;
    // Văn bản nâng cấp.
    public string upgradeText;
}
