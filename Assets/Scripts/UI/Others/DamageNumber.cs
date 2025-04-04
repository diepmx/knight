using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [Space(10)]
    // Tham chiếu đến thành phần TextMeshPro để hiển thị số sát thương
    public TMP_Text damageText;

    [Space(10)]
    // Thời gian sống của số sát thương trước khi được trả lại vào pool
    public float lifetime;
    // Tốc độ di chuyển của số sát thương theo chiều dọc
    public float floatSpeed = 2f;

    private float lifeCounter;

    // Phương thức Start được gọi khi script bắt đầu
    void Start()
    {
        // Khởi tạo bộ đếm thời gian sống của số sát thương
        lifeCounter = lifetime;
    }

    // Phương thức Update được gọi mỗi khung hình
    void Update()
    {
        // Giảm thời gian sống còn lại
        if (lifeCounter > 0)
        {
            lifeCounter -= Time.deltaTime;
            // Nếu thời gian sống kết thúc, trả số sát thương về pool
            if (lifeCounter < 0)
            {
                DamageController.instance.PlaceInPool(this);
            }
        }

        // Di chuyển số sát thương lên trên để tạo hiệu ứng nổi
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    // Thiết lập số sát thương với giá trị được chỉ định
    public void Setup(int damageDisplay)
    {
        // Cập nhật lại thời gian sống và thiết lập văn bản số sát thương
        lifeCounter = lifetime;
        damageText.text = damageDisplay.ToString();
    }
}
