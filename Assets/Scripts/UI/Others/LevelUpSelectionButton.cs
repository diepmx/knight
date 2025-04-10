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
        // Nếu vũ khí đang hoạt động, hiển thị mô tả nâng cấp và biểu tượng của nó
        if (theWeapon.gameObject.activeSelf == true)
        {
            upgradeDescText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;
            weaponIcon.sprite = theWeapon.icon;

            nameLevelText.text = theWeapon.name;
        }
        else
        {
            // Nếu vũ khí không hoạt động nhưng có thể mở khóa, hiển thị văn bản mở khóa và biểu tượng của nó
            if (theWeapon.tag != "PlayerUpdate")
            {
                upgradeDescText.text = "UNLOCK\n" + theWeapon.name;
                weaponIcon.sprite = theWeapon.icon;

                nameLevelText.text = theWeapon.name;
            }
            else
            {
                // Nếu vũ khí là cập nhật người chơi, hiển thị mô tả nâng cấp chung
                upgradeDescText.text = "+10%";
                weaponIcon.sprite = theWeapon.icon;

                nameLevelText.text = theWeapon.name;
            }
        }

        // Gán vũ khí hiện tại cho nút
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
