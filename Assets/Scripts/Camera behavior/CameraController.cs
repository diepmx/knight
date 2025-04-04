using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Tham chiếu đến Transform của đối tượng mục tiêu (trong trường hợp này là người chơi)
    private Transform target;

    private void Awake()
    {
        // Tìm và gán Transform của PlayerController làm mục tiêu theo dõi
        target = FindObjectOfType<PlayerController>().transform;
    }

    private void FixedUpdate()
    {
        // Cập nhật vị trí của camera để khớp với vị trí X và Y của mục tiêu,
        // nhưng giữ nguyên vị trí Z của chính nó để tránh thay đổi góc nhìn không mong muốn
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
