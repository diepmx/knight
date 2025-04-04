using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [Space(10)]
    // Component Rigidbody2D.
    public Rigidbody2D theRB;

    [Space(10)]
    // Lực ném theo hướng X.
    public float throwPowerX;
    // Lực ném theo hướng Y.
    public float throwPowerY;

    [Space(10)]
    // Tốc độ quay.
    public float rotateSpeed;

    void Start()
    {
        // Thiết lập vận tốc ban đầu cho việc ném.
        theRB.velocity = new Vector2(Random.Range(-throwPowerX, throwPowerX), throwPowerY);
    }

    void Update()
    {
        // Quay vũ khí dựa trên vận tốc.
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (rotateSpeed * 360f * Time.deltaTime * Mathf.Sign(theRB.velocity.x)));
    }
}
