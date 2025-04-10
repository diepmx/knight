using UnityEngine;

public class PlayerSceneInitializer : MonoBehaviour
{
    void Start()
    {
        string selectedName = CharacterSwapper.Instance?.GetSelectedCharacterName();
        if (string.IsNullOrEmpty(selectedName)) return;

        // So sánh tên object trong scene với character name
        if (gameObject.name == selectedName)
        {
            Debug.Log($"[Scene Init] Kích hoạt player: {selectedName}");

            gameObject.SetActive(true);
            gameObject.tag = "Player";

            // Di chuyển đến vị trí spawn
            Transform spawnPoint = GameObject.FindWithTag("Respawn")?.transform;
            if (spawnPoint != null)
                transform.position = spawnPoint.position;

            // Camera follow
            Camera.main.GetComponent<CameraController>().SetTarget(transform);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
