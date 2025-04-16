using UnityEngine;

public class Portal : Weapon
{
    public static Portal instance;

    [Space(10)]
    // Tham chi?u ??n component Damager ?? x? l� s�t th??ng ho?c hi?u ?ng.
    public EnemyDamager damager;
    // Tham chi?u ??n prefab ??i di?n cho c?ng d?ch chuy?n.
    public GameObject portalPrefab;

    [Space(10)]
    // LayerMask ?? x�c ??nh nh?ng ??i t??ng n�o c� th? t??ng t�c v?i c?ng.
    public LayerMask whatIsInteractable;

    [Space(10)]
    // Ph?m vi ho?t ??ng c?a c?ng.
    public float portalRange;

    // B? ??m th?i gian cho c�c l?n k�ch ho?t c?ng.
    private float activationCounter;

    // M?c c?p ?? c?a Portal.
    public int portalLevel;

    void Awake()
    {
        // ??m b?o r?ng ch? c� m?t instance duy nh?t c?a Portal.
        instance = this;
    }

    void Start()
    {
        // Kh?i t?o c�c ch? s? khi b?t ??u tr� ch?i.
        SetStats();
    }

    void Update()
    {
        // Ki?m tra xem c� c?n c?p nh?t l?i c�c ch? s? kh�ng.
        if (statsUpdated == true)
        {
            statsUpdated = false;
            SetStats();
        }

        // ??m ng??c th?i gian cho l?n k�ch ho?t ti?p theo.
        activationCounter -= Time.deltaTime;

        // N?u ?� ??n th?i ?i?m k�ch ho?t c?ng.
        if (activationCounter <= 0)
        {
            // ??t l?i b? ??m th?i gian.
            activationCounter = stats[weaponLevel].timeBetweenAttacks;

            // Ki?m tra c�c ??i t??ng trong ph?m vi.
            Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, portalRange * stats[weaponLevel].range, whatIsInteractable);
            if (interactables.Length > 0)
            {
                // T?o c?ng t?i v? tr� ng?u nhi�n trong ph?m vi.
                Vector3 targetPosition = interactables[Random.Range(0, interactables.Length)].transform.position;
                Instantiate(portalPrefab, targetPosition, Quaternion.identity).gameObject.SetActive(true);

                // Ph�t �m thanh khi t?o c?ng.
                SFXManager.instance.PlaySFXPitched(7);

                // L�m rung camera khi c?ng ???c k�ch ho?t.
                CameraShake.instance.ShakeIt(0.15f, 0.25f);
            }
        }

        // C?p nh?t c?p ?? c?a Portal.
        portalLevel = weaponLevel;
    }

    // C?p nh?t c�c ch? s? khi c?p ?? thay ??i.
    void SetStats()
    {
        // C?p nh?t c�c ch? s? c?a damager (c?ng).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        // ??t l?i b? ??m th?i gian k�ch ho?t.
        activationCounter = 0f;
    }
}
