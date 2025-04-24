using UnityEngine;

public class Rocks : Weapon
{
    public static Rocks instance;

    [Space(10)]
    // Tham chiếu đến component Damager để xử lý sát thương.
    public EnemyDamager damager;
    // Tham chiếu đến prefab ProjectileWeapon, đại diện cho quả cầu lửa.
    public ProjectileWeapon projectile;

    [Space(10)]
    // LayerMask để xác định những đối tượng nào được coi là kẻ thù.
    public LayerMask whatIsEnemy;

    [Space(10)]
    // Phạm vi tấn công của quả cầu lửa.
    public float weaponRange;

    // Bộ đếm thời gian cho các đợt tấn công.
    private float shotCounter;

    // Mức cấp độ của Fireball.
    public int fireballLevel;

    void Awake()
    {
        // Đảm bảo rằng chỉ có một instance duy nhất của Fireball.
        instance = this;
    }

    void Start()
    {
        // Khởi tạo các chỉ số vũ khí khi bắt đầu trò chơi.
        SetStats();
    }

    void Update()
    {
        // Kiểm tra xem có cần cập nhật lại các chỉ số vũ khí không.
        if (statsUpdated == true)
        {
            statsUpdated = false;
            // Cập nhật các chỉ số vũ khí.
            SetStats();
        }

        // Đếm ngược thời gian cho các đợt tấn công.
        shotCounter -= Time.deltaTime;

        // Kiểm tra nếu bộ đếm thời gian tấn công đã hết, thực hiện tấn công.
        if (shotCounter <= 0)
        {
            // Đặt lại bộ đếm thời gian tấn công.
            shotCounter = stats[weaponLevel].timeBetweenAttacks;

            // Kiểm tra các kẻ thù trong phạm vi.
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                // Tấn công mỗi kẻ thù trong phạm vi.
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    // Chọn ngẫu nhiên một kẻ thù trong phạm vi.
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;

                    // Tính toán hướng bắn quả cầu lửa về phía kẻ thù.
                    Vector3 direction = targetPosition - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle -= 90; // Điều chỉnh góc cho đúng hướng.

                    // Cài đặt góc quay của projectile (quả cầu lửa) và tạo mới nó.
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
                }

                // Phát âm thanh khi thực hiện tấn công.
                SFXManager.instance.PlaySFXPitched(5);
            }
        }

        // Cập nhật cấp độ của Fireball.
        fireballLevel = weaponLevel;
    }

    // Cập nhật các chỉ số vũ khí khi cấp độ thay đổi.
    void SetStats()
    {
        // Cập nhật các chỉ số của damager (quả cầu lửa).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // Đặt lại bộ đếm thời gian tấn công.
        shotCounter = 0f;
        // Cập nhật tốc độ di chuyển của quả cầu lửa.
        projectile.moveSpeed = stats[weaponLevel].speed;
    }
}