using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponListController : MonoBehaviour
{
    // Tham chiếu đến đối tượng PlayerController, nơi chứa danh sách các vũ khí của người chơi
    public PlayerController playerController;
    // Mảng các UI Image dùng để hiển thị các biểu tượng vũ khí của người chơi
    public Image[] weaponImagesUI;
    // Danh sách chứa các biểu tượng vũ khí (Sprites) của người chơi
    private List<Sprite> weaponImages = new();

    // Cập nhật giao diện người chơi mỗi frame
    void Update()
    {
        int displayedWeaponsCount = 0;

        // Duyệt qua các phần tử UI Image trong weaponImagesUI
        for (int i = 0; i < weaponImagesUI.Length; i++)
        {
            // Kiểm tra xem có vũ khí trong danh sách của người chơi ở chỉ mục này không
            if (i < playerController.listWeapons.Count)
            {
                Weapon weapon = playerController.listWeapons[i];

                // Kiểm tra nếu vũ khí không có tag "PlayerUpdate" và có biểu tượng
                if (weapon.tag != "PlayerUpdate" && weapon.icon != null)
                {
                    // Kích hoạt UI và hiển thị biểu tượng của vũ khí
                    weaponImagesUI[displayedWeaponsCount].gameObject.SetActive(true);
                    weaponImagesUI[displayedWeaponsCount].sprite = weapon.icon;

                    // Nếu displayedWeaponsCount >= số phần tử trong weaponImages, thêm biểu tượng vũ khí vào danh sách
                    if (displayedWeaponsCount >= weaponImages.Count)
                    {
                        weaponImages.Add(weapon.icon);
                    }
                    else
                    {
                        // Cập nhật lại biểu tượng trong danh sách nếu có
                        weaponImages[displayedWeaponsCount] = weapon.icon;
                    }
                    displayedWeaponsCount++;
                }
                else
                {
                    // Nếu vũ khí có tag "PlayerUpdate" hoặc không có biểu tượng, ẩn UI
                    weaponImagesUI[displayedWeaponsCount].gameObject.SetActive(false);
                }
            }
            else
            {
                // Nếu không có vũ khí ở chỉ mục này, ẩn UI
                weaponImagesUI[i].gameObject.SetActive(false);
            }
        }
    }
}
