using UnityEngine;

public class CharacterSelectSign : MonoBehaviour
{
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Thực hiện hành động đổi nhân vật (nếu có)
            Debug.Log("Đổi nhân vật tại tế đàn!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            CharacterSelectHintUI.instance.ShowHint();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            CharacterSelectHintUI.instance.HideHint();
        }
    }
}

