using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    [Space(10)]
    // Khoảng thời gian để thay đổi trạng thái của chữ (blink interval).
    public float blinkInterval = 1.0f;

    // Thành phần TextMeshPro để hiển thị văn bản.
    private TextMeshProUGUI textMesh;

    // Phương thức Start gọi khi script bắt đầu.
    public void Start()
    {
        // Lấy thành phần TextMeshProUGUI từ đối tượng gắn script.
        textMesh = GetComponent<TextMeshProUGUI>();
        // Bắt đầu coroutine để thực hiện hiệu ứng blink.
        StartCoroutine(BlinkText());
    }

    // Coroutine để làm chữ nhấp nháy (blink).
    IEnumerator BlinkText()
    {
        // Vòng lặp vô hạn để thay đổi trạng thái hiển thị của chữ.
        while (true)
        {
            // Lật ngược trạng thái hiển thị của văn bản (bật/tắt).
            textMesh.enabled = !textMesh.enabled;
            // Đợi một khoảng thời gian (blinkInterval) trước khi tiếp tục thay đổi.
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
