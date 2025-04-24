using UnityEngine;

public class Tonado : Weapon
{
    public static Tonado instance;

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
        if (statsUpdated == true)
        {
            statsUpdated = false;
            SetStats();
        }

        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0)
        {
            shotCounter = stats[weaponLevel].timeBetweenAttacks;

            for (int i = 0; i < stats[weaponLevel].amount; i++)
            {
                // Tạo một hướng ngẫu nhiên
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                // Tạo projectile tại vị trí hiện tại
                GameObject newProjectile = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
                newProjectile.SetActive(true);

                // Đảm bảo projectile di chuyển theo hướng ngẫu nhiên
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = randomDirection * stats[weaponLevel].speed;
                }
                else
                {
                    Debug.LogError("Rigidbody2D is missing on the Tornado prefab!");
                }
            }

            SFXManager.instance.PlaySFXPitched(5);
        }

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