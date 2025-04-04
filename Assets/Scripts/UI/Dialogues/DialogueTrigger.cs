using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    // Đối tượng Dialogue chứa thông tin về đoạn hội thoại
    public Dialogue dialogue;

    // Biến boolean để kiểm tra xem người chơi có trong phạm vi hay không
    public bool isInRange;

    // Gọi khi collider khác đi vào vùng trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu collider là người chơi
        if (collision.CompareTag("Player"))
        {
            isInRange = true; // Đặt isInRange thành true khi người chơi vào khu vực
        }
    }

    // Gọi khi collider khác rời khỏi vùng trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Kiểm tra nếu collider là người chơi
        if (collision.CompareTag("Player"))
        {
            isInRange = false; // Đặt isInRange thành false khi người chơi rời khỏi khu vực
        }
    }

    // Kích hoạt đoạn hội thoại
    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue); // Gọi phương thức StartDialogue để bắt đầu hội thoại
    }
}
