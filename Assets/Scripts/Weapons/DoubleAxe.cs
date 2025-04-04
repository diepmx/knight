using UnityEngine;

public class DoubleAxe : Weapon
{
    public static DoubleAxe instance;

    [Space(10)]
    // Tham chiếu đến component Damager, quản lý sát thương đối với kẻ thù khi ném rìu.
    public EnemyDamager damager;

    // Bộ đếm thời gian cho các đợt ném rìu
    private float throwCounter;

    // Mức cấp độ của DoubleAxe
    public int doubleAxeLevel;

    void Awake()
    {
        // Đảm bảo rằng chỉ có một instance duy nhất của DoubleAxe.
        instance = this;
    }

    void Start()
    {
        // Khởi tạo các chỉ số vũ khí khi bắt đầu trò chơi
        SetStats();
    }

    void Update()
    {
        // Kiểm tra xem có cần cập nhật lại các chỉ số vũ khí không
        if (statsUpdated == true)
        {
            statsUpdated = false;
            // Cập nhật các chỉ số vũ khí
            SetStats();
        }

        // Đếm ngược thời gian cho việc ném rìu.
        throwCounter -= Time.deltaTime;

        // Kiểm tra nếu thời gian đếm ngược kết thúc, ném rìu.
        if (throwCounter <= 0)
        {
            // Đặt lại bộ đếm thời gian ném rìu.
            throwCounter = stats[weaponLevel].timeBetweenAttacks;

            // Thực hiện ném rìu theo số lượng xác định.
            for (int i = 0; i < stats[weaponLevel].amount; i++)
            {
                // Tạo một damager mới (rìu) tại vị trí của vũ khí.
                Instantiate(damager, damager.transform.position, damager.transform.rotation).gameObject.SetActive(true);

                // Phát âm thanh khi ném rìu.
                SFXManager.instance.PlaySFXPitched(4);
            }
        }

        // Cập nhật cấp độ của DoubleAxe
        doubleAxeLevel = weaponLevel;
    }

    // Cập nhật các chỉ số vũ khí khi cấp độ thay đổi.
    void SetStats()
    {
        // Cập nhật các chỉ số cho damager (rìu).
        damager.damageAmount = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;

        // Đặt lại bộ đếm thời gian ném rìu.
        throwCounter = 0f;
    }
}
