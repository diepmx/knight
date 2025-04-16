using UnityEngine;

public class Explosion : Weapon
{
    public static Explosion instance;

    [Space(10)]
    public EnemyDamager damager; // X? lý sát th??ng
    public GameObject explosionEffect; // Hi?u ?ng n?
    public LayerMask whatIsEnemy; // Xác ??nh k? thù

    [Space(10)]
    public float weaponRange; // Ph?m vi v? n?

    private float shotCounter; // B? ??m th?i gian gi?a các v? n?
    public int explosionLevel; // C?p ?? c?a k? n?ng

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetStats();
    }

    void Update()
    {
        if (statsUpdated)
        {
            statsUpdated = false;
            SetStats();
        }

        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0)
        {
            shotCounter = stats[weaponLevel].timeBetweenAttacks;

            // Tìm t?t c? k? thù trong ph?m vi
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    // Ch?n ng?u nhiên m?t k? thù và t?o v? n?
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                    GameObject explosion = Instantiate(explosionEffect, targetPosition, Quaternion.identity);
                    explosion.SetActive(true);
                }

                // Phát âm thanh v? n?
                SFXManager.instance.PlaySFXPitched(7);

                // Làm rung camera
                CameraShake.instance.ShakeIt(0.2f, 0.3f);
            }
        }

        explosionLevel = weaponLevel;
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        shotCounter = 0f;
    }
}
