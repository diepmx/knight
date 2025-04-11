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
        // Đợi tới khi player đã khởi tạo
        yield return new WaitUntil(() => PlayerHealthController.instance != null);

        Transform playerTransform = PlayerHealthController.instance.transform;

        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
        {
            enemy.SetTarget(playerTransform);
        }
    }



}
