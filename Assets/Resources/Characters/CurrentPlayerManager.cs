using UnityEngine;

public class CurrentPlayerManager : MonoBehaviour
{
    public static CurrentPlayerManager Instance;

    public Transform currentCharacter;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCurrentCharacter(Transform character)
    {
        currentCharacter = character;

        // Cập nhật lại target cho tất cả EnemyController
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.SetTarget(currentCharacter);
        }
    }
}
