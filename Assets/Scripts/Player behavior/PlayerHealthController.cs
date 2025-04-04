using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    // Singleton instance.
    public static PlayerHealthController instance;

    [Space(10)]
    // Slider UI cho thanh máu.
    public Slider healthSlider;

    [Space(10)]
    // Giá trị máu hiện tại của người chơi.
    public float currentHealth;
    // Giá trị máu tối đa của người chơi.
    public float maxHealth = 100f;

    [Space(10)]
    // Tổng lượng sát thương đã nhận.
    public float totalDamage = 0f;

    [Space(10)]
    // Hiệu ứng khi người chơi chết (ví dụ: hiệu ứng nổ hoặc mờ dần).
    public GameObject deathEffect;

    void Awake()
    {
        // Đảm bảo chỉ có một instance của PlayerHealthController.
        instance = this;
    }

    void Start()
    {
        // Khởi tạo máu hiện tại bằng máu tối đa khi bắt đầu.
        currentHealth = maxHealth;

        // Đặt giá trị tối đa cho thanh máu.
        healthSlider.maxValue = maxHealth;
        // Đặt giá trị hiện tại của thanh máu.
        healthSlider.value = currentHealth;
    }

    public void Update()
    {
        // Xử lý hồi phục máu.
        Regeneration();
    }

    // Phương thức xử lý khi người chơi nhận sát thương.
    public void TakeDamage(float damageToTake)
    {
        // Giảm máu theo lượng sát thương nhận vào.
        currentHealth -= damageToTake;
        // Cập nhật tổng lượng sát thương đã nhận.
        totalDamage += damageToTake;

        // Kiểm tra nếu máu giảm xuống 0 hoặc dưới 0.
        if (currentHealth <= 0)
        {
            // Tắt đối tượng người chơi.
            gameObject.SetActive(false);

            // Gọi phương thức game over.
            GameController.instance.GameOver();
            // Gây rung lắc camera khi chết.
            CameraShake.instance.ShakeIt(0.5f, 0.2f);
            // Tạo hiệu ứng chết tại vị trí của người chơi.
            Instantiate(deathEffect, transform.position, transform.rotation);
        }

        // Cập nhật giá trị thanh máu sau khi nhận sát thương.
        healthSlider.value = currentHealth;
    }

    // Phương thức hồi phục máu theo thời gian.
    public void Regeneration()
    {
        // Nếu máu chưa đầy đủ, bắt đầu hồi phục.
        if (currentHealth < maxHealth)
        {
            // Tăng máu dần theo thời gian.
            currentHealth += 1 * Time.deltaTime;
            // Cập nhật giá trị thanh máu sau khi hồi phục.
            healthSlider.value = currentHealth;
        }
    }
}
