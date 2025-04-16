using UnityEngine;

public class Tornado : Weapon
{
    public static Tornado instance;

    [Space(10)]
    public EnemyDamager damager; // X? l� s�t th??ng
    public GameObject tornadoPrefab; // Prefab c?a c?n l?c xo�y
    public LayerMask whatIsEnemy; // X�c ??nh k? th�

    [Space(10)]
    public float weaponRange; // Ph?m vi ?nh h??ng c?a c?n l?c
    public float tornadoSpeed; // T?c ?? di chuy?n c?a c?n l?c

    private float shotCounter; // B? ??m th?i gian gi?a c�c l?n t?o l?c
    public int tornadoLevel; // C?p ?? c?a k? n?ng

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

            // T?o c?n l?c xo�y t?i v? tr� ng?u nhi�n g?n ng??i ch?i
            Vector3 spawnPosition = transform.position + (Vector3)Random.insideUnitCircle * weaponRange;
            GameObject tornado = Instantiate(tornadoPrefab, spawnPosition, Quaternion.identity);
            tornado.SetActive(true);

            // C�i ??t c�c th�ng s? cho c?n l?c
            TornadoWeapon tornadoBehavior = tornado.GetComponent<TornadoWeapon>();
            if (tornadoBehavior != null)
            {
                tornadoBehavior.Initialize(damager, tornadoSpeed, stats[weaponLevel].duration, stats[weaponLevel].range);
            }

            // Ph�t �m thanh v� hi?u ?ng
            SFXManager.instance.PlaySFXPitched(8);
            CameraShake.instance.ShakeIt(0.2f, 0.3f);
        }

        tornadoLevel = weaponLevel;
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        shotCounter = 0f;
    }
}
