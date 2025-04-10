using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Singleton instance của UIController
    public static UIController instance;

    // Các phần tử UI hiển thị kinh nghiệm
    [Space(10)]
    public Slider experienceLevelSlider;  // Thanh trượt kinh nghiệm
    public TMP_Text experienceLevelText;  // Văn bản hiển thị cấp độ

    // Các nút chọn khi lên cấp
    public LevelUpSelectionButton[] levelUpButton;

    // Các panel UI
    public GameObject levelUpPanel;
    public GameObject statisticsPanel;

    // Cờ để theo dõi xem panel thống kê có đang hiển thị không
    public bool isStatisticsPanelDisplayed = false;

    // Tên của cảnh menu chính
    public string mainMenuName;

    // Các phần tử UI cho màn hình pause
    public GameObject pauseScreen;
    public GameObject defaultSelectedButton;

    [Space(10)]
    // Các phần tử UI hiển thị thống kê
    public TMP_Text totalPlayerDistance;
    public TMP_Text totalDamageReceived;
    public TMP_Text totalEnemiesSpawned;
    public TMP_Text totalEnemiesDefeated;
    public TMP_Text totalCoinsCollected;

    [Space(10)]
    public int enemiesSpawned;
    public int enemiesDefeated;
    public int coinsCollected;

    [Space(10)]
    public TMP_Text daggerLevel;
    public TMP_Text dangerZoneLevel;
    public TMP_Text doubleAxeLevel;
    public TMP_Text fireballLevel;
    public TMP_Text lightningLevel;
    public TMP_Text orbitingShurikenLevel;

    [Space(10)]
    // Các phần tử UI hiển thị thời gian
    public TMP_Text timeText;

    // Các phần tử UI cho màn hình game over và màn hình kết thúc level
    public GameObject gameOverScreen;
    public GameObject levelEndScreen;
    public TMP_Text gameOverTimerText;
    public TMP_Text endTimerText;

    private void Awake()
    {
        instance = this;
    }

    // Cập nhật panel thống kê với các giá trị hiện tại
    private void Update()
    {
        // Cập nhật các thông tin thống kê
        totalPlayerDistance.text = "PLAYER DISTANCE: " + CharacterController.instance.playerDistance.ToString("000000");
        totalDamageReceived.text = "DAMAGE RECEIVED: " + PlayerHealthController.instance.totalDamage.ToString("000000");
        totalEnemiesSpawned.text = "ENEMIES SPAWNED: " + enemiesSpawned.ToString("000000");
        totalEnemiesDefeated.text = "ENEMIES DEFEATED: " + enemiesDefeated.ToString("000000");
        totalCoinsCollected.text = "COINS COLLECTED: " + coinsCollected.ToString("000000");

        // Cập nhật mức độ các vũ khí
        if (Dagger.instance != null && Dagger.instance.gameObject.activeSelf)
        {
            daggerLevel.text = "DAGGER LEVEL " + (Dagger.instance.daggerLevel + 1) + "/15";
        }
        if (DangerZone.instance != null && DangerZone.instance.gameObject.activeSelf)
        {
            dangerZoneLevel.text = "DANGER ZONE LEVEL " + (DangerZone.instance.dangerZoneLevel + 1) + "/15";
        }
        if (DoubleAxe.instance != null && DoubleAxe.instance.gameObject.activeSelf)
        {
            doubleAxeLevel.text = "DOUBLE AXE LEVEL " + (DoubleAxe.instance.doubleAxeLevel + 1) + "/15";
        }
        if (Fireball.instance != null && Fireball.instance.gameObject.activeSelf)
        {
            fireballLevel.text = "FIREBALL LEVEL " + (Fireball.instance.fireballLevel + 1) + "/15";
        }
        if (Lightning.instance != null && Lightning.instance.gameObject.activeSelf)
        {
            lightningLevel.text = "LIGHTNING LEVEL " + (Lightning.instance.lightningLevel + 1) + "/15";
        }
        if (OrbitingShuriken.instance != null && OrbitingShuriken.instance.gameObject.activeSelf)
        {
            orbitingShurikenLevel.text = "SHURIKEN LEVEL " + (OrbitingShuriken.instance.orbitingShurikenLevel + 1) + "/15";
        }
    }

    // Phương thức gọi khi nhấn nút pause
    public void OnPressPause()
    {
        // Kiểm tra xem game có đang hoạt động không
        if (GameController.instance.gameActive == true)
        {
            // Nếu game đang hoạt động, toggle pause/unpause
            PauseUnpause();
        }
    }

    // Phương thức để hiển thị/ẩn panel thống kê
    public void OnDisplayStatistics()
    {
        if (statisticsPanel.activeSelf == false)
        {
            statisticsPanel.SetActive(true);
        }
        else
        {
            statisticsPanel.SetActive(false);
        }
    }

    // Cập nhật các phần tử UI kinh nghiệm
    public void UpdateExperience(int currentExperience, int levelExperience, int currentLevel)
    {
        experienceLevelSlider.maxValue = levelExperience;
        experienceLevelSlider.value = currentExperience;

        experienceLevelText.text = "Level " + currentLevel;
    }

    // Phương thức bỏ qua level up panel
    public void SkipLevelUp()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        GameController.instance.gameActive = true;
    }

    // Phương thức quay về menu chính
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
        Time.timeScale = 1f;
    }

    // Phương thức để khởi động lại scene hiện tại
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    // Phương thức thoát game
    public void Quitgame()
    {
        Application.Quit();
    }

    // Phương thức để tạm dừng hoặc tiếp tục game
    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf == false)
        {
            // Tạm dừng game
            SFXManager.instance.StopAllSFX();
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
        }
        else
        {
            // Tiếp tục game
            pauseScreen.SetActive(false);
            if (levelUpPanel.activeSelf == false)
            {
                Time.timeScale = 1f;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    // Cập nhật hiển thị đồng hồ
    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    // Tăng số lượng kẻ thù đã xuất hiện
    public void IncrementEnemiesSpawned()
    {
        enemiesSpawned++;
    }

    // Phương thức tăng số lượng kẻ thù bị tiêu diệt
    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated++;
    }

    // Tăng số lượng đồng xu đã thu thập
    public void IncrementCoinsCollected()
    {
        coinsCollected++;
    }
}
