using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHubDisplay : MonoBehaviour
{
    [Header("Tắt Rigidbody khi thành NPC")]
    [SerializeField] public Rigidbody2D[] disableRigidbodies;

    [Header("Tắt các GameObject khi thành NPC (UI, hiệu ứng...)")]
    [SerializeField] public GameObject[] disableObjects;

    [Header("Tắt các GameObject khi là PLAYER (HUD, UI hub...)")]
    [SerializeField] public GameObject[] disableWhenPlayer;

    [Header("Tắt các Component khi thành NPC (PlayerController, PlayerInput...)")]
    [SerializeField] public Behaviour[] disableComponents;

    private string lastTag = "";

    private void Update()
    {
        if (gameObject.tag != lastTag)
        {
            lastTag = gameObject.tag;

            if (lastTag == "Player")
            {
                ActivateHubMode(); // Bật điều khiển
            }
            else
            {
                DeactivateHubMode(); // Tắt điều khiển
            }
        }
    }

    public void ActivateHubMode()
    {
        foreach (var rb in disableRigidbodies)
        {
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;
        }

        foreach (var go in disableObjects)
        {
            if (go != null)
                go.SetActive(true); // Hiện mấy thứ NPC cần
        }

        foreach (var go in disableWhenPlayer)
        {
            if (go != null)
                go.SetActive(false); // Tắt mấy thứ *chỉ NPC mới thấy*
        }

        foreach (var comp in disableComponents)
        {
            if (comp == null) continue;

            if (comp is PlayerInput input)
            {
                input.enabled = true;
                input.ActivateInput();
            }
            else
            {
                comp.enabled = true;
            }
        }

        Debug.Log($"[{name}] 🔥 Activated hub mode (Player)");
    }

    public void DeactivateHubMode()
    {
        foreach (var rb in disableRigidbodies)
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        foreach (var go in disableObjects)
        {
            if (go != null)
                go.SetActive(false);
        }

        foreach (var go in disableWhenPlayer)
        {
            if (go != null)
                go.SetActive(true); // Bật lại cho NPC
        }

        foreach (var comp in disableComponents)
        {
            if (comp == null) continue;

            if (comp is PlayerInput input)
            {
                input.DeactivateInput();
                input.enabled = false;
            }
            else
            {
                comp.enabled = false;
            }
        }

        Debug.Log($"[{name}] 💤 Deactivated hub mode (NPC)");
    }
}
