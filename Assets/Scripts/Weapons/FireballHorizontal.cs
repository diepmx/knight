using UnityEngine;

public class FireballHorizontal : Weapon
{
    [Space(10)]
    public EnemyDamager damager;
    public ProjectileWeapon projectile;

    [Space(10)]
    public LayerMask whatIsEnemy;

    [Space(10)]
    public float weaponRange;

    private float shotCounter;
    public int fireballLevel;

    void Start()
    {
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

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                    Vector3 direction = targetPosition - transform.position;

                    // Tính toán góc xoay dựa trên hướng di chuyển
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Tạo projectile tại vị trí hiện tại và xoay theo góc tính toán
                    GameObject newProjectile = Instantiate(projectile.gameObject, transform.position, Quaternion.Euler(0, 0, angle));
                    newProjectile.SetActive(true);
                    angle -= 0;

                    // Áp dụng góc xoay
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);

                    // Đảm bảo projectile di chuyển theo hướng đã tính toán
                    ProjectileWeapon projectileComponent = newProjectile.GetComponent<ProjectileWeapon>();
                    if (projectileComponent != null)
                    {
                        projectileComponent.moveSpeed = stats[weaponLevel].speed;

                        // Cập nhật hướng di chuyển của projectile
                        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.velocity = direction.normalized * projectileComponent.moveSpeed;
                        }
                    }
                }

                SFXManager.instance.PlaySFXPitched(5);
            }
        }

        fireballLevel = weaponLevel;
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        shotCounter = 0f;
        projectile.moveSpeed = stats[weaponLevel].speed;
    }
}
