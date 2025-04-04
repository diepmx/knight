using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Biến instance dùng để tạo mô hình Singleton, giúp truy cập dễ dàng từ các script khác
    public static CameraShake instance;

    [Space(10)]
    // Thời gian rung của camera
    public float shakeDuration = 0.1f;
    // Cường độ rung của camera
    public float shakeAmount = 0.2f;

    // Biến kiểm tra xem camera có đang rung không
    private bool isShaking = false;

    void Awake()
    {
        // Gán instance này là đối tượng hiện tại của CameraShake
        instance = this;
    }

    // Coroutine thực hiện hiệu ứng rung
    private IEnumerator Shake(float shakeDuration, float shakeAmount)
    {
        // Nếu camera đang rung, thoát khỏi coroutine để tránh trùng lặp
        if (isShaking)
        {
            yield return null;
        }
        isShaking = true;

        // Lưu vị trí ban đầu của camera
        Vector3 originalCameraPosition = transform.localPosition;

        // Biến theo dõi thời gian đã trôi qua kể từ khi rung bắt đầu
        float elapsed = 0.0f;

        // Lặp lại hiệu ứng rung cho đến khi thời gian rung đạt giới hạn
        while (elapsed < shakeDuration)
        {
            // Tạo giá trị ngẫu nhiên cho vị trí X và Y dựa trên cường độ rung
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            // Cập nhật vị trí của camera với các giá trị mới, giữ nguyên vị trí Z      
            transform.localPosition = new Vector3(originalCameraPosition.x + x, originalCameraPosition.y + y, originalCameraPosition.z);

            // Cộng dồn thời gian đã trôi qua
            elapsed += Time.deltaTime;

            // Chờ đến khung hình tiếp theo trước khi tiếp tục vòng lặp
            yield return null;
        }

        // Khi thời gian rung kết thúc, đặt lại vị trí ban đầu của camera
        transform.localPosition = originalCameraPosition;
        // Đặt lại biến kiểm tra trạng thái rung
        isShaking = false;
    }

    // Phương thức công khai để bắt đầu hiệu ứng rung với thời gian và cường độ tùy chỉnh
    public void ShakeIt(float shakeDuration, float shakeAmount)
    {
        // Bắt đầu coroutine hiệu ứng rung với các tham số được cung cấp
        StartCoroutine(Shake(shakeDuration, shakeAmount));
    }
}
