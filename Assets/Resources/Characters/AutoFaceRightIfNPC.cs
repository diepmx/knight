using UnityEngine;

public class AutoFaceRightIfNPC : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Nếu không phải là Player, đảm bảo luôn quay mặt phải
        if (gameObject.tag != "Untagged" && spriteRenderer != null)
        {
            spriteRenderer.flipX = false;
        }
    }
}
