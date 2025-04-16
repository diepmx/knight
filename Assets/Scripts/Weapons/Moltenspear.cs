using UnityEngine;

public class Moltenspear : Weapon
{
    public static Moltenspear instance;

    [Space(10)]
    public EnemyDamager damager; // X? l� s�t th??ng
    public GameObject spearPrefab; // Prefab c?a ng?n gi�o dung nham
    public LayerMask whatIsEnemy; // X�c ??nh k? th�

    [Space(10)]
    public float weaponRange; // Ph?m vi ph�ng c?a ng?n gi�o
    public float spearSpeed; // T?c ?? bay c?a ng?n gi�o

    private float shotCounter; // B? ??m th?i gian gi?a c�c l?n ph�ng gi�o
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

            // T?o ng?n gi�o dung nham t?i v? tr� c?a ng??i ch?i
            Vector3 spawnPosition = transform.position;
            GameObject spear = Instantiate(spearPrefab, spawnPosition, Quaternion.identity);
            spear.SetActive(true);

            // C�i ??t c�c th�ng s? cho ng?n gi�o
            MoltenspearProjectile spearProjectile = spear.GetComponent<MoltenspearProjectile>();
            if (spearProjectile != null)
            {
                spearProjectile.Initialize(damager, spearSpeed, stats[weaponLevel].range, stats[weaponLevel].damage);
            }

            // Ph�t �m thanh v� hi?u ?ng
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
