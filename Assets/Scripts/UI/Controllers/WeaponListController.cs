using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponListController : MonoBehaviour
{
    // Tham chiếu đến đối tượng PlayerController, nơi chứa danh sách các vũ khí của người chơi
    public CharacterController characterController;
    // Mảng các UI Image dùng để hiển thị các biểu tượng vũ khí của người chơi
    public Image[] weaponImagesUI;
    // Danh sách chứa các biểu tượng vũ khí (Sprites) của người chơi
    private List<Sprite> weaponImages = new();

    // Cập nhật giao diện người chơi mỗi frame
    void Update()
    {
        if (CharacterController.ActiveCharacter == null)
            return;

        characterController = CharacterController.ActiveCharacter;

        if (characterController.listWeapons == null)
            return;

        int displayedWeaponsCount = 0;

        for (int i = 0; i < weaponImagesUI.Length; i++)
        {
            if (i < characterController.listWeapons.Count)
            {
                Weapon weapon = characterController.listWeapons[i];

                if (weapon.tag != "PlayerUpdate" && weapon.icon != null)
                {
                    weaponImagesUI[displayedWeaponsCount].gameObject.SetActive(true);
                    weaponImagesUI[displayedWeaponsCount].sprite = weapon.icon;

                    if (displayedWeaponsCount >= weaponImages.Count)
                    {
                        weaponImages.Add(weapon.icon);
                    }
                    else
                    {
                        weaponImages[displayedWeaponsCount] = weapon.icon;
                    }
                    displayedWeaponsCount++;
                }
                else
                {
                    weaponImagesUI[displayedWeaponsCount].gameObject.SetActive(false);
                }
            }
            else
            {
                weaponImagesUI[i].gameObject.SetActive(false);
            }
        }
    }

}
