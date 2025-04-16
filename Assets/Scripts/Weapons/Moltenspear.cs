using UnityEngine;

public class Moltenspear : Weapon
{
    public static Moltenspear instance;

    [Space(10)]
    public EnemyDamager damager; // X? lý sát th??ng
    public GameObject spearPrefab; // Prefab c?a ng?n giáo dung nham
    public LayerMask whatIsEnemy; // Xác ??nh k? thù

    [Space(10)]
    public float weaponRange; // Ph?m vi phóng c?a ng?n giáo
    public float spearSpeed; // T?c ?? bay c?a ng?n giáo

    private float shotCounter; // B? ??m th?i gian gi?a các l?n phóng giáo
    public int moltenspearLevel; // C?p ?? c?a k? n?ng

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

            // T?o ng?n giáo dung nham t?i v? trí c?a ng??i ch?i
            Vector3 spawnPosition = transform.position;
            GameObject spear = Instantiate(spearPrefab, spawnPosition, Quaternion.identity);
            spear.SetActive(true);

            // Cài ??t các thông s? cho ng?n giáo
            MoltenspearProjectile spearProjectile = spear.GetComponent<MoltenspearProjectile>();
            if (spearProjectile != null)
            {
                spearProjectile.Initialize(damager, spearSpeed, stats[weaponLevel].range, stats[weaponLevel].damage);
            }

            // Phát âm thanh và hi?u ?ng
            SFXManager.instance.PlaySFXPitched(10);
            CameraShake.instance.ShakeIt(0.1f, 0.2f);
        }

        moltenspearLevel = weaponLevel;
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        shotCounter = 0f;
    }
}
