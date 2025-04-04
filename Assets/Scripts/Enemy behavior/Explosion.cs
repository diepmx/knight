using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Hiệu ứng nổ sẽ kích hoạt khi đối tượng bị hủy.
    public GameObject deathEffect;

    void OnDestroy()
    {
        // Tạo hiệu ứng nổ tại vị trí của đối tượng khi nó bị hủy.
        Instantiate(deathEffect, transform.position, Quaternion.identity);
    }
}
