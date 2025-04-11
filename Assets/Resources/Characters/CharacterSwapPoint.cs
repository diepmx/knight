using UnityEngine;

public class CharacterSwapPoint : MonoBehaviour
{
    public static CharacterSwapPoint instance;
    private void Awake()
    {
        if (gameObject.activeInHierarchy)
        {
            instance = this;
        }
    }
    [Header("Nhân vật sẽ điều khiển nếu đổi")]
    public GameObject targetCharacterInScene;

    [Header("Tên để load prefab sau này (nếu cần)")]
    public string characterName;

    [Header("Vị trí xuất hiện khi trở thành player")]
    public Transform anchorPoint;

    private GameObject currentPlayerInZone;

    void Update()
    {
        if (currentPlayerInZone != null && currentPlayerInZone.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("[CharacterSwap] Swapping to " + characterName);
                CharacterSwapper.Instance.SwapCharacterInHub(targetCharacterInScene, characterName, anchorPoint);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayerInZone = other.gameObject;
            Debug.Log("[CharacterSwapPoint] Player đã vào vùng");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentPlayerInZone)
        {
            currentPlayerInZone = null;
            Debug.Log("[CharacterSwapPoint] Player đã rời vùng");
        }
    }
}
