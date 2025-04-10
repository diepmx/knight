using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    // Biến tĩnh (static) để truy cập GameController từ bất kỳ đâu
    public static GameController instance;

    [Space(10)]
    // Tham chiếu đến script EnemySpawner để quản lý việc sinh quái
    public EnemySpawner enemySpawner;
    // Hiệu ứng khi kẻ địch bị tiêu diệt
    public GameObject deathEffect;

    [Space(10)]
    // GameObject của Boss sẽ xuất hiện khi hết thời gian
    public GameObject boss;

    [Space(10)]
    // Thời gian tồn tại của game trước khi Boss xuất hiện (600 giây = 10 phút)
    public float endTimer = 600f;

    // Tham chiếu đến người chơi
    private CharacterController player;
    // Bộ đếm thời gian trong game
    private float timer;
    // Thời gian chơi tốt nhất được ghi nhận
    public int bestTimer;
    // Thời gian chờ trước khi hiển thị màn hình kết thúc
    private float waitToShowEndScreen = 2f;
    // Biến kiểm tra xem Boss đã xuất hiện chưa
    private bool bossSpawned = false;
    // Trạng thái của game (đang chạy hay không)
    public bool gameActive;

    // Kiểm tra điều kiện kết thúc game
    public bool endGame = true;

    // Nút mặc định được chọn khi màn hình game over xuất hiện
    public GameObject defaultGameOverSelectedButton;
    // Nút mặc định được chọn khi màn hình end game xuất hiện
    public GameObject defaultEndGameSelectedButton;

    public void Awake()
    {
        // Gán thể hiện (instance) cho biến tĩnh để truy cập toàn cục
        instance = this;
        // Đảm bảo game chạy bình thường (thoát khỏi trạng thái tạm dừng)
        Time.timeScale = 1f;
    }

    void Start()
    {
        gameActive = true;

        if (PlayerHealthController.instance != null)
        {
            GameObject playerObject = PlayerHealthController.instance.gameObject;
            player = playerObject.GetComponent<CharacterController>();
        }
        else
        {
            Debug.LogWarning("PlayerHealthController.instance is null in GameController.Start()");
        }
    }


    void Update()
    {
        // Nếu game đang hoạt động, cập nhật bộ đếm thời gian
        if (gameActive == true)
        {
            timer += Time.deltaTime;
            UIController.instance.UpdateTimer(timer);
        }

        // Nếu đến thời điểm xuất hiện Boss và game chưa kết thúc
        if (endGame == true && timer >= endTimer)
        {
            BossSpawn(); // Triệu hồi Boss
            EndGame();   // Kết thúc game
        }
    }

    // Phương thức xử lý khi game over
    public void GameOver()
    {
        gameActive = false;

        StartCoroutine(GameOverCo());
    }

    // Coroutine hiển thị màn hình game over sau một khoảng thời gian
    IEnumerator GameOverCo()
    {
        // Dừng âm thanh hiện tại
        SFXManager.instance.StopSFX(0);

        yield return new WaitForSeconds(waitToShowEndScreen);

        // Phát âm thanh kết thúc game
        SFXManager.instance.PlaySFX(8);

        // Tạm dừng game
        Time.timeScale = 0f;

        // Ghi nhận thời gian chơi
        bestTimer = Mathf.FloorToInt(timer);

        float minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = Mathf.FloorToInt(timer % 60);

        // Cập nhật thời gian trên màn hình game over
        UIController.instance.gameOverTimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        UIController.instance.gameOverScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultGameOverSelectedButton);
    }

    // Phương thức triệu hồi Boss khi đến thời điểm
    public void BossSpawn()
    {
        if (!bossSpawned)
        {
            // Rung màn hình khi Boss xuất hiện
            CameraShake.instance.ShakeIt(0.5f, 0.2f);

            // Xuất hiện Boss gần người chơi (cách -30 đơn vị trên trục X)
            Instantiate(boss, new Vector3(player.transform.position.x + (-30), player.transform.position.y, 0), Quaternion.identity);

            bossSpawned = true;
        }
    }

    // Phương thức kết thúc game
    public void EndGame()
    {
        // Tìm đối tượng Boss và danh sách kẻ địch trên màn hình
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Dừng việc sinh quái
        enemySpawner.StopEnemyGeneration();

        // Phát hiệu ứng và tiêu diệt tất cả kẻ địch hiện tại
        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;

            Instantiate(deathEffect, enemyPosition, transform.rotation);
            Destroy(enemy);
        }

        // Nếu Boss đã bị tiêu diệt, game kết thúc
        if (bossObject == null)
        {
            gameActive = false;

            StartCoroutine(GameEndCo());
        }
    }

    // Coroutine hiển thị màn hình end game sau một khoảng thời gian
    IEnumerator GameEndCo()
    {
        // Dừng âm thanh hiện tại
        SFXManager.instance.StopSFX(0);

        yield return new WaitForSeconds(waitToShowEndScreen);

        // Dừng nhạc nền và phát hiệu ứng âm thanh kết thúc game
        BGMManager.instance.StopBGM(0);
        SFXManager.instance.PlaySFX(9);
        SFXManager.instance.PlaySFX(10);

        // Tạm dừng game
        Time.timeScale = 0f;

        float minutes = Mathf.FloorToInt(timer / 60f);
        float seconds = Mathf.FloorToInt(timer % 60);

        // Cập nhật thời gian trên màn hình kết thúc game
        UIController.instance.endTimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        UIController.instance.levelEndScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(defaultEndGameSelectedButton);
    }
}
