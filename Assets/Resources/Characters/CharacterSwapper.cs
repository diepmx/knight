using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwapper : MonoBehaviour
{
    public static CharacterSwapper Instance;
    public GameObject currentPlayer;
    private string selectedCharacter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Gọi trong Hub Scene
    // Gọi trong Hub Scene
    public void SwapCharacterInHub(GameObject selectedInScene, string characterName, Transform anchorPoint)
    {
        if (currentPlayer != null)
        {
            // 🟢 Di chuyển về vị trí altar gốc của nó trước
            CharacterAnchorData anchorData = currentPlayer.GetComponent<CharacterAnchorData>();
            if (anchorData != null && anchorData.defaultAnchor != null)
            {
                currentPlayer.transform.position = anchorData.defaultAnchor.position;
               
            }

            // 🛑 Tắt điều khiển và các UI của player cũ
            CharacterHubDisplay oldDisplay = currentPlayer.GetComponent<CharacterHubDisplay>();
            if (oldDisplay != null)
                oldDisplay.DeactivateHubMode();

            currentPlayer.tag = "Untagged";
        }


        // ✅ Đưa player mới tới altar hiện tại
        selectedInScene.transform.position = anchorPoint.position;

        // Gán player mới
        currentPlayer = selectedInScene;
        currentPlayer.tag = "Player";

        // Bật điều khiển và UI cho player mới
        CharacterHubDisplay newDisplay = currentPlayer.GetComponent<CharacterHubDisplay>();
        if (newDisplay != null)
            newDisplay.ActivateHubMode();

        // Cập nhật camera
        Camera.main.GetComponent<CameraController>().SetTarget(currentPlayer.transform);

        // Chuyển action map
        var input = currentPlayer.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (input != null)
            input.SwitchCurrentActionMap("InGame");

        // Ghi nhận tên nhân vật
        selectedCharacter = characterName;
    }










    public string GetSelectedCharacterName()
    {
        return selectedCharacter;
    }

}
