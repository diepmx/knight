using UnityEngine;

public class Rocks : Weapon
{
    public static Rocks instance;

    [Space(10)]
    // Tham chi?u ??n component Damager ?? x? lý sát th??ng.
    public EnemyDamager damager;
    // Tham chi?u ??n prefab ??i di?n cho t?ng ?á.
    public GameObject rockPrefab;

    [Space(10)]
    // LayerMask ?? xác ??nh nh?ng ??i t??ng nào ???c coi là k? thù.
    public LayerMask whatIsEnemy;

    [Space(10)]
    // Ph?m vi t?n công c?a t?ng ?á.
    public float weaponRange;

    // B? ??m th?i gian cho các ??t t?n công.
    private float shotCounter;

    // M?c c?p ?? c?a Rocks.
    public int rocksLevel;

    void Awake()
    {
        // ??m b?o r?ng ch? có m?t instance duy nh?t c?a Rocks.
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
                    // Ch?n ng?u nhiên m?t v? trí g?n k? thù và t?o ra t?ng ?á.
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                    targetPosition.y += 2f; // ??t t?ng ?á r?i t? trên cao.
                    Instantiate(rockPrefab, targetPosition, Quaternion.identity).gameObject.SetActive(true);
                }

                // Phát âm thanh khi th?c hi?n t?n công.
                SFXManager.instance.PlaySFXPitched(4);

                // Làm rung camera khi t?n công.
                CameraShake.instance.ShakeIt(0.2f, 0.3f);
            }
        }

        // C?p nh?t c?p ?? c?a Rocks.
        rocksLevel = weaponLevel;
    }

    // C?p nh?t các ch? s? v? khí khi c?p ?? thay ??i.
    void SetStats()
    {
        // C?p nh?t các ch? s? c?a damager (t?ng ?á).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // ??t l?i b? ??m th?i gian t?n công.
        shotCounter = 0f;
    }
}
