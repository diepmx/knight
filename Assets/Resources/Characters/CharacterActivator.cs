using UnityEngine;

public class CharacterActivator : MonoBehaviour
{
    void Start()
    {
        string selectedName = PlayerDataCarrier.Instance.GetSelectedCharacterName();

        foreach (Transform child in transform)
        {
            CharacterData data = child.GetComponent<CharacterData>();
            if (data != null)
            {
                bool shouldActivate = data.characterName == selectedName;
                child.gameObject.SetActive(shouldActivate);
                if (shouldActivate)
                {
                    Debug.Log($"✔️ Đã bật nhân vật: {data.characterName}");
                }
            }
        }
    }
}
