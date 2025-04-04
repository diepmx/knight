using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [Space(10)]
    // Tốc độ của viên đạn.
    public float moveSpeed;

    void Update()
    {
        // Di chuyển viên đạn về phía trước.
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
