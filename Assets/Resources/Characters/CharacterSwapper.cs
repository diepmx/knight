using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwapper : MonoBehaviour
{
    public static CharacterSwapper Instance;

    public GameObject currentPlayer;
    private string selectedCharacter;

    private void Awake()
    {
        Instance = this; // Không cần kiểm tra nữa, chỉ dùng 1 bản duy nhất trong Hub
    }


    public void SwapCharacterInHub(GameObject selectedInScene, string characterName, Transform anchorPoint)
    {
        if (currentPlayer != null)
        {
            // Quay về chỗ cũ
            CharacterAnchorData anchorData = currentPlayer.GetComponent<CharacterAnchorData>();
            if (anchorData != null && anchorData.defaultAnchor != null)
            {
                currentPlayer.transform.position = anchorData.defaultAnchor.position;
            }

            // Tắt UI
            CharacterHubDisplay oldDisplay = currentPlayer.GetComponent<CharacterHubDisplay>();
            if (oldDisplay != null)
                oldDisplay.DeactivateHubMode();

            currentPlayer.tag = "Untagged";
        }

        // Di chuyển player mới đến altar
        selectedInScene.transform.position = anchorPoint.position;
        currentPlayer = selectedInScene;
        currentPlayer.tag = "Player";

        // 👉 Lấy characterName từ chính CharacterSwapPoint (ưu tiên)
        CharacterSwapPoint swapPoint = selectedInScene.GetComponent<CharacterSwapPoint>();
        if (swapPoint != null)
        {
            selectedCharacter = swapPoint.characterName;
        }
        else
        {
            selectedCharacter = characterName; // fallback
        }

        // Bật UI
        CharacterHubDisplay newDisplay = currentPlayer.GetComponent<CharacterHubDisplay>();
        if (newDisplay != null)
            newDisplay.ActivateHubMode();

        // Camera
        Camera.main.GetComponent<CameraController>().SetTarget(currentPlayer.transform);

        // Input Map
        var input = currentPlayer.GetComponent<PlayerInput>();
        if (input != null)
            input.SwitchCurrentActionMap("InGame");
        // Gửi dữ liệu qua PlayerDataCarrier để load nhân vật đúng ở màn sau
        if (PlayerDataCarrier.Instance != null)
        {
            PlayerDataCarrier.Instance.SetSelectedCharacterName(selectedCharacter);
        }

    }

    public string GetSelectedCharacterName()
    {
        return selectedCharacter;
    }
}
