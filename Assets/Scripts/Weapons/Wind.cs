using UnityEngine;

public class Wind : Weapon
{
    public static Wind instance;

    [Space(10)]
    // Tham chi?u ??n component Damager ?? x? lý sát th??ng.
    public EnemyDamager damager;
    // Tham chi?u ??n prefab ??i di?n cho lu?ng gió.
    public ProjectileWeapon windProjectile;

    [Space(10)]
    // LayerMask ?? xác ??nh nh?ng ??i t??ng nào ???c coi là k? thù.
    public LayerMask whatIsEnemy;

    [Space(10)]
    // Ph?m vi t?n công c?a lu?ng gió.
    public float weaponRange;

    // B? ??m th?i gian cho các ??t t?n công.
    private float shotCounter;

    // M?c c?p ?? c?a Wind.
    public int windLevel;

    void Awake()
    {
        // ??m b?o r?ng ch? có m?t instance duy nh?t c?a Wind.
        instance = this;
    }

    void Start()
    {
        // Kh?i t?o các ch? s? v? khí khi b?t ??u trò ch?i.
        SetStats();
    }

    void Update()
    {
        // Ki?m tra xem có c?n c?p nh?t l?i các ch? s? v? khí không.
        if (statsUpdated == true)
        {
            statsUpdated = false;
            SetStats();
        }

        // ??m ng??c th?i gian cho các ??t t?n công.
        shotCounter -= Time.deltaTime;

        // Ki?m tra n?u b? ??m th?i gian t?n công ?ã h?t, th?c hi?n t?n công.
        if (shotCounter <= 0)
        {
            // ??t l?i b? ??m th?i gian t?n công.
            shotCounter = stats[weaponLevel].timeBetweenAttacks;

            // Ki?m tra các k? thù trong ph?m vi.
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                // T?n công m?i k? thù trong ph?m vi.
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    // Ch?n ng?u nhiên m?t k? thù trong ph?m vi và t?o ra lu?ng gió.
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                    Instantiate(windProjectile, targetPosition, Quaternion.identity).gameObject.SetActive(true);
                }

                // Phát âm thanh khi th?c hi?n t?n công.
                SFXManager.instance.PlaySFXPitched(8);

                // Làm rung camera khi t?n công.
                CameraShake.instance.ShakeIt(0.1f, 0.2f);
            }
        }

        // C?p nh?t c?p ?? c?a Wind.
        windLevel = weaponLevel;
    }

    // C?p nh?t các ch? s? v? khí khi c?p ?? thay ??i.
    void SetStats()
    {
        // C?p nh?t các ch? s? c?a damager (lu?ng gió).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // ??t l?i b? ??m th?i gian t?n công.
        shotCounter = 0f;
    }
}
