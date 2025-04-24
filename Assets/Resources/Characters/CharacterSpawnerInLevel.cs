using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSpawnerInLevel : MonoBehaviour
{
    void Start()
    {
        string selectedCharacterName = PlayerDataCarrier.Instance?.GetSelectedCharacterName();
        //if (string.IsNullOrEmpty(selectedCharacterName))
        //{
        //    Debug.LogError("Không tìm thấy tên nhân vật đã chọn!");
        //     // Thay bằng tên nhân vật mặc định của bạn
        //    return;
        //}
        selectedCharacterName = "ArchDemon";

        // Tìm tất cả nhân vật bị ẩn trong scene có PlayerHealthController
        PlayerHealthController[] allCharacters = Resources.FindObjectsOfTypeAll<PlayerHealthController>();
        GameObject selectedCharacter = null;

        Debug.Log("== Danh sách nhân vật tìm thấy trong scene ==");
        foreach (PlayerHealthController ch in allCharacters)
        {
            Debug.Log("👤 " + ch.gameObject.name);
            if (ch.gameObject.name.Trim().ToLower() == selectedCharacterName.Trim().ToLower())
            {
                selectedCharacter = ch.gameObject;
            }
        }

        if (selectedCharacter == null)
        {
            Debug.LogError("Không tìm thấy nhân vật " + selectedCharacterName + " trong scene!");
            return;
        }

        // Kích hoạt nhân vật đã chọn
        selectedCharacter.SetActive(true);

        // Gán tag và setup camera theo dõi
        selectedCharacter.tag = "Player";
        Camera.main.GetComponent<CameraController>()?.SetTarget(selectedCharacter.transform);

        // Kích hoạt action map InGame nếu có PlayerInput
        var input = selectedCharacter.GetComponent<PlayerInput>();
        if (input != null)
        {
            input.SwitchCurrentActionMap("InGame");
        }
    }
}
