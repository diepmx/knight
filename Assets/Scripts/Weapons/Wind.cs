using UnityEngine;

public class Wind : Weapon
{
    public static Wind instance;

    [Space(10)]
    // Tham chi?u ??n component Damager ?? x? l� s�t th??ng.
    public EnemyDamager damager;
    // Tham chi?u ??n prefab ??i di?n cho lu?ng gi�.
    public ProjectileWeapon windProjectile;

    [Space(10)]
    // LayerMask ?? x�c ??nh nh?ng ??i t??ng n�o ???c coi l� k? th�.
    public LayerMask whatIsEnemy;

    [Space(10)]
    // Ph?m vi t?n c�ng c?a lu?ng gi�.
    public float weaponRange;

    // B? ??m th?i gian cho c�c ??t t?n c�ng.
    private float shotCounter;

    // M?c c?p ?? c?a Wind.
    public int windLevel;

    void Awake()
    {
        // ??m b?o r?ng ch? c� m?t instance duy nh?t c?a Wind.
        instance = this;
    }

    void Start()
    {
        // Kh?i t?o c�c ch? s? v? kh� khi b?t ??u tr� ch?i.
        SetStats();
    }

    void Update()
    {
        // Ki?m tra xem c� c?n c?p nh?t l?i c�c ch? s? v? kh� kh�ng.
        if (statsUpdated == true)
        {
            statsUpdated = false;
            SetStats();
        }

        // ??m ng??c th?i gian cho c�c ??t t?n c�ng.
        shotCounter -= Time.deltaTime;

        // Ki?m tra n?u b? ??m th?i gian t?n c�ng ?� h?t, th?c hi?n t?n c�ng.
        if (shotCounter <= 0)
        {
            // ??t l?i b? ??m th?i gian t?n c�ng.
            shotCounter = stats[weaponLevel].timeBetweenAttacks;

            // Ki?m tra c�c k? th� trong ph?m vi.
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, whatIsEnemy);
            if (enemies.Length > 0)
            {
                // T?n c�ng m?i k? th� trong ph?m vi.
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    // Ch?n ng?u nhi�n m?t k? th� trong ph?m vi v� t?o ra lu?ng gi�.
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                    Instantiate(windProjectile, targetPosition, Quaternion.identity).gameObject.SetActive(true);
                }

                // Ph�t �m thanh khi th?c hi?n t?n c�ng.
                SFXManager.instance.PlaySFXPitched(8);

                // L�m rung camera khi t?n c�ng.
                CameraShake.instance.ShakeIt(0.1f, 0.2f);
            }
        }

        // C?p nh?t c?p ?? c?a Wind.
        windLevel = weaponLevel;
    }

    // C?p nh?t c�c ch? s? v? kh� khi c?p ?? thay ??i.
    void SetStats()
    {
        // C?p nh?t c�c ch? s? c?a damager (lu?ng gi�).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // ??t l?i b? ??m th?i gian t?n c�ng.
        shotCounter = 0f;
    }
}
