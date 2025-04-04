using System.Net.NetworkInformation;
using UnityEngine;

public class OrbitingShuriken : Weapon
{
    public static OrbitingShuriken instance;

    [Space(10)]
    // Tham chiếu đến Damager của kẻ thù để xử lý sát thương.
    public EnemyDamager damager;
    // Prefab của shuriken sẽ được tạo ra.
    public Transform projectile;
    // Đối tượng chứa shuriken, dùng để quay vòng.
    public Transform holder;

    [Space(10)]
    // Tốc độ quay của shuriken.
    public float rotateSpeed = 180f;
    // Thời gian giữa mỗi lần shuriken xuất hiện.
    public float timeBetweenSpawn = 5f;

    // Bộ đếm thời gian cho việc sinh shuriken.
    private float spawnCounter;

    // Cấp độ của OrbitingShuriken.
    public int orbitingShurikenLevel;

    void Awake()
    {
        // Đảm bảo rằng chỉ có một instance duy nhất của OrbitingShuriken.
        instance = this;
    }

    void Start()
    {
        // Khởi tạo các chỉ số vũ khí khi bắt đầu trò chơi.
        SetStats();
    }

    void Update()
    {
        // Quay đối tượng holder.
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime * stats[weaponLevel].speed));

        // Đếm ngược thời gian để sinh shuriken.
        spawnCounter -= Time.deltaTime;

        // Nếu bộ đếm thời gian về 0, tiến hành sinh shuriken.
        if (spawnCounter <= 0)
        {
            spawnCounter = timeBetweenSpawn;

            // Sinh shuriken xung quanh holder.
            for (int i = 0; i < stats[weaponLevel].amount; i++)
            {
                // Tính góc để đặt vị trí shuriken.
                float rot = (360f / stats[weaponLevel].amount) * i;

                // Tạo và kích hoạt shuriken.
                Instantiate(projectile, projectile.position, Quaternion.Euler(0f, 0f, rot), holder).gameObject.SetActive(true);
            }
        }

        // Cập nhật lại chỉ số vũ khí nếu có sự thay đổi.
        if (statsUpdated == true)
        {
            statsUpdated = false;

            // Cập nhật các chỉ số vũ khí.
            SetStats();
        }

        // Cập nhật cấp độ OrbitingShuriken.
        orbitingShurikenLevel = weaponLevel;
    }

    // Cập nhật các chỉ số vũ khí.
    public void SetStats()
    {
        // Cập nhật chỉ số sát thương của Damager.
        damager.damageAmount = stats[weaponLevel].damage;
        // Cập nhật kích thước cho phạm vi shuriken.
        transform.localScale = Vector3.one * stats[weaponLevel].range;
        // Cập nhật thời gian giữa mỗi lần sinh shuriken.
        timeBetweenSpawn = stats[weaponLevel].timeBetweenAttacks;
        // Cập nhật thời gian sống của shuriken.
        damager.lifeTime = stats[weaponLevel].duration;
    }
}
