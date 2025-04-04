using UnityEngine;

public class DangerZone : Weapon
{
    public static DangerZone instance;

    [Space(10)]
    // Tham chiếu đến component Damager, quản lý sát thương đối với kẻ thù trong vùng nguy hiểm
    public EnemyDamager damager;

    // Thời gian giữa các lần spawn (tạo ra) các damager (kẻ gây sát thương)
    private float spawnTime;
    // Bộ đếm thời gian cho việc spawn damager
    private float spawnCounter;

    // Mức cấp độ của DangerZone
    public int dangerZoneLevel;

    void Awake()
    {
        // Đảm bảo rằng chỉ có một instance duy nhất của DangerZone
        instance = this;
    }

    void Start()
    {
        // Khởi tạo các chỉ số vũ khí ngay khi bắt đầu trò chơi
        SetStats();
    }

    void Update()
    {
        // Kiểm tra xem có cần cập nhật lại các chỉ số vũ khí không
        if (statsUpdated == true)
        {
            statsUpdated = false;
            // Cập nhật các chỉ số vũ khí
            SetStats();
        }

        // Đếm ngược thời gian cho việc spawn damager
        spawnCounter -= Time.deltaTime;

        // Khi bộ đếm thời gian về 0, spawn một damager mới
        if (spawnCounter <= 0f)
        {
            // Đặt lại bộ đếm thời gian spawn
            spawnCounter = spawnTime;
            // Tạo ra một damager mới tại vị trí đã chỉ định
            Instantiate(damager, damager.transform.position, Quaternion.identity, transform).gameObject.SetActive(true);
        }

        // Cập nhật cấp độ của DangerZone
        dangerZoneLevel = weaponLevel;
    }

    // Cập nhật các chỉ số của vũ khí DangerZone
    void SetStats()
    {
        // Cập nhật các chỉ số của damager
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.timeBetweenDamage = stats[weaponLevel].speed;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // Cập nhật thời gian giữa các lần spawn
        spawnTime = stats[weaponLevel].timeBetweenAttacks;
    }
}
