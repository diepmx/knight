using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Các biến xác định khu vực sinh sản kẻ địch
    [Space(10)]
    // Điểm giới hạn tối thiểu của khu vực sinh sản
    public Transform minSpawn;
    // Điểm giới hạn tối đa của khu vực sinh sản
    public Transform maxSpawn;

    // Biến liên quan đến kẻ địch được sinh ra
    [Space(10)]
    // Prefab của kẻ địch sẽ được sinh ra
    public GameObject enemyToSpawn;

    [Space(10)]
    // Thời gian giữa mỗi lần sinh kẻ địch
    public float timeToSpawn;

    // Biến kiểm soát việc hủy kẻ địch và theo dõi
    [Space(10)]
    // Số lượng kẻ địch được kiểm tra để hủy mỗi frame
    public int checkPerFrame;
    // Danh sách các đợt sinh sản kẻ địch
    public List<WaveInfo> waves;

    // Danh sách các kẻ địch đã được sinh ra
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    // Chỉ số của kẻ địch sẽ được kiểm tra để hủy
    private int enemyToCheck;
    // Vị trí mục tiêu mà bộ sinh sản sẽ theo dõi (thường là người chơi)
    private Transform target;
    // Khoảng cách từ bộ sinh sản mà kẻ địch sẽ bị hủy
    private float despawnDistance;
    // Bộ đếm thời gian để sinh kẻ địch
    private float spawnCounter;
    // Chỉ số của đợt hiện tại
    private int currentWave;
    // Bộ đếm thời gian cho thời gian tồn tại của đợt hiện tại
    private float waveCounter;

    void Start()
    {
        // Khởi tạo các giá trị ban đầu
        spawnCounter = timeToSpawn;
        target = PlayerHealthController.instance.transform;
        // Tính toán khoảng cách để hủy kẻ địch
        despawnDistance = Vector3.Distance(transform.position, maxSpawn.position) + 5f;
        currentWave = -1;
        // Bắt đầu đợt đầu tiên
        GoToNextWave();
    }

    void Update()
    {
        if (PlayerHealthController.instance.gameObject.activeSelf)
        {
            // Chỉ tiếp tục logic khi người chơi vẫn còn tồn tại
            if (currentWave < waves.Count)
            {
                // Đếm ngược thời gian của đợt hiện tại
                waveCounter -= Time.deltaTime;
                if (waveCounter <= 0)
                {
                    // Chuyển sang đợt tiếp theo khi hết thời gian
                    GoToNextWave();
                }
                spawnCounter -= Time.deltaTime;
                if (spawnCounter <= 0)
                {
                    // Sinh kẻ địch mới khi bộ đếm đạt 0
                    spawnCounter = waves[currentWave].timeBetweenSpawns;
                    GameObject newEnemy = Instantiate(waves[currentWave].enemyToSpawn, SelectSpawnPoint(), Quaternion.identity);
                    UIController.instance.IncrementEnemiesSpawned();
                    // Thêm kẻ địch vào danh sách quản lý
                    spawnedEnemies.Add(newEnemy);
                }
            }
        }
        // Cập nhật vị trí của bộ sinh sản theo vị trí người chơi
        transform.position = target.position;

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
                        // Hủy kẻ địch nếu nó ở quá xa
                        Destroy(spawnedEnemies[enemyToCheck]);
                        spawnedEnemies.RemoveAt(enemyToCheck);
                        checkTarget--;
                    }
                    else
                    {
                        // Chuyển sang kiểm tra kẻ địch tiếp theo
                        enemyToCheck++;
                    }
                }
                else
                {
                    // Xóa kẻ địch null khỏi danh sách
                    spawnedEnemies.RemoveAt(enemyToCheck);
                    checkTarget--;
                }
            }
            else
            {
                // Đặt lại chỉ số kiểm tra nếu đã kiểm tra hết danh sách
                enemyToCheck = 0;
                checkTarget = 0;
            }
        }
    }

    // Chọn một điểm ngẫu nhiên trong khu vực sinh sản
    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        // Xác định sinh sản theo cạnh dọc hay ngang
        bool spawVerticalEdge = Random.Range(0f, 1f) > .5f;
        if (spawVerticalEdge)
        {
            // Chọn ngẫu nhiên vị trí theo trục y
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);
            // Chọn giữa x tối đa hoặc tối thiểu
            spawnPoint.x = Random.Range(0f, 1f) > .5f ? maxSpawn.position.x : minSpawn.position.x;
        }
        else
        {
            // Chọn ngẫu nhiên vị trí theo trục x
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);
            // Chọn giữa y tối đa hoặc tối thiểu
            spawnPoint.y = Random.Range(0f, 1f) > .5f ? maxSpawn.position.y : minSpawn.position.y;
        }
        return spawnPoint;
    }

    // Chuyển sang đợt sinh sản tiếp theo
    public void GoToNextWave()
    {
        currentWave++;
        // Đảm bảo chỉ số không vượt quá số lượng đợt có sẵn
        if (currentWave >= waves.Count)
        {
            currentWave = waves.Count - 1;
        }
        // Cập nhật thời gian cho đợt mới
        waveCounter = waves[currentWave].waveLength;
        spawnCounter = waves[currentWave].timeBetweenSpawns;
    }

    // Dừng việc sinh sản kẻ địch
    public void StopEnemyGeneration()
    {
        enabled = false;
    }
}

[System.Serializable]
public class WaveInfo
{
    // Prefab kẻ địch cho đợt này
    public GameObject enemyToSpawn;
    // Thời gian tồn tại của đợt
    public float waveLength = 10f;
    // Thời gian giữa mỗi lần sinh kẻ địch
    public float timeBetweenSpawns = 1f;
}
