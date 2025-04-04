using UnityEngine;

public class Lightning : Weapon
{
    public static Lightning instance;

    [Space(10)]
    // Tham chiếu đến component Damager để xử lý sát thương.
    public EnemyDamager damager;
    // Tham chiếu đến prefab ProjectileWeapon, đại diện cho tia sét.
    public ProjectileWeapon projectile;

    [Space(10)]
    // LayerMask để xác định những đối tượng nào được coi là kẻ thù.
    public LayerMask whatIsEnemy;

    [Space(10)]
    // Phạm vi tấn công của tia sét.
    public float weaponRange;

    // Bộ đếm thời gian cho các đợt tấn công.
    private float shotCounter;

    // Mức cấp độ của Lightning.
    public int lightningLevel;

    void Awake()
    {
        // Đảm bảo rằng chỉ có một instance duy nhất của Lightning.
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
                    // Chọn ngẫu nhiên một kẻ thù trong phạm vi và tạo ra projectile.
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                    Instantiate(projectile, targetPosition, Quaternion.identity).gameObject.SetActive(true);
                }

                // Phát âm thanh khi thực hiện tấn công.
                SFXManager.instance.PlaySFXPitched(6);

                // Làm rung camera khi tấn công.
                CameraShake.instance.ShakeIt(0.1f, 0.2f);
            }
        }

        // Cập nhật cấp độ của Lightning.
        lightningLevel = weaponLevel;
    }

    // Cập nhật các chỉ số vũ khí khi cấp độ thay đổi.
    void SetStats()
    {
        // Cập nhật các chỉ số của damager (tia sét).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // Đặt lại bộ đếm thời gian tấn công.
        shotCounter = 0f;
    }
}
