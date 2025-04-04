using UnityEngine;

public class InfiniteGrid : MonoBehaviour
{
    [Space(10)]
    // Mục tiêu cần theo dõi (thường là người chơi hoặc camera).
    public Transform target;

    [Space(10)]
    // Giá trị khoảng cách để "bắt lưới" (snap).
    public float snap = 2f;

    void Update()
    {
        // Tính toán vị trí "bắt lưới" dựa trên vị trí của mục tiêu và giá trị snap.
        Vector2 position = new Vector2(
            Mathf.Round(target.position.x / snap) * snap,
            Mathf.Round(target.position.y / snap) * snap
        );

        // Cập nhật vị trí của đối tượng lưới theo vị trí đã "bắt lưới".
        transform.position = position;
    }
}
