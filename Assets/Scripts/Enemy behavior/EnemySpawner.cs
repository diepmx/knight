using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Space(10)]
    public Transform minSpawn;
    public Transform maxSpawn;

    [Space(10)]
    public GameObject enemyToSpawn;

    [Space(10)]
    public float timeToSpawn;

    [Space(10)]
    public int checkPerFrame;
    public List<WaveInfo> waves;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private int enemyToCheck;
    private Transform target;
    private float despawnDistance;
    private float spawnCounter;
    private int currentWave;
    private float waveCounter;

    void Start()
    {
        spawnCounter = timeToSpawn;

        // Đợi PlayerHealthController được khởi tạo hoàn toàn rồi mới gán target
        if (PlayerHealthController.instance != null)
        {
            target = PlayerHealthController.instance.transform;
        }

        despawnDistance = Vector3.Distance(transform.position, maxSpawn.position) + 5f;
        currentWave = -1;
        GoToNextWave();
    }

    void Update()
    {
        // Cập nhật lại target nếu bị null
        if (target == null && PlayerHealthController.instance != null)
        {
            target = PlayerHealthController.instance.transform;
        }

        if (PlayerHealthController.instance != null && PlayerHealthController.instance.gameObject != null)
        {
            if (currentWave < waves.Count)
            {
                waveCounter -= Time.deltaTime;
                if (waveCounter <= 0)
                {
                    GoToNextWave();
                }

                spawnCounter -= Time.deltaTime;
                if (spawnCounter <= 0)
                {
                    spawnCounter = waves[currentWave].timeBetweenSpawns;
                    GameObject newEnemy = Instantiate(waves[currentWave].enemyToSpawn, SelectSpawnPoint(), Quaternion.identity);
                    UIController.instance.IncrementEnemiesSpawned();
                    spawnedEnemies.Add(newEnemy);
                }
            }

            // Di chuyển spawner theo người chơi nếu target đã được gán
            if (target != null)
            {
                transform.position = target.position;
            }
        }

        // Kiểm tra và hủy các kẻ địch vượt quá phạm vi
        int checkTarget = enemyToCheck + checkPerFrame;
        while (enemyToCheck < checkTarget)
        {
            if (enemyToCheck < spawnedEnemies.Count)
            {
                if (spawnedEnemies[enemyToCheck] != null)
                {
                    if (Vector3.Distance(transform.position, spawnedEnemies[enemyToCheck].transform.position) > despawnDistance)
                    {
                        Destroy(spawnedEnemies[enemyToCheck]);
                        spawnedEnemies.RemoveAt(enemyToCheck);
                        checkTarget--;
                    }
                    else
                    {
                        enemyToCheck++;
                    }
                }
                else
                {
                    spawnedEnemies.RemoveAt(enemyToCheck);
                    checkTarget--;
                }
            }
            else
            {
                enemyToCheck = 0;
                checkTarget = 0;
            }
        }
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        bool spawVerticalEdge = Random.Range(0f, 1f) > .5f;

        if (spawVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);
            spawnPoint.x = Random.Range(0f, 1f) > .5f ? maxSpawn.position.x : minSpawn.position.x;
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);
            spawnPoint.y = Random.Range(0f, 1f) > .5f ? maxSpawn.position.y : minSpawn.position.y;
        }

        return spawnPoint;
    }

    public void GoToNextWave()
    {
        currentWave++;
        if (currentWave >= waves.Count)
        {
            currentWave = waves.Count - 1;
        }
        waveCounter = waves[currentWave].waveLength;
        spawnCounter = waves[currentWave].timeBetweenSpawns;
    }

    public void StopEnemyGeneration()
    {
        enabled = false;
    }
}

[System.Serializable]
public class WaveInfo
{
    public GameObject enemyToSpawn;
    public float waveLength = 10f;
    public float timeBetweenSpawns = 1f;
}
