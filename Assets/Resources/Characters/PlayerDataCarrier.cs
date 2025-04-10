using UnityEngine;

public class PlayerDataCarrier : MonoBehaviour
{
    public static PlayerDataCarrier Instance;

    private string selectedCharacterName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Tránh tạo trùng
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
    }


    public void SetSelectedCharacterName(string name)
    {
        selectedCharacterName = name;
    }

    public string GetSelectedCharacterName()
    {
        return selectedCharacterName;
    }
}
