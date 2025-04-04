using UnityEngine;

public class Dagger : Weapon
{
    public static Dagger instance;

    [Space(10)]
    // Tham chiếu đến component Damager, quản lý sát thương đối với kẻ thù
    public EnemyDamager damager;
    // Tham chiếu đến projectile (phi tiêu) của vũ khí
    public ProjectileWeapon projectile;

    [Space(10)]
    // Layer mask để phát hiện kẻ thù
    public LayerMask whatIsEnemy;

    [Space(10)]
    // Phạm vi của vũ khí
    public float weaponRange;

    // Biến đếm thời gian cho các cuộc tấn công
    private float shotCounter;

    // Mức cấp độ của dao
    public int daggerLevel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Khởi tạo các chỉ số vũ khí
        SetStats();
    }

    void Update()
    {
        // Kiểm tra xem có cần cập nhật lại các chỉ số không
        if (statsUpdated == true)
        {
            statsUpdated = false;
            // Cập nhật các chỉ số
            SetStats();
        }

        // Đếm ngược thời gian giữa các lần tấn công
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0)
        {
            // Đặt lại bộ đếm thời gian tấn công
            shotCounter = stats[weaponLevel].timeBetweenAttacks;

            // Phát hiện các kẻ thù trong phạm vi
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                // Tấn công mỗi kẻ thù trong phạm vi
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;

                    // Tính toán hướng phi tiêu
                    Vector3 direction = targetPosition - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle -= 90;

                    // Đặt góc xoay của phi tiêu và tạo đối tượng phi tiêu
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
                }

                // Phát âm thanh khi tấn công
                SFXManager.instance.PlaySFXPitched(2);
            }

        }

        daggerLevel = weaponLevel;
    }

    // Cập nhật các chỉ số của vũ khí
    void SetStats()
    {
        // Cập nhật các chỉ số cho Damager (kẻ gây sát thương)
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // Đặt lại bộ đếm thời gian tấn công
        shotCounter = 0f;
        // Cập nhật tốc độ phi tiêu
        projectile.moveSpeed = stats[weaponLevel].speed;
    }
}
