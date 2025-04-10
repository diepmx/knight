using UnityEngine;

public class LevelCharacterActivator : MonoBehaviour
{
    void Start()
    {
        if (PlayerDataCarrier.Instance == null)
        {
            Debug.LogError("[LevelCharacterActivator] ❌ PlayerDataCarrier bị null!");
            return;
        }

        string selectedName = PlayerDataCarrier.Instance.GetSelectedCharacterName();
        Debug.Log("[LevelCharacterActivator] Selected character: " + selectedName);

        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject character in allCharacters)
        {
            bool isMatch = character.name == selectedName;
            character.SetActive(isMatch);
            Debug.Log("→ " + character.name + " = " + (isMatch ? "✅ ON" : "❌ OFF"));
        }
    }
}
