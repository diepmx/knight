using UnityEngine;

public class Moltenspear : Weapon
{
    public static Moltenspear instance;

    [Space(10)]
    public EnemyDamager damager; // X? l� s�t th??ng
    public GameObject geyserPrefab; // Prefab c?a c?t n??c
    public LayerMask whatIsEnemy; // X�c ??nh k? th�

    [Space(10)]
    public float weaponRange; // Ph?m vi xu?t hi?n c?a c?t n??c

    private float shotCounter; // B? ??m th?i gian gi?a c�c l?n t?o c?t n??c
    public int geyserLevel; // C?p ?? c?a k? n?ng

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

            // T?o c?t n??c t?i v? tr� ng?u nhi�n g?n ng??i ch?i
            Vector3 spawnPosition = transform.position + (Vector3)Random.insideUnitCircle * weaponRange;
            GameObject geyser = Instantiate(geyserPrefab, spawnPosition, Quaternion.identity);
            geyser.SetActive(true);

            // C�i ??t c�c th�ng s? cho c?t n??c
            WaterGeyserWeapon geyserBehavior = geyser.GetComponent<WaterGeyserWeapon>();
            if (geyserBehavior != null)
            {
                geyserBehavior.Initialize(damager, stats[weaponLevel].duration, stats[weaponLevel].range);
            }

            // Ph�t �m thanh v� hi?u ?ng
            SFXManager.instance.PlaySFXPitched(9);
            CameraShake.instance.ShakeIt(0.2f, 0.3f);
        }

        geyserLevel = weaponLevel;
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        shotCounter = 0f;
    }
}
