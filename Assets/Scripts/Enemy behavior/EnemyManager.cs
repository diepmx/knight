using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    

    void Start()
    {
        StartCoroutine(WaitThenInitEnemies());
    }

    IEnumerator WaitThenInitEnemies()
    {
        // Đợi tới khi ActiveCharacter đã gán
        yield return new WaitUntil(() => CharacterController.ActiveCharacter != null);

        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
        {
            enemy.SetTarget(CharacterController.ActiveCharacter.transform);
        }
    }



}
