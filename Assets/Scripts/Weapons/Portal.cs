using UnityEngine;

public class Portal : Weapon
{
    public static Portal instance;

    [Space(10)]
    // Tham chi?u ??n component Damager ?? x? lý sát th??ng ho?c hi?u ?ng.
    public EnemyDamager damager;
    // Tham chi?u ??n prefab ??i di?n cho c?ng d?ch chuy?n.
    public GameObject portalPrefab;

    [Space(10)]
    // LayerMask ?? xác ??nh nh?ng ??i t??ng nào có th? t??ng tác v?i c?ng.
    public LayerMask whatIsInteractable;

    [Space(10)]
    // Ph?m vi ho?t ??ng c?a c?ng.
    public float portalRange;

    // B? ??m th?i gian cho các l?n kích ho?t c?ng.
    private float activationCounter;

    // M?c c?p ?? c?a Portal.
    public int portalLevel;

    void Awake()
    {
        // ??m b?o r?ng ch? có m?t instance duy nh?t c?a Portal.
        instance = this;
    }

    void Start()
    {
        // Kh?i t?o các ch? s? khi b?t ??u trò ch?i.
        SetStats();
    }

    void Update()
    {
        // Ki?m tra xem có c?n c?p nh?t l?i các ch? s? không.
        if (statsUpdated == true)
        {
            statsUpdated = false;
            SetStats();
        }

        // ??m ng??c th?i gian cho l?n kích ho?t ti?p theo.
        activationCounter -= Time.deltaTime;

        // N?u ?ã ??n th?i ?i?m kích ho?t c?ng.
        if (activationCounter <= 0)
        {
            // ??t l?i b? ??m th?i gian.
            activationCounter = stats[weaponLevel].timeBetweenAttacks;

            // Ki?m tra các ??i t??ng trong ph?m vi.
            Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, portalRange * stats[weaponLevel].range, whatIsInteractable);
            if (interactables.Length > 0)
            {
                // T?o c?ng t?i v? trí ng?u nhiên trong ph?m vi.
                Vector3 targetPosition = interactables[Random.Range(0, interactables.Length)].transform.position;
                Instantiate(portalPrefab, targetPosition, Quaternion.identity).gameObject.SetActive(true);

                // Phát âm thanh khi t?o c?ng.
                SFXManager.instance.PlaySFXPitched(7);

                // Làm rung camera khi c?ng ???c kích ho?t.
                CameraShake.instance.ShakeIt(0.15f, 0.25f);
            }
        }

        // C?p nh?t c?p ?? c?a Portal.
        portalLevel = weaponLevel;
    }

    // C?p nh?t các ch? s? khi c?p ?? thay ??i.
    void SetStats()
    {
        // C?p nh?t các ch? s? c?a damager (c?ng).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // ??t l?i b? ??m th?i gian kích ho?t.
        activationCounter = 0f;
    }
}
