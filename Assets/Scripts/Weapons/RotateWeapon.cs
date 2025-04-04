using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    [Space(10)]
    // Tốc độ quay.
    public float rotateSpeed = 360f;

    void Update()
    {
        // Quay vũ khí.
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
    }
}
