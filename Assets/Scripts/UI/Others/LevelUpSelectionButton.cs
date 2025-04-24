using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSelectionButton : MonoBehaviour
{
    [Space(10)]
    // Văn bản hiển thị mô tả nâng cấp vũ khí
    public TMP_Text upgradeDescText;

    // Văn bản hiển thị tên vũ khí và cấp độ hiện tại
    public TMP_Text nameLevelText;

    // Hình ảnh hiển thị biểu tượng vũ khí
    public Image weaponIcon;

    // Vũ khí gán cho nút này
    private Weapon assignedWeapon;

    // Cập nhật hiển thị của nút với vũ khí đã cho
    public void UpdateButtonDisplay(Weapon theWeapon)
    {
        if (theWeapon == null)
        {
            Debug.LogError("The weapon passed to UpdateButtonDisplay is null!");
            return;
        }

        if (theWeapon.stats == null || theWeapon.stats.Count <= theWeapon.weaponLevel)
        {
            Debug.LogError($"Weapon stats are null or weaponLevel ({theWeapon.weaponLevel}) is out of range for weapon: {theWeapon.name}");
            return;
        }

        var weaponStats = theWeapon.stats[theWeapon.weaponLevel];
        if (weaponStats == null)
        {
            Debug.LogError($"Weapon stats at level {theWeapon.weaponLevel} are null for weapon: {theWeapon.name}");
            return;
        }

        if (theWeapon.gameObject.activeSelf)
        {
            upgradeDescText.text = weaponStats.upgradeText;
            weaponIcon.sprite = theWeapon.icon;
            nameLevelText.text = theWeapon.name;
        }
        else
        {
            if (theWeapon.tag != "PlayerUpdate")
            {
                upgradeDescText.text = "UNLOCK\n" + theWeapon.name;
                weaponIcon.sprite = theWeapon.icon;
                nameLevelText.text = theWeapon.name;
            }
            else
            {
                upgradeDescText.text = "+10%";
                weaponIcon.sprite = theWeapon.icon;
                nameLevelText.text = theWeapon.name;
            }
        }

        if (theWeapon.icon != null)
        {
            weaponIcon.sprite = theWeapon.icon;
        }
        else
        {
            Debug.LogWarning("Weapon icon is NULL for: " + theWeapon.name);
        }

        assignedWeapon = theWeapon;
    }

    // Hàm xử lý việc chọn nâng cấp
    public void SelectUpgrade()
    {
        if (assignedWeapon != null)
        {
            // Nếu vũ khí đang hoạt động, nâng cấp vũ khí
            if (assignedWeapon.gameObject.activeSelf == true)
            {
                assignedWeapon.LevelUp();
            }
            else
            {
                // Nếu vũ khí không hoạt động, thêm nó vào vũ khí của người chơi
                CharacterController.instance.AddWeapon(assignedWeapon);
            }

            // Đóng bảng nâng cấp và tiếp tục trò chơi
            UIController.instance.levelUpPanel.SetActive(false);
            GameController.instance.gameActive = true;
            Time.timeScale = 1f;
        }
    }
}
